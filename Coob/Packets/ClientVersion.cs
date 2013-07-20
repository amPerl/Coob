using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            public static Base Parse(Client client)
            {
                int version = client.Reader.ReadInt32();
                return new ClientVersion(version, client);
            }

            public override bool CallScript()
            {
                return Root.Scripting.CallFunction<bool>("onClientVersion", Version, Sender);
            }

            public override void Process()
            {
                Sender.Writer.Write(SCPacketIDs.Join); // ServerData
                Sender.Writer.Write(0);
                Sender.Writer.Write(Sender.ID);
                Sender.Writer.Write(new byte[0x1168]);

                Sender.Writer.Write(SCPacketIDs.SeedData);
                Sender.Writer.Write(Root.Coob.Options.WorldSeed);
            }
        }
    }
}
