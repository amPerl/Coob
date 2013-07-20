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
            Engine.AddPermission(new FileIOPermission(PermissionState.Unrestricted));

            Engine.SetParameter("coob", Root.Coob);

            Engine.SetFunction("LogInfo", (Action<string>)Log.Info);
            Engine.SetFunction("LogWarning", (Action<string>)Log.Warning);
            Engine.SetFunction("LogError", (Action<string>)Log.Error);

            try
            {
                Engine.Run(File.ReadAllText(source));
            }
            catch (JintException ex)
            {
                Log.Error(ex.InnerException != null ? (ex.Message + ": " + ex.InnerException.Message) : ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
