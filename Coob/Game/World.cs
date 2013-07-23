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

            Root.ScriptManager.CallEvent("OnWorldUpdate", new WorldUpdateEventArgs(dt));
        }

        public void SendEntityUpdates()
        {

        }
    }
}