using System;

namespace PlexLander.Configuration
{
    public class BuiltInApp
    {
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
