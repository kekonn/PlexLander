namespace PlexLander.Plex
{
    public class LoginResult
    {
        public bool Succes { get; set; }
        public string Error { get; set; }
        public PlexUser User { get; internal set; }

        public override string ToString()
        {
            if (Succes)
                return $"Succes - User {User.Username}";
            else
                return $"Error - {Error}";
        }
    }
}
