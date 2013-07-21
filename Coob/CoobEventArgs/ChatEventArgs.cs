using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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