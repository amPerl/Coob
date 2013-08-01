namespace Coob.CoobEventArgs
{
    public class ClientDisconnectEventArgs : ScriptEventArgs
    {
        public string Reason { get; private set; }

        public ClientDisconnectEventArgs(Client client, string reason)
            : base(client)
        {
            Reason = reason;
        }
    }
}
