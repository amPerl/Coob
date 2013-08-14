namespace Coob
{
    public static class Globals
    {
        public static readonly int ServerVersion = 3;

        // Currently the max characters that fits before text is cutoff on client
        public static readonly int MaxChatMessageLength = 46;
        public static readonly int WorldTickPerSecond = (int)(1000f / 60f);
        public static readonly int EntityUpdatesPerSecond = (int)(1000f / 30f);
        public static readonly int MaxConcurrentPlayers = 4;

        public static readonly int DefaultPort = 12345;
        public static readonly uint DefaultMaxClients = 1024;
        public static readonly int DefaultWorldSeed = 26879;

        // If true, events will be printed to the console as they're called.
        public static bool PrintEvents = true;
        // If true, events such as world update will be printed aswell.
        public static bool PrintEventUpdates = false;
    }
}
