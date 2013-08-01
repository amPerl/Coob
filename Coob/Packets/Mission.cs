using System.IO;

namespace Coob.Packets
{
    public partial class Packet
    {
        public class Mission : Base
        {
            public uint SectorX;
            public uint SectorY;
            public uint Something1;
            public uint Something2;
            public uint Something3;
            public uint Something4;
            public uint Something5;
            public uint MonsterId;
            public uint QuestLevel;
            public byte Something8;
            public byte Something9;
            public float Something10;
            public float Something11;
            public uint ChunkX;
            public uint ChunkY;

            public Mission(Client client) : base(client) { }

            public static Base Parse(Client client, Coob coob)
            {
                var mission = new Mission(client);

                mission.SectorX = client.Reader.ReadUInt32();
                mission.SectorY = client.Reader.ReadUInt32();
                mission.Something1 = client.Reader.ReadUInt32();
                mission.Something2 = client.Reader.ReadUInt32();
                mission.Something3 = client.Reader.ReadUInt32();
                mission.Something4 = client.Reader.ReadUInt32();
                mission.Something5 = client.Reader.ReadUInt32();
                mission.MonsterId = client.Reader.ReadUInt32();
                mission.QuestLevel = client.Reader.ReadUInt32();
                mission.Something8 = client.Reader.ReadByte();
                mission.Something9 = client.Reader.ReadByte();
                client.Reader.ReadBytes(2);
                mission.Something10 = client.Reader.ReadSingle();
                mission.Something11 = client.Reader.ReadSingle();
                mission.ChunkX = client.Reader.ReadUInt32();
                mission.ChunkY = client.Reader.ReadUInt32();

                return mission;
            }

            public override void Write(BinaryWriter bw)
            {
                bw.Write(SectorX);
                bw.Write(SectorY);
                bw.Write(Something1);
                bw.Write(Something2);
                bw.Write(Something3);
                bw.Write(Something4);
                bw.Write(Something5);
                bw.Write(MonsterId);
                bw.Write(QuestLevel);
                bw.Write(Something8);
                bw.Write(Something9);
                bw.Pad(2);
                bw.Write(Something10);
                bw.Write(Something11);
                bw.Write(ChunkX);
                bw.Write(ChunkY);
            }

            public override bool CallScript()
            {
                return true;
            }

            public override void Process()
            {

            }
        }

    }
}
