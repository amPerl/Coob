using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Coob.Packets
{
    /* Structure info mostly stolen from mat^2 ;) */

    public partial class Packet
    {
        public class Shoot : Base
        {
            public ulong EntityID;

            public int ChunkX;
            public int ChunkY;

            public uint something5;

            public QVector3 Position;

            public uint something13;
            public uint something14;
            public uint something15;

            public Vector3 Velocity;

            public float something19; // rand() something, probably damage multiplier.
            public float something20;
            public float something21;
            public float something22; // used stamina? amount of stun?
            public uint something23;
            public byte something24;
            public uint something25;
            public byte something26;
            public uint something27;
            public uint something28;

            public Shoot(Client client)
                : base(client)
            {
            }

            public override bool CallScript()
            {
                return true;
            }

            public override void Process()
            {
                //BinaryWriter writer = Sender.Writer;
                //
                //writer.Write(EntityID);
                //writer.Write(ChunkX);
                //writer.Write(ChunkY);
                //writer.Write(something5);
                //writer.Pad(4);
                //writer.Write(Position);
                //writer.Write(something13);
                //writer.Write(something14);
                //writer.Write(something15);
                //writer.Write(Velocity);
                //writer.Write(something19);
                //writer.Write(something20);
                //writer.Write(something21);
                //writer.Write(something22);
                //writer.Write(something23);
                //writer.Write(something24);
                //writer.Pad(3);
                //writer.Write(something25);
                //writer.Write(something26);
                //writer.Pad(3);
                //writer.Write(something27);
                //writer.Write(something28);
            }

            public static Base Parse(Client client)
            {
                NetReader reader = client.Reader;
                var shoot = new Shoot(client);

                shoot.EntityID = reader.ReadUInt64();
                shoot.ChunkX = reader.ReadInt32();
                shoot.ChunkY = reader.ReadInt32();
                shoot.something5 = reader.ReadUInt32();
                reader.ReadBytes(4);
                shoot.Position = reader.ReadQVector3();
                shoot.something13 = reader.ReadUInt32();
                shoot.something14 = reader.ReadUInt32();
                shoot.something15 = reader.ReadUInt32();
                shoot.Velocity = reader.ReadVector3();
                shoot.something19 = reader.ReadSingle();
                shoot.something20 = reader.ReadSingle();
                shoot.something21 = reader.ReadSingle();
                shoot.something22 = reader.ReadSingle();
                shoot.something23 = reader.ReadUInt32();
                shoot.something24 = reader.ReadByte();
                reader.ReadBytes(3);
                shoot.something25 = reader.ReadUInt32();
                shoot.something26 = reader.ReadByte();
                reader.ReadBytes(3);
                shoot.something27 = reader.ReadUInt32();
                shoot.something28 = reader.ReadUInt32();

                return shoot;
            }
        }
    }
}
