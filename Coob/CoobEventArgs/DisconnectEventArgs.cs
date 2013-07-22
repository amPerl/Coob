using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coob.CoobEventArgs
{
    class DisconnectEventArgs : ScriptEventArgs
    {
        public DisconnectEventArgs(Client client) : base(client) { }
    }
}
