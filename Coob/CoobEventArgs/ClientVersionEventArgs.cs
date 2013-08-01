namespace Coob.CoobEventArgs
{
    public class ClientVersionEventArgs : ScriptEventArgs
    {
        public int Version { get; private set; }

        public ClientVersionEventArgs(Client client, int version)
            : base(client)
        {
            Version = version;
        }
    }
}
