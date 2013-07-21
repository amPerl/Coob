using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coob.Structures;

namespace Coob.CoobEventArgs
{
    class EntityUpdateEventArgs : ScriptEventArgs
    {
        public Entity Entity { get; private set; }
        public Entity Changes { get; private set; }

        public EntityUpdateEventArgs(Client client, Entity entity, Entity changes) : base(client)
        {
            Entity = entity;
            Changes = changes;
        }
    }
}
