namespace Coob.CoobEventArgs
{
    public class InitializeEventArgs : ScriptEventArgs
    {
        public int WorldSeed;

        public InitializeEventArgs() : base(null)
        {
            WorldSeed = 1;
        }
    }
}
