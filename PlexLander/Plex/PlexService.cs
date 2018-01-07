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

namespace PlexLander.Plex
{
    public interface IPlexService : IDisposable
    {
        Task<LoginResult> Login(string username, string password);
    }

    public class PlexService : IPlexService
    {
        // how to find plex token: https://support.plex.tv/hc/en-us/articles/204059436

        #region Plex header constants
        private const string X_PLEX_TOKEN_HEADER = "X-Plex-Token";
        private const string X_PLEX_PLATFORM_HEADER = "X-Plex-Platform";
        private const string X_PLEX_PLATFORM_VERSION_HEADER = "X-Plex-Platform-Version";
        private const string X_PLEX_CLIENT_IDENTIFIER_HEADER = "X-Plex-Client-Identifier";
        private const string X_PLEX_PRODUCT_HEADER = "X-Plex-Product";
        private const string X_PLEX_VERSION_HEADER = "X-Plex-Version";
        private const string X_PLEX_DEVICE_HEADER = "X-Plex-Device";
        private const string X_PLEX_CONTAINER_START_HEADER = "X-Plex-Container-Start";
        private const string X_PLEX_CONTAINER_SIZE_HEADER = "X-Plex-Container-Size";
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

        private static HttpClient client;
        private string apiEndpoint = "https://app.plex.tv/";
        private Configuration.IConfigurationManager configManager;

        public PlexService(Configuration.IConfigurationManager configManager)
        {
            this.configManager = configManager ?? throw new ArgumentNullException("configManager");

            if (!configManager.IsPlexEnabled)
                throw new ApplicationException("Please enable and configure Plex.");


            if (client == null)
            {
                client = new HttpClient();
            }

            if (!string.IsNullOrEmpty(configManager.PlexApp.Endpoint))
            {
                apiEndpoint = configManager.PlexApp.Endpoint;
            }

            client.BaseAddress = new Uri(apiEndpoint);
            SetupRequestHeaders(configManager);
        }

        private void SetupRequestHeaders(Configuration.IConfigurationManager configManager)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add(X_PLEX_PRODUCT_HEADER, configManager.ApplicationName);
            client.DefaultRequestHeaders.Add(X_PLEX_VERSION_HEADER, configManager.ApplicationVersion);
            client.DefaultRequestHeaders.Add(X_PLEX_DEVICE_HEADER, configManager.DeviceName);
            client.DefaultRequestHeaders.Add(X_PLEX_CLIENT_IDENTIFIER_HEADER, GetClientIdentifier());
            client.DefaultRequestHeaders.Add(X_PLEX_PLATFORM_HEADER, configManager.PlatformName);
            client.DefaultRequestHeaders.Add(X_PLEX_PLATFORM_VERSION_HEADER, configManager.PlatformVersion);
        }

        /// <summary>
        /// Sets the headers that do the paging
        /// </summary>
        /// <param name="start">at what item the page should start</param>
        /// <param name="count">the amount of items to return (or less if less are available)</param>
        private void SetPlexPage(int start, int count = 10)
        {
            client.DefaultRequestHeaders.Remove(X_PLEX_CONTAINER_SIZE_HEADER);
            client.DefaultRequestHeaders.Remove(X_PLEX_CONTAINER_START_HEADER);
            client.DefaultRequestHeaders.Add(X_PLEX_CONTAINER_START_HEADER, start.ToString());
            client.DefaultRequestHeaders.Add(X_PLEX_CONTAINER_SIZE_HEADER, count.ToString());
        }

        #region Login
        public async Task<LoginResult> Login(string userName, string password)
        {
            //HttpClient setup
            var loginClient = new HttpClient();
            loginClient.BaseAddress = new Uri(PLEX_LOGIN_BASE);
            loginClient.DefaultRequestHeaders.Add(X_PLEX_CLIENT_IDENTIFIER_HEADER, GetClientIdentifier());
            loginClient.DefaultRequestHeaders.Add(X_PLEX_PRODUCT_HEADER, configManager.ApplicationName);
            loginClient.DefaultRequestHeaders.Add(X_PLEX_VERSION_HEADER, configManager.ApplicationVersion);
            loginClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

            var formContent = new Dictionary<string, string> {
                {PLEX_LOGIN_USER_NAME, userName },
                {PLEX_LOGIN_USER_PASSWORD, password }
            };
            HttpContent content = new FormUrlEncodedContent(formContent);
            var response = await loginClient.PostAsync(PLEX_LOGIN_ENDPOINT, content);
            if (response.IsSuccessStatusCode)
            {
                content = response.Content;
                var document = XDocument.Parse(await content.ReadAsStringAsync());
                //TODO: finish parsing the XML

                return new LoginResult { Succes = true };
            }
            else
            {
                return new LoginResult { Succes = false, Error = response.StatusCode.ToString() };
            }

        }
        #endregion

        /// <summary>
        /// Makes an SHA512 hash of the application name, version number and device name
        /// </summary>
        /// <returns></returns>
        private string GetClientIdentifier()
        {
            string salt = String.Format("{0}-{1}-{2}", configManager.ApplicationName, configManager.ApplicationVersion, configManager.DeviceName);
            byte[] saltyBytes = Encoding.UTF8.GetBytes(salt);
            byte[] clientIdentifierBytes;
            using (var sha512 = new SHA512Managed())
            {
                clientIdentifierBytes = sha512.ComputeHash(saltyBytes);
            }

            saltyBytes = null;
            salt = null;

            return Encoding.UTF8.GetString(clientIdentifierBytes);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (client != null)
                    {
                        client.Dispose();
                    }
                }

                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }

    public class LoginResult
    {
        public string Token { get; set; }
        public bool Succes { get; set; }
        public string Error { get; set; }
    }
}
