namespace Coob.CoobEventArgs
{
    public class ChatEventArgs : ScriptEventArgs
    {
        public string Message { get; set; }

        public ChatEventArgs(Client client, string message) : base(client)
        {
            Message = message;
        }
    }
}
