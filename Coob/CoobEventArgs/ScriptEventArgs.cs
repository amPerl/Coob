using System;
using Coob.Structures;

namespace Coob
{
    public abstract class ScriptEventArgs : EventArgs
    {
        public bool Canceled = false;
        public Entity Entity { get; private set; }
        public Client Client { get; private set; }

        protected ScriptEventArgs(Client client)
        {
            Client = client;
            Entity = client != null ? client.Entity : null;
        }
    }
}
