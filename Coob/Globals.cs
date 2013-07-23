using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coob
{
    public static class Globals
    {
        public static readonly int MaxChatMessageLength = 46; // Currently the max characters that fits before text is cutoff on client
        public static readonly int WorldTickPerSecond = (int)(1000f / 60f);
        public static readonly int EntityUpdatesPerSecond = (int)(1000f / 30f);
    }
}