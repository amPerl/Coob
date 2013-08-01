using System.IO;

namespace Coob.Packets
{
    /* Structure info mostly stolen from mat^2 ;) */

    public partial class Packet
    {
        public class Shoot : Base
        {
            public ulong EntityId;

            public int ChunkX;
            public int ChunkY;

            private uint something5;

            public QVector3 Position;

            private uint something13;
            private uint something14;
            private uint something15;

            public Vector3 Velocity;

            private float something19; // rand() something, probably damage multiplier.
            private float something20;
            private float something21;
            private float something22; // used stamina? amount of stun?
            private uint something23;
            private byte something24;
            private uint something25;
            private byte something26;
            private uint something27;
            private uint something28;

            public Shoot(Client client)
                : base(client)
            {

            }

            public override bool CallScript()
            {
                return true;
            }

            public override void Write(BinaryWriter writer)
            {
 
                writer.Write(EntityId);
                writer.Write(ChunkX);
                writer.Write(ChunkY);
                writer.Write(something5);
                writer.Pad(4);
                writer.Write(Position);
                writer.Write(something13);
                writer.Write(something14);
                writer.Write(something15);
                writer.Write(Velocity);
                writer.Write(something19);
                writer.Write(something20);
                writer.Write(something21);
                writer.Write(something22);
                writer.Write(something23);
                writer.Write(something24);
                writer.Pad(3);
                writer.Write(something25);
                writer.Write(something26);
                writer.Pad(3);
                writer.Write(something27);
                writer.Write(something28);
            }

            public override void Process()
            {

            }

            public static Base Parse(Client client, Coob coob)
            {
                NetReader reader = client.Reader;
                var shoot = new Shoot(client);

                shoot.EntityId = reader.ReadUInt64();
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
                coob.World.ShootPackets.Add(shoot);
                return shoot;
            }
        }
    }
}
