﻿using System.Text;
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
                Message = message;
            }

            public static Base Parse(Client client, Coob coob)
            {
                int length = client.Reader.ReadInt32();
                string message = Encoding.Unicode.GetString(client.Reader.ReadBytes(length * 2));

                if (message.Length > Globals.MaxChatMessageLength)
                    message = message.Substring(0, Globals.MaxChatMessageLength);

                return new ChatMessage(message, client);
            }

            public override bool CallScript()
            {
                var chatArgs = (ChatEventArgs)Program.ScriptManager.CallEvent("OnChatMessage", new ChatEventArgs(Sender, Message));

                Message = chatArgs.Message;
                return !chatArgs.Canceled;
            }

            public override void Process()
            {
                Sender.Coob.World.BroadcastChat(Sender.Id, Message);
            }
        }
    }
}
