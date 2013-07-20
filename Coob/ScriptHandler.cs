using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jint;
using Jint.Native;
using System.IO;
using System.Security.Permissions;

namespace Coob
{
    class ScriptHandler
    {
        public JintEngine Engine;

        public ScriptHandler(string source)
        {

            Engine = new JintEngine();
            Engine.AddPermission(new UIPermission(PermissionState.Unrestricted));

            Engine.SetParameter("coob", Root.Coob);

            Engine.SetFunction("LogInfo", (Action<string>)Log.WriteInfo);
            Engine.SetFunction("LogWarning", (Action<string>)Log.WriteWarning);
            Engine.SetFunction("LogError", (Action<string>)Log.WriteError);

            try
            {
                Engine.Run(File.ReadAllText(source));
            }
            catch (JintException ex) { Log.WriteError(ex.Message); }
        }
    }
}
