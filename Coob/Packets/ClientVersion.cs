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
                Version = version;
            }

            public static Base Parse(Client client, Coob coob)
            {
                return new ClientVersion(client.Reader.ReadInt32(), client);
            }

            public override bool CallScript()
            {
                return !Program.ScriptManager.CallEvent("OnClientVersion", new ClientVersionEventArgs(Sender, Version)).Canceled;
            }

            public override void Process()
            {
                if (Version != Globals.ServerVersion)
                {
                    Sender.Writer.Write(SCPacketIDs.ServerMismatch);
                    Sender.Disconnect("Invalid version");
                    return;
                }

                if (Sender.Coob.Clients.Values.Count >= Globals.MaxConcurrentPlayers)
                {
                    Sender.Writer.Write(SCPacketIDs.ServerFull);
                    Sender.Disconnect("Server full");
                    return;
                }

                // ServerData
                Sender.Writer.Write(SCPacketIDs.Join);
                Sender.Writer.Write(0);
                Sender.Writer.Write(Sender.Id);
                Sender.Writer.Write(new byte[0x1168]);

                Sender.Writer.Write(SCPacketIDs.SeedData);
                Sender.Writer.Write(Sender.Coob.World.Seed);
            }
        }
    }
}
