using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlexLander.Models;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography;
using Common.Logging;

namespace PlexLander.Plex
{
    public interface IPlexService
    {
        Task<LoginResult> Login(string username, string password);
        bool HasValidLogin { get; }
    }

    public class PlexService : IPlexService
    {
        // how to find plex token: https://support.plex.tv/hc/en-us/articles/204059436

        #region Logging
        ILog Log = LogManager.GetLogger("PlexService");
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
        private const string PLEX_LOGIN_USER_NAME = "user[name]";
        private const string PLEX_LOGIN_USER_PASSWORD = "user[password]";
        #endregion

        #region Plex.tv constants
        private const string PLEX_TV_SERVERS_ENDPOINT = "pms/servers.xml";
        private const string PLEX_TV_ACCOUNT_ENDPOINT = "users/account.xml";
        private const string PLEX_TV_BASE = "https://plex.tv/";
        #endregion
        
        private string apiEndpoint = "https://app.plex.tv/";
        private Configuration.IConfigurationManager configManager;
        private Tuple<DateTime, LoginResult> lastLoginResult;

        #region Properties
        public bool HasValidLogin
        {
            get
            {
                return lastLoginResult != null
                && (DateTime.Now - lastLoginResult.Item1).Days <= 10
                && lastLoginResult.Item2.Succes == true;
            }
        }
        #endregion

        public PlexService(Configuration.IConfigurationManager configManager)
        {
            this.configManager = configManager ?? throw new ArgumentNullException("configManager");

            if (!configManager.IsPlexEnabled)
                throw new ApplicationException("Please enable and configure Plex.");
            
            if (!string.IsNullOrEmpty(configManager.PlexApp.Endpoint))
            {
                apiEndpoint = configManager.PlexApp.Endpoint;
            }
            
        }

        private HttpClient GetPlexClient(string baseAddress)
        {
            string endpoint;
            if (string.IsNullOrWhiteSpace(baseAddress) && !string.IsNullOrWhiteSpace(apiEndpoint))
            {
                endpoint = apiEndpoint;
            } else if (!string.IsNullOrWhiteSpace(baseAddress))
            {
                endpoint = baseAddress;
            } else
                throw new InvalidOperationException("No endpoint is defined. Set the default endpoint or pass an endpoint as a parameter.");

            var client = new HttpClient()
            {
                BaseAddress = new Uri(endpoint)
            };

            //set all the default headers
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            client.DefaultRequestHeaders.Add(X_PLEX_PRODUCT_HEADER, configManager.ApplicationName);
            client.DefaultRequestHeaders.Add(X_PLEX_VERSION_HEADER, configManager.ApplicationVersion);
            client.DefaultRequestHeaders.Add(X_PLEX_DEVICE_HEADER, configManager.DeviceName);
            client.DefaultRequestHeaders.Add(X_PLEX_CLIENT_IDENTIFIER_HEADER, GetClientIdentifier());
            client.DefaultRequestHeaders.Add(X_PLEX_PLATFORM_HEADER, configManager.PlatformName);
            client.DefaultRequestHeaders.Add(X_PLEX_PLATFORM_VERSION_HEADER, configManager.PlatformVersion);

            return client;
        }

        #region Login
        public async Task<LoginResult> Login(string userName, string password)
        {
            if (HasValidLogin) //we can always change this timeout later, for now it is hardcoded
            {
                // we've already logged on succesfully in the last 10 days
                return lastLoginResult.Item2;
            }
            //HttpClient setup
            var loginClient = GetPlexClient(PLEX_LOGIN_BASE);
            loginClient.BaseAddress = new Uri(PLEX_LOGIN_BASE);

            var formContent = new Dictionary<string, string> {
                {PLEX_LOGIN_USER_NAME, userName },
                {PLEX_LOGIN_USER_PASSWORD, password }
            };
            HttpContent content = new FormUrlEncodedContent(formContent);
            Log.Debug(l => l("Calling {0}", PLEX_LOGIN_BASE + PLEX_LOGIN_ENDPOINT));
            Log.Debug(l => l("With content {0}", content.ToString()));

            var response = await loginClient.PostAsync(PLEX_LOGIN_ENDPOINT, content);
            Log.Debug(l => l("Got response:\n {0}", response.ToString()));

            if (response.IsSuccessStatusCode)
            {
                content = response.Content;
                var document = XDocument.Parse(await content.ReadAsStringAsync());
                //TODO: finish parsing the XML
                Log.Debug(l => l("Got response:"));
                Log.Debug(l => l(document.ToString()));

                lastLoginResult = new Tuple<DateTime, LoginResult>(DateTime.Now, new LoginResult() { Succes = true });
            }
            else
            {
                lastLoginResult = new Tuple<DateTime, LoginResult>(DateTime.Now, new LoginResult { Succes = false, Error = response.StatusCode.ToString() });
            }

            Log.Info(l => l("Login result: {0}", lastLoginResult.Item2.ToString()));
            return lastLoginResult.Item2;
        }
        #endregion

        /// <summary>
        /// Makes an SHA512 hash of the application name, version number and device name
        /// </summary>
        /// <returns></returns>
        private string GetClientIdentifier()
        {
            string salt = String.Format("{0}-{1}", configManager.ApplicationName, configManager.DeviceName);
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

            Log.Debug(l => l("Client Identifier: {0}", sb.ToString()));
            return sb.ToString();
        }
    }

    public class LoginResult
    {
        public string Token { get; set; }
        public bool Succes { get; set; }
        public string Error { get; set; }

        public override string ToString()
        {
            return $"Succes: {Succes} Token: {Token} Error: {Error}";
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
        private static void SetPlexPage(this HttpClient client, int start, int count = 10)
        {
            client.DefaultRequestHeaders.Remove(X_PLEX_CONTAINER_SIZE_HEADER);
            client.DefaultRequestHeaders.Remove(X_PLEX_CONTAINER_START_HEADER);
            client.DefaultRequestHeaders.Add(X_PLEX_CONTAINER_START_HEADER, start.ToString());
            client.DefaultRequestHeaders.Add(X_PLEX_CONTAINER_SIZE_HEADER, count.ToString());
        }
    }
}
