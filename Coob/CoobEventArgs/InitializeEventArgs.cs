namespace Coob.CoobEventArgs
{
    public class InitializeEventArgs : ScriptEventArgs
    {
        public int Port;
        public int WorldSeed;

        public InitializeEventArgs()
            : base(null)
        {
            // Default Values, can be changed by script
            Port = Globals.DefaultPort;
            WorldSeed = Globals.DefaultWorldSeed;
        }
    }
}
