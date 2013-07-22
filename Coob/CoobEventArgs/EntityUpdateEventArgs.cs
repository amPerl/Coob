using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coob.Structures;

namespace Coob.CoobEventArgs
{
    class EntityUpdateEventArgs : ScriptEventArgs
    {
        public Entity Changes { get; private set; }

        public EntityUpdateEventArgs(Client client, Entity changes) : base(client)
        {
            Changes = changes;
        }
    }
}
