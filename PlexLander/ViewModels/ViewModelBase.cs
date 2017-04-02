namespace PlexLander.ViewModels
{
    public abstract class ViewModelBase
    {
        public string ServerName { get; private set; }

        public ViewModelBase(string serverName)
        {
            ServerName = serverName;
        }
    }
}
