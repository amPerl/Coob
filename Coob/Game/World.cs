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

        public World(int seed)
        {
            Seed = seed;
            Entities = new ConcurrentDictionary<ulong, Entity>();
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
            Root.Coob.Clients.Select(cl => cl.Value)
                .Where(cl => cl.Joined)
                .ToList()
                .ForEach(cl => cl.SetTime(day, time));
        }

        public void SendServerMessage(string message)
        {
            Root.Coob.Clients.Select(cl => cl.Value)
                .Where(cl => cl.Joined)
                .ToList()
                .ForEach(
                    cl => cl.SendServerMessage(message)
                );
        }

        public void BroadcastChat(ulong id, string message)
        {
            Root.Coob.Clients.Select(cl => cl.Value)
               .Where(cl => cl.Joined)
               .ToList()
               .ForEach(
                   cl => cl.SendMessage(id, message)
               );
        }
    }
}