namespace PlexLander.Plex
{
    public class LoginResult
    {
        public bool Succes { get; set; }
        public string Error { get; set; }
        public PlexUser User { get; internal set; }

        public override string ToString()
        {
            return $"Succes: {Succes} - Token: {User.Token} - Error: {Error}";
        }
    }
}
