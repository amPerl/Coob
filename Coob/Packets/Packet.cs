using System.IO;

namespace Coob.Packets
{
    public partial class Packet
    {
        public abstract class Base
        {
            public Client Sender;

            protected Base(Client client)
            {
                Sender = client;
            }

            public string PacketTypeName
            {
                get
                {
                    return GetType().Name;
                }
            }

            public abstract bool CallScript();
            public abstract void Process();
            public virtual void Write(BinaryWriter writer)
            {

            }
        }
    }

    public enum CSPacketIDs
    {
        EntityUpdate = 0,
        Interact = 6,
        Hit = 7,
        Stealth = 8,
        Shoot = 9,
        ClientChatMessage = 10,
        ChunkDiscovered = 11,
        SectorDiscovered = 12,
        ClientVersion = 17,
    }

    public enum SCPacketIDs
    {
        EntityUpdate = 0,
        MultipleEntityUpdate = 1, // Not used
        UpdateFinished = 2,
        Unknown3 = 3,
        ServerUpdate = 4,
        CurrentTime = 5,
        ServerChatMessage = 10,
        SeedData = 15,
        Join = 16,
        ServerFull = 18,
        ServerMismatch = 19,
    }
}
