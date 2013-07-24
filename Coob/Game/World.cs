using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coob.CoobEventArgs;
using Coob.Structures;

namespace Coob.Game
{
    public class World
    {
        public int Seed { get; set; }
        public ConcurrentDictionary<ulong, Entity> Entities { get; private set; }
        public Coob Coob { get; private set; }

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

        public void SendEntityUpdates()
        {

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