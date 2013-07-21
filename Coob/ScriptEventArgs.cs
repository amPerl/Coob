using System;

namespace Coob
{
    public abstract class ScriptEventArgs : EventArgs
    {
        public bool Canceled = false;
    }
}
