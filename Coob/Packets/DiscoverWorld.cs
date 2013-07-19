using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coob.Packets
{
    partial class Packet
    {

        public class UpdateChunk : Base
        {
            public int ChunkX, ChunkY;

            public UpdateChunk(int chunkX, int chunkY, Client client)
                : base(client)
            {
                ChunkX = chunkX;
                ChunkY = chunkY;
            }

            public static Base Parse(Client client)
            {
                return new UpdateChunk(
                    client.Reader.ReadInt32(),
                    client.Reader.ReadInt32(),
                    client);
            }

            public override bool CallScript()
            {
                return true;
            }

            public override void Process()
            {
            }
        }

        public class UpdateSector : Base
        {
            public int SectorX, SectorY;

            public UpdateSector(int sectorX, int sectorY, Client client)
                : base(client)
            {
                SectorX = sectorX;
                SectorY = sectorY;
            }

            public static Base Parse(Client client)
            {
                return new UpdateSector(
                    client.Reader.ReadInt32(),
                    client.Reader.ReadInt32(),
                    client);
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
