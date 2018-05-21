using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PlexLander.Models;
using System.Text;
using System.Xml.Linq;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace PlexLander.Plex
{
    public interface IPlexService
    {
        Task<LoginResult> Login(string username, string password);
        bool HasValidLogin { get; }
        Task<IEnumerable<PlexServer>> GetPlexServerAsync();
    }

    public class PlexService : IPlexService
    {
        // how to find plex token: https://support.plex.tv/hc/en-us/articles/204059436
        // api description

        #region Logging
        private readonly ILogger<IPlexService> _logger;
        #endregion 

        #region Plex header constants
        private const string X_PLEX_TOKEN_HEADER = "X-Plex-Token";
        private const string X_PLEX_PLATFORM_HEADER = "X-Plex-Platform";
        private const string X_PLEX_PLATFORM_VERSION_HEADER = "X-Plex-Platform-Version";
        private const string X_PLEX_CLIENT_IDENTIFIER_HEADER = "X-Plex-Client-Identifier";
        private const string X_PLEX_PRODUCT_HEADER = "X-Plex-Product";
        private const string X_PLEX_VERSION_HEADER = "X-Plex-Version";
        private const string X_PLEX_DEVICE_HEADER = "X-Plex-Device";
        #endregion

        #region Login constants
        private const string PLEX_LOGIN_BASE = "https://plex.tv/";
        private const string PLEX_LOGIN_ENDPOINT = "users/sign_in.xml";
        private const string HTTP_AUTHORIZATION = "Authorization";
        private const string HTTP_BASIC_FORMAT = "Basic {0}";
        private readonly TimeSpan SessionTimeout = new TimeSpan(10, 0, 0, 0, 0);
        #endregion

        #region Plex.tv constants
        private const string PLEX_TV_SERVERS_ENDPOINT = "pms/servers.xml";
        private const string PLEX_TV_ACCOUNT_ENDPOINT = "users/account.xml";
        private const string PLEX_TV_BASE = "https://plex.tv/";
        #endregion

        private string apiEndpoint = "https://app.plex.tv/";
        private readonly Configuration.IConfigurationManager _configManager;
        private readonly Data.IPlexSessionRepository _sessionRepo;
        private Tuple<DateTime, LoginResult> _lastLoginResult;

        #region Properties
        public bool HasValidLogin
        {
            get
            {
                return _lastLoginResult != null
                    && (DateTime.Now.Date - _lastLoginResult.Item1) <= SessionTimeout
                    && _lastLoginResult.Item2.Succes == true;
            }
        }
        #endregion

        public PlexService(Configuration.IConfigurationManager configManager, Data.IPlexSessionRepository sessionRepo, ILogger<IPlexService> logger)
        {
            _configManager = configManager ?? throw new ArgumentNullException("configManager");
            if (!configManager.IsPlexEnabled)
                throw new ApplicationException("Please enable and configure Plex.");

            _sessionRepo = sessionRepo;

            _logger = logger;

            if (!string.IsNullOrEmpty(configManager.PlexApp.Endpoint))
            {
                apiEndpoint = configManager.PlexApp.Endpoint;
            }

            LoadLastLogin();
        }

        private void LoadLastLogin()
        {
            var lastSession = _sessionRepo.GetLastSession();

            if (lastSession == null)
                return;

            _lastLoginResult = new Tuple<DateTime, LoginResult>(lastSession.SessionStart, new LoginResult() {
                Succes = true,
                User = new PlexUser
                {
                    Email = lastSession.Email,
                    Thumbnail = lastSession.Thumbnail,
                    Token = lastSession.Token,
                    Username = lastSession.Username
                }
            });
        }

        #region HttpClient building
        private HttpClient GetBaseClient(string baseAddress)
        {
            string endpoint;
            if (string.IsNullOrWhiteSpace(baseAddress) && !string.IsNullOrWhiteSpace(apiEndpoint))
            {
                endpoint = apiEndpoint;
            }
            else if (!string.IsNullOrWhiteSpace(baseAddress))
            {
                endpoint = baseAddress;
            }
            else
                throw new InvalidOperationException("No endpoint is defined. Set the default endpoint or pass an endpoint as a parameter.");

            var client = new HttpClient()
            {
                BaseAddress = new Uri(endpoint)
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            client.DefaultRequestHeaders.Add(X_PLEX_PRODUCT_HEADER, _configManager.ApplicationName);
            client.DefaultRequestHeaders.Add(X_PLEX_VERSION_HEADER, _configManager.ApplicationVersion);
            client.DefaultRequestHeaders.Add(X_PLEX_CLIENT_IDENTIFIER_HEADER, GetClientIdentifier());

            return client;
        }

        private HttpClient GetPlexClient(string baseAddress)
        {
            var client = GetBaseClient(baseAddress);

            //set all the default headers
            client.DefaultRequestHeaders.Add(X_PLEX_DEVICE_HEADER, _configManager.DeviceName);
            client.DefaultRequestHeaders.Add(X_PLEX_PLATFORM_HEADER, _configManager.PlatformName);
            client.DefaultRequestHeaders.Add(X_PLEX_PLATFORM_VERSION_HEADER, _configManager.PlatformVersion);

            LoadLastLogin(); //ensures all objects present when there is still a valid session

            if (_lastLoginResult != null) // if null, no valid session
            {
                client.DefaultRequestHeaders.Add(X_PLEX_TOKEN_HEADER, _lastLoginResult.Item2.User.Token);
            } else
            {
                throw new InvalidOperationException("Please create a valid session first. The previous session has likely expired.");
            }

            return client;
        }

        private HttpClient GetPlexTvClient()
        {
            return GetPlexClient(PLEX_TV_BASE);
        }

        private HttpClient GetLoginClient()
        {
            return GetBaseClient(PLEX_LOGIN_BASE);
        }
        #endregion

        #region Login
        public async Task<LoginResult> Login(string userName, string password)
        {
            if (HasValidLogin) //we can always change this timeout later, for now it is hardcoded
            {
                // we've already logged on succesfully in the last 10 days
                return _lastLoginResult.Item2;
            }
            //HttpClient setup
            var loginClient = GetLoginClient();

            var authString = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes($"{userName}:{password}"));
            loginClient.DefaultRequestHeaders.Add(HTTP_AUTHORIZATION, string.Format(HTTP_BASIC_FORMAT, authString));
            var response = await loginClient.PostAsync(PLEX_LOGIN_ENDPOINT, null);

            if (response.IsSuccessStatusCode) // GREAT SUCCES!
            {
                var document = XDocument.Parse(await response.Content.ReadAsStringAsync());

                var plexUser = GetPlexUserFromXml(document);

                _lastLoginResult = new Tuple<DateTime, LoginResult>(DateTime.Now, new LoginResult() { Succes = true, User = plexUser });
            }
            else // *sad trombone*
            {
                _lastLoginResult = new Tuple<DateTime, LoginResult>(DateTime.Now, new LoginResult { Succes = false, Error = response.StatusCode.ToString() });
            }

            _logger.LogTrace("Login result: {0}", _lastLoginResult.Item2.ToString());
            return _lastLoginResult.Item2;
        }

        private PlexUser GetPlexUserFromXml(XDocument doc)
        {
            if (doc == null)
                throw new ArgumentNullException("doc");

            var root = doc.Element("user");
            return new PlexUser
            {
                Email = (string)root.Attribute("email"),
                Username = (string)root.Attribute("username"),
                Thumbnail = (string)root.Attribute("thumb"),
                Token = (string)root.Attribute("authToken")
            };
        }
        #endregion

        #region Servers
        public async Task<IEnumerable<PlexServer>> GetPlexServerAsync()
        {
            if (!HasValidLogin)
                throw new InvalidOperationException("Please get a valid session from Plex.TV first");


            using (var client = GetPlexTvClient())
            {
                var result = await client.GetAsync(PLEX_TV_SERVERS_ENDPOINT);

                if (result.IsSuccessStatusCode)
                {
                    var serversDoc = XDocument.Parse(await result.Content.ReadAsStringAsync());
                    var servers = GetPlexServersFromXML(serversDoc);
                    return servers;
                } else
                {
                    throw new InvalidOperationException($"The request has failed with the following status: {result.StatusCode.ToString()}.");
                }
            }
        }

        private List<PlexServer> GetPlexServersFromXML(XDocument serversDoc)
        {
            if (serversDoc == null)
                throw new ArgumentNullException("serversDoc");

            var root = serversDoc.Element("MediaContainer");
            var serverList = new List<PlexServer>(5);
            foreach (var serverNode in root.Descendants())
            {
                var server = new PlexServer()
                {
                    Name = (string)serverNode.Attribute("name"),
                    AccessToken = (string)serverNode.Attribute("accessToken"),
                    Address = (string)serverNode.Attribute("address"),
                    Port = (string)serverNode.Attribute("port"),
                    Scheme = (string)serverNode.Attribute("scheme"),
                    Owned = (bool)serverNode.Attribute("owned"),
                    LastUpdated = new DateTime(1970, 1, 1).AddSeconds((double)serverNode.Attribute("updatedAt")),
                    UsesPlexHome = (serverNode.Attribute("home") == null || (serverNode.Attribute("home") != null && ((string)serverNode.Attribute("home")).Equals("1")))
                };

                var localAddresses = (string)serverNode.Attribute("localAddresses");
                if (!string.IsNullOrEmpty(localAddresses))
                {
                    server.LocalAddresses = new List<string>(localAddresses.Split(','));
                } else
                {
                    server.LocalAddresses = new List<string>(0);
                }

                serverList.Add(server);
            }

            return serverList;
        }
        #endregion

        /// <summary>
        /// Makes an SHA512 hash of the application name and device name
        /// </summary>
        /// <returns></returns>
        private string GetClientIdentifier()
        {
            string salt = String.Format("{0}-{1}", _configManager.ApplicationName, _configManager.DeviceName);
            byte[] saltyBytes = Encoding.UTF8.GetBytes(salt);
            byte[] clientIdentifierBytes;
            using (var sha512 = new SHA512Managed())
            {
                clientIdentifierBytes = sha512.ComputeHash(saltyBytes);
            }

            saltyBytes = null;
            salt = null;

            var sb = new StringBuilder();
            foreach (var byteChar in clientIdentifierBytes)
            {
                sb.Append(byteChar.ToString("x2"));
            }

            _logger.LogTrace("Client Identifier: {0}", sb.ToString());
            return sb.ToString();
        }
    }

    static class HttpClientExtensions
    {
        private const string X_PLEX_CONTAINER_START_HEADER = "X-Plex-Container-Start";
        private const string X_PLEX_CONTAINER_SIZE_HEADER = "X-Plex-Container-Size";

        /// <summary>
        /// Sets the headers that do the paging
        /// </summary>
        /// <param name="start">at what item the page should start</param>
        /// <param name="count">the amount of items to return (or less if less are available)</param>
        private static void SetPlexPage(this HttpClient client, int? start = 1, int? count = 10)
        {
            if (start.HasValue)
            {
                client.DefaultRequestHeaders.Remove(X_PLEX_CONTAINER_START_HEADER);
                client.DefaultRequestHeaders.Add(X_PLEX_CONTAINER_START_HEADER, start.Value.ToString());
            }
            if (count.HasValue)
            {
                client.DefaultRequestHeaders.Remove(X_PLEX_CONTAINER_SIZE_HEADER);
                client.DefaultRequestHeaders.Add(X_PLEX_CONTAINER_SIZE_HEADER, count.ToString());
            }
        }
    }
}
