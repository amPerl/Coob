namespace Coob.CoobEventArgs
{
    public class ClientConnectEventArgs : ScriptEventArgs
    {
        public string Ip { get; private set; }

        public ClientConnectEventArgs(string ip)
            : base(null)
        {
            Ip = ip;
        }
    }
}
