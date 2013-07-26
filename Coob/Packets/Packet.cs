using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coob.Packets
{
    public partial class Packet
    {
        public abstract class Base
        {
            public Client Sender;

            public Base(Client client)
            {
                Sender = client;
            }

            public string PacketTypeName
            {
                get
                {
                    return this.GetType().Name;
                }
            }

            public abstract bool CallScript();
            public abstract void Process();
        }
    }

    public enum CSPacketIDs : int
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

    public enum SCPacketIDs : int
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
