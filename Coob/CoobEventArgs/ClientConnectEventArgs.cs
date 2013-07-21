namespace Coob.CoobEventArgs
{
    public class ClientConnectEventArgs : ScriptEventArgs
    {
        public string IP { get; private set; }

        public ClientConnectEventArgs(string ip) : base(null)
        {
            IP = ip;
        }
    }
}