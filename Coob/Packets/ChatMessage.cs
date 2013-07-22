using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coob.CoobEventArgs;

namespace Coob.Packets
{
    public partial class Packet
    {
        public class ChatMessage : Base
        {
            public string Message;

            public ChatMessage(string message, Client client)
                : base(client)
            {
                this.Message = message;
            }

            public static Base Parse(Client client)
            {
                int length = client.Reader.ReadInt32();
                string message = Encoding.Unicode.GetString(client.Reader.ReadBytes(length * 2));
                return new ChatMessage(message, client);
            }

            public override bool CallScript()
            {
                var chatArgs = Root.ScriptManager.CallEvent("OnChatMessage", new ChatEventArgs(Sender, Message)) as ChatEventArgs;

                Message = chatArgs.Message;
                return !chatArgs.Canceled;
            }

            public override void Process()
            {
                Root.Coob.BroadcastChat(Sender.ID, Message);
            }
        }

    }
}
