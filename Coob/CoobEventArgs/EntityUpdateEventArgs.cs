using Coob.Structures;

namespace Coob.CoobEventArgs
{
    public class EntityUpdateEventArgs : ScriptEventArgs
    {
        public Entity Changes { get; private set; }

        public EntityUpdateEventArgs(Client client, Entity changes)
            : base(client)
        {
            Changes = changes;
        }
    }
}
