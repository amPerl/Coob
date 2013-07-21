using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coob.CoobEventArgs
{
    class ClientJoinEventArgs : ScriptEventArgs
    {
        public ClientJoinEventArgs(Client client) : base(client) {}
    }
}
