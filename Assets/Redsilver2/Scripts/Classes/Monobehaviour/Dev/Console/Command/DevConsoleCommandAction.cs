using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Dev
{
    public abstract partial class DevConsole : MonoBehaviour
    {
        public sealed class DevConsoleCommandAction
        {
            private readonly DevConsoleArgument[]  arguments;
            private readonly UnityAction<string[]> onExecuted;

            public DevConsoleCommandAction(UnityAction<string[]> onExecuted)
            {
                this.arguments = new DevConsoleArgument[0];
                this.onExecuted = onExecuted;
            }

            public DevConsoleCommandAction(DevConsoleArgument[] arguments, UnityAction<string[]> onExecuted) {
                this.arguments  = arguments;
                this.onExecuted = onExecuted;
            }

            public void Execute(string[] args) 
            {
                if(IsValid(args))
                   if(onExecuted != null)
                        onExecuted.Invoke(args);    
            }

            public string GetPreview()
            {
                string result = string.Empty;
                if (arguments == null) return result;

                foreach(DevConsoleArgument argument in arguments)
                    result += $"{argument.argument} ";

                return result;
            }

            public bool IsValid(string[] args)
            {
                if (args == null || arguments == null || args.Length != arguments.Length)
                    return false;

                for (int i = 0; i < arguments.Length; i++) 
                    if(!arguments[i].IsValid(args[i])) return false;

                return true;
            }

            public bool IsPreviewAllowed(string[] args)
            {
                 if (args == null || arguments == null) return false;
                 return true;
            }
        }
    }
}
