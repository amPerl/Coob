using System;
using Coob.Structures;

namespace Coob.CoobEventArgs
{
    public abstract class ScriptEventArgs : EventArgs
    {
        public Entity Entity { get; private set; }
        public Client Client { get; private set; }
        public bool Canceled;

        protected ScriptEventArgs(Client client)
        {
            Client = client;
            Entity = client != null ? client.Entity : null;
            Canceled = false;
        }
    }
}
