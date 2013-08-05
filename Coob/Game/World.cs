using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Coob.CoobEventArgs;
using Coob.Structures;
using Coob.Packets;
using System.IO;

namespace Coob.Game
{
    public class World
    {
        public int Seed { get; set; }
        public ConcurrentDictionary<ulong, Entity> Entities { get; private set; }
        public Coob Coob { get; private set; }

        public List<Packet.Hit> HitPackets = new List<Packet.Hit>();
        public List<Packet.Shoot> ShootPackets = new List<Packet.Shoot>();

        public World(int seed, Coob coob)
        {
            Seed = seed;
            Entities = new ConcurrentDictionary<ulong, Entity>();
            Coob = coob;
        }

        public void Update(float dt)
        {
            // Update stuff

            var eventArgs = Program.ScriptManager.CallEvent("OnWorldUpdate", new WorldUpdateEventArgs(dt));
        }

        public void SendServerUpdate()
        {
            byte[] compressed;

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                const uint Items1Length = 0;
                bw.Write(Items1Length);

                bw.Write(HitPackets.Count);
                foreach (Packet.Hit hit in HitPackets)
                    hit.Write(bw);

                const uint Items3Length = 0;
                bw.Write(Items3Length);

                const uint SoundActionsLength = 0;
                bw.Write(SoundActionsLength);

                bw.Write(ShootPackets.Count);
                foreach (Packet.Shoot shoot in ShootPackets)
                    shoot.Write(bw);

                const uint Items6Length = 0;
                bw.Write(Items6Length);

                const uint ChunkItemsLength = 0;
                bw.Write(ChunkItemsLength);

                const uint Items8Length = 0;
                bw.Write(Items8Length);

                const uint PickupsLength = 0;
                bw.Write(PickupsLength);

                const uint KillActions = 0;
                bw.Write(KillActions);

                const uint DamageActions = 0;
                bw.Write(DamageActions);

                const uint Items12Length = 0;
                bw.Write(Items12Length);

                const uint MissionsLength = 0;
                bw.Write(MissionsLength);

                byte[] uncompressed = ms.ToArray();
                compressed = ZlibHelper.CompressBuffer(uncompressed);
            }

            // Send all the compressed data to every client
            foreach (Client client in Coob.GetClients().Where(client => client.Joined))
            {
                try
                {
                    client.Writer.Write(SCPacketIDs.ServerUpdate);
                    client.Writer.Write((uint)compressed.Length);
                    client.Writer.Write(compressed);
                }
                catch (IOException)
                {

                }
            }

            // Clear data
            HitPackets.Clear();
            ShootPackets.Clear();
        }

        /// <summary>Sets the current day and time for the client.</summary>
        /// <param name="day">The current day (not sure what use this has).</param>
        /// <param name="time">The elapsed hours in 0-24 range.</param>
        public void SetTime(uint day, float time)
        {
            foreach (var client in Coob.GetClients().Where(client => client.Joined))
                client.SetTime(day, time);
        }

        public void SendServerMessage(string message)
        {
            foreach (var client in Coob.GetClients().Where(client => client.Joined))
                client.SendServerMessage(message);
        }

        public void BroadcastChat(ulong id, string message)
        {
            foreach (var client in Coob.GetClients().Where(client => client.Joined))
                client.SendMessage(id, message);
        }
    }
}
