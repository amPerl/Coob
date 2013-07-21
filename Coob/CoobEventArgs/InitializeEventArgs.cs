namespace Coob.CoobEventArgs
{
    public class InitializeEventArgs : ScriptEventArgs
    {
        public int WorldSeed;

        public InitializeEventArgs(int defaultWorldSeed)
        {
            WorldSeed = defaultWorldSeed;
        }
    }
}
