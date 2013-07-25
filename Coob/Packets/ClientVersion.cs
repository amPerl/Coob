using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coob.CoobEventArgs;

namespace Coob.Packets
{
    public partial class Packet
    {
        public class ClientVersion : Base
        {
            public int Version;

            private ClientVersion(int version, Client client)
                : base(client)
            {
                this.Version = version;
            }

            public static Base Parse(Client client, Coob coob)
            {
                int version = client.Reader.ReadInt32();
                return new ClientVersion(version, client);
            }

            public override bool CallScript()
            {
                return Root.ScriptManager.CallEvent("OnClientVersion", new ClientVersionEventArgs(Sender, Version)).Canceled == false;
            }

            public override void Process()
            {
                if (Version != Globals.ServerVersion)
                {
                    Sender.Writer.Write(SCPacketIDs.ServerMismatch);
                    Sender.Disconnect("Invalid version");
                    return;
                }
                else if (Sender.Coob.Clients.Values.Count >= Globals.MaxConcurrentPlayers)
                {
                    Sender.Writer.Write(SCPacketIDs.ServerFull);
                    Sender.Disconnect("Server full");
                    return;
                }
              
                    Sender.Writer.Write(SCPacketIDs.Join); // ServerData
                    Sender.Writer.Write(0);
                    Sender.Writer.Write(Sender.ID);
                    Sender.Writer.Write(new byte[0x1168]);

                    Sender.Writer.Write(SCPacketIDs.SeedData);
                    Sender.Writer.Write(Sender.Coob.World.Seed);
                
            }
        }
    }
}
