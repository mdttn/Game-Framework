using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.GPUSort;


namespace RedSilver2.Framework.Dev
{
    public abstract partial class DevConsole : MonoBehaviour
    {
        public class LogCommand : DevConsoleCommand
        {
            private const string PREFIX = "Log";

            protected override void SetActions(ref List<DevConsoleCommandAction> actions)
            {
                if (actions == null) actions = new List<DevConsoleCommandAction>();

                actions.Add(new DevConsoleCommandAction(new DevConsoleArgument[]
                {
                   new DevConsoleArgument("useUnityLog [Boolean]", DevConsoleArgumentType.Boolean)
                },
                LogAction02()));

            }

            protected override void SetPrefix(ref string prefix)
            {
                prefix = PREFIX;
            }

            private UnityAction<string[]> LogAction02()
            {

                return args =>
                {
                    List<string> results = args.ToList();
                    bool canUseUnityLog = false;
                    if (args == null || args.Length == 0) return;


                    if (results.Count > 1) {
                        if      (args[0].ToLower().Equals("true") || args[1].ToLower().Equals("t")) canUseUnityLog = true;
                        else if (args[0].ToLower().Equals("false") || args[1].ToLower().Equals("f")) canUseUnityLog = false;
                        else return;

                        results.RemoveAt(0);
                        if (results.Count == 0) return;
                    }
                    else
                    {
                        return;
                    }

                   Log(results.ToArray(), canUseUnityLog);
                };
            }

            private void Log(string[] results, bool useUnityLog)
            {
                string debugMessage = string.Empty;

                foreach (string arg in results)
                    debugMessage += $"{arg} ";

                DevConsole.Log(debugMessage, useUnityLog);
            }
        }
    }
}
