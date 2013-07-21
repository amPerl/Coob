using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coob.Packets;

namespace Coob.CoobEventArgs
{
    public class InteractEventArgs : ScriptEventArgs
    {
        public InteractEventArgs(Client client, Packet.Interact packet) : base(client)
        {
            
        }
    }
}
