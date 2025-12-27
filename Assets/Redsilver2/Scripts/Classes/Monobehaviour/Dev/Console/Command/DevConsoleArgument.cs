using UnityEngine;

namespace RedSilver2.Framework.Dev
{
    public abstract partial class DevConsole : MonoBehaviour
    {
        public sealed class DevConsoleArgument
        {
            public readonly string argument;
            public readonly DevConsoleArgumentType type;
            
            public DevConsoleArgument(string argument, DevConsoleArgumentType type) {
                this.argument = argument;
                this.type     = type;
            }

            public bool IsValid(string argument)
            {
                switch (type) {
                    case DevConsoleArgumentType.Char:    return IsArgumentChar(argument);
                    case DevConsoleArgumentType.Boolean: return IsArgumentBool(argument);
                    case DevConsoleArgumentType.Int:     return IsArgumentInt(argument);
                    case DevConsoleArgumentType.UInt:    return IsArgumentUInt(argument);
                    case DevConsoleArgumentType.Float:   return IsArgumentFloat(argument);
                    case DevConsoleArgumentType.Double:  return IsArgumentDouble(argument);
                    case DevConsoleArgumentType.Long:    return IsArgumentLong(argument);
                }

                return true;
            }

            private bool IsArgumentBool(string argument)
            {
                if (!string.IsNullOrEmpty(argument)) {
                    if (bool.TryParse(argument, out bool result))
                        return result;

                    argument = argument.ToLower();

                    if      (argument == "t" || argument == "true")  return true;
                    else if (argument == "f" || argument == "false") return false;
                }

                DevConsole.LogError("Argument " + argument + " is not of type Bool.");
                return false;
            }


            private bool IsArgumentChar(string argument)
            {
                if (!string.IsNullOrEmpty(argument))
                    if (char.TryParse(argument, out char result))
                        return true;

                DevConsole.LogError("Argument (" + argument + ") is not of type Char.");
                return false;
            }

            private bool IsArgumentInt(string argument)
            {
                if (!string.IsNullOrEmpty(argument))
                    if (int.TryParse(argument, out int result))
                        return true;

                DevConsole.LogError("Argument (" +  argument + ") is not of type Int.");
                return false;
            }


            private bool IsArgumentUInt(string argument)
            {
                if (!string.IsNullOrEmpty(argument))
                    if (uint.TryParse(argument, out uint result))
                        return true;

                DevConsole.LogError("Argument (" + argument + ") is not of type UInt.");
                return false;
            }

            private bool IsArgumentFloat(string argument)
            {
                if (!string.IsNullOrEmpty(argument))
                    if (float.TryParse(argument, out  float result))
                        return true;

                DevConsole.LogError("Argument (" + argument + ") is not of type Float.");
                return false;
            }

            private bool IsArgumentDouble(string argument)
            {
                if (!string.IsNullOrEmpty(argument))
                    if (double.TryParse(argument, out double result))
                        return true;

                DevConsole.LogError("Argument (" + argument + ") is not of type Double.");
                return false;
            }

            private bool IsArgumentLong(string argument)
            {
                if (!string.IsNullOrEmpty(argument))
                    if (long.TryParse(argument, out long result))
                        return true;

                DevConsole.LogError("Argument (" + argument + ") is not of type Long.");
                return false;
            }
        }
    }
}
