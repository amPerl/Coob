using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coob.Structures;

namespace Coob.Packets
{
    public partial class Packet
    {

        public enum InteractType : byte
        {
            NPC = 2,
            Normal = 3,
            Pickup = 5,
            Drop = 6,
            Examine = 8
        }

        public class Interact : Base
        {
            public Item Item;
            public int ChunkX, ChunkY;
            public int Something3;
            public uint Something4;
            public InteractType Type;
            public byte Something6;
            public ushort Something7;

            public Interact(Client client)
                : base(client)
            {
            }

            public static Base Parse(Client client)
            {
                Item Item = new Item();
                Item.ReadNet(client.Reader);
                return new Interact(client)
                {
                    Item = Item,
                    ChunkX = client.Reader.ReadInt(),
                    ChunkY = client.Reader.ReadInt(),
                    Something3 = client.Reader.ReadInt(),
                    Something4 = client.Reader.ReadUInt(),
                    Type = (InteractType)client.Reader.ReadByte(),
                    Something6 = client.Reader.ReadByte(),
                    Something7 = client.Reader.ReadUShort()
                };
            }

            public override bool CallScript()
            {
                return (bool)Root.JavaScript.Engine.CallFunction("onInteract", this, Sender);
            }

            public override void Process()
            {

            }
        }
    }
}
