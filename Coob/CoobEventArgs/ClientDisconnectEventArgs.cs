using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coob.CoobEventArgs
{
    public class ClientDisconnectEventArgs : ScriptEventArgs
    {
        public string Reason { get; private set; }

        public ClientDisconnectEventArgs(Client client, string reason) : base(client)
        {
            Reason = reason;
        }
    }
}