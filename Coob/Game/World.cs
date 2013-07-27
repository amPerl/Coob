using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        public List<Packets.Packet.Hit> HitPackets = new List<Packets.Packet.Hit>();
        public List<Packets.Packet.Shoot> ShootPackets = new List<Packets.Packet.Shoot>();

        public World(int seed, Coob coob)
        {
            Seed = seed;
            Entities = new ConcurrentDictionary<ulong, Entity>();
            Coob = coob;
        }

        public void Update(float dt)
        {
            // Update stuff

            var eventArgs = Root.ScriptManager.CallEvent("OnWorldUpdate", new WorldUpdateEventArgs(dt));
        }

        public void SendServerUpdate()
        {
            // Start of our Server-wide Updates

            byte[] compressed;

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {

                uint items_1_length = 0;
                bw.Write(items_1_length);
                bw.Write(HitPackets.Count);
                foreach (Packets.Packet.Hit hit in HitPackets)
                {
                    hit.write(bw);
                }
                uint items_3_length = 0;
                bw.Write(items_3_length);
                uint sound_actions_length = 0;
                bw.Write((uint)sound_actions_length);

                bw.Write(ShootPackets.Count);
                foreach (Packets.Packet.Shoot shoot in ShootPackets)
                {
                    shoot.write(bw);
                }

                uint items_6_length = 0;
                bw.Write(items_6_length);
                uint chunk_items_length = 0;
                bw.Write(chunk_items_length);
                uint items_8_length = 0;
                bw.Write(items_8_length);
                uint pickups_length = 0;
                bw.Write(pickups_length);
                uint kill_actions = 0;
                bw.Write(kill_actions);
                uint damage_actions = 0;
                bw.Write(damage_actions);
                uint items_12_length = 0;
                bw.Write(items_12_length);
                uint missions_length = 0;
                bw.Write(missions_length);


                byte[] uncompressed = ms.ToArray();
                compressed = ZlibHelper.CompressBuffer(uncompressed);
            }


            // Send all the compressed data to every client
            foreach (Client client in Coob.GetClients())
            {
                if (!client.Joined)
                    continue;
                    client.Writer.Write(SCPacketIDs.ServerUpdate);
                    client.Writer.Write((uint)compressed.Length);
                    client.Writer.Write(compressed);
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
            foreach (var client in Coob.GetClients())
            {
                if (client.Joined)
                {
                    client.SetTime(day, time);
                }
            }
        }

        public void SendServerMessage(string message)
        {
            foreach(var client in Coob.GetClients())
            {
                if (client.Joined)
                {
                    client.SendServerMessage(message);
                }
            }
        }

        public void BroadcastChat(ulong id, string message)
        {
            foreach (var client in Coob.GetClients())
            {
                if (client.Joined)
                {
                    client.SendMessage(id, message);
                }
            }
        }
    }

 
}