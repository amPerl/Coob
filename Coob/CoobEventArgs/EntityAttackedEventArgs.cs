using Coob.Structures;

namespace Coob.CoobEventArgs
{
    public class EntityAttackedEventArgs : ScriptEventArgs
    {
        public Entity Attacker { get; private set; }
        public Entity Target { get; private set; }
        public bool Killed { get; private set; }

        public EntityAttackedEventArgs(Entity attacker, Entity target)
            : base(null)
        {
            Attacker = attacker;
            Target = target;
            Killed = Target.Hp <= 0;
        }
    }
}
