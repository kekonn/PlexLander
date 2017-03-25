namespace PlexLander.ViewModels
{
    public abstract class ViewModelBase
    {
        public string ServerName { get; private set; }
        public string ActiveControllerName { get; set; } = null;

        public ViewModelBase(string serverName)
        {
            ServerName = serverName;
        }
    }
}
