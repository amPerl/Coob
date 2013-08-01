using Coob.Packets;

namespace Coob.CoobEventArgs
{
    public class InteractEventArgs : ScriptEventArgs
    {
        public InteractEventArgs(Client client, Packet.Interact packet)
            : base(client)
        {
            
        }
    }
}
