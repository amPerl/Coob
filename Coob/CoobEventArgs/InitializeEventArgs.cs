namespace Coob.CoobEventArgs
{
    public class InitializeEventArgs : ScriptEventArgs
    {
        public int Port;
        public int WorldSeed;

        public InitializeEventArgs() : base(null)
        {
            //Default Values. Can be changed by script
            Port = 12345; 
            WorldSeed = 26879;
        }
    }
}
