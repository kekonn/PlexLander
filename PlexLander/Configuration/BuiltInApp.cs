using System;

namespace PlexLander.Configuration
{
    public class BuiltInApp
    {
        /// <summary>
        /// internal app id's go down from -1
        /// </summary>
        private const int ID_UPPER_RANGE = -1;

        /// <summary>
        /// The ID, mainly used for sorting, is a negative number
        /// </summary>
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Icon { get; private set; }
        public string Url { get; private set; }
        public string Endpoint { get; private set; }
        public string Token { get; private set; }

        /// <summary>
        /// Constructor for the BuiltInApp
        /// </summary>
        /// <param name="name">The user readable name of the app</param>
        /// <param name="icon">The url for the icon. Is always local</param>
        /// <param name="url">The url where the app can be found. See remarks.</param>
        /// <param name="endpoint">When the url is local, this is the controller name. See remarks</param>
        /// <param name="token">Optional token if the url needs one</param>
        /// <param name="id">Optional ID for sorting if necessary</param>
        /// <remarks>When URL is local, we look at the endpoint for the controllername. If the url is defined completely it is assumed to not be local, even if it is</remarks>
        public BuiltInApp(string name, string icon, string url, string endpoint = null, string token = null, int? id = null)
        {
            Name = name ?? throw new ArgumentNullException("name");
            Icon = icon ?? throw new ArgumentNullException("icon");
            Url = url ?? throw new ArgumentNullException("url");
            Endpoint = endpoint;
            Token = token;

            if (id.HasValue)
            {
                if (id.Value > ID_UPPER_RANGE)
                    throw new ArgumentOutOfRangeException("id", String.Format("id must me smaller than or equal to {0}",ID_UPPER_RANGE));

                Id = id.Value;
            } else
            {
                Id = ID_UPPER_RANGE;
            }
        }
    }
}
