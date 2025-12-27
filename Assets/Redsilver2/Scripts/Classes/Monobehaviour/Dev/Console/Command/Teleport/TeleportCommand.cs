using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Dev
{
    public abstract partial class DevConsole : MonoBehaviour
    {
        public sealed class TeleportCommand : DevConsoleCommand
        {
            private const string PREFIX = "Teleport";

            protected sealed override void SetPrefix(ref string prefix){
                prefix = PREFIX;
            }

            protected sealed override void SetActions(ref List<DevConsoleCommandAction> actions)
            {
                if (actions == null) actions = new List<DevConsoleCommandAction>();
                SetTeleportToGameObjectAction(ref actions);
            }

            private void SetTeleportToGameObjectAction(ref List<DevConsoleCommandAction> actions)
            {
                if (actions == null) return;
                actions.Add(new DevConsoleCommandAction(
                    GetTeleportToDestinationArguments01(),
                    GetTeleportToDestinationAction01()
                ));

                actions.Add(new DevConsoleCommandAction(
                    GetTeleportToXYZArguments01(),
                    GetTeleportXYZAction01()
                ));

                actions.Add(new DevConsoleCommandAction(
                    GetTeleportToXYZArguments02(),
                    GetTeleportXYZAction02()
                ));
            }

            private DevConsoleArgument[] GetBaseTeleportToArguments()
            {
                return new DevConsoleArgument[] {
                     new DevConsoleArgument("targetName [String]", DevConsoleArgumentType.String),
                };
            }

            private DevConsoleArgument[] GetTeleportToDestinationArguments01()
            {
                List<DevConsoleArgument> arguments = GetBaseTeleportToArguments().ToList();
                arguments.Add(new DevConsoleArgument("destinationName [String]", DevConsoleArgumentType.String));
                return arguments.ToArray();
            }

            private DevConsoleArgument[] GetTeleportToXYZArguments01()
            {
                List<DevConsoleArgument> arguments = GetBaseTeleportToArguments().ToList();
                arguments.Add(new DevConsoleArgument("(X,", DevConsoleArgumentType.Float));
                arguments.Add(new DevConsoleArgument("Y," , DevConsoleArgumentType.Float));
                arguments.Add(new DevConsoleArgument("Z)" , DevConsoleArgumentType.Float));
                return arguments.ToArray();
            }

            private DevConsoleArgument[] GetTeleportToXYZArguments02() {
                List<DevConsoleArgument> arguments = GetTeleportToXYZArguments01().ToList();
                arguments.Add(new DevConsoleArgument("teleportationType (Local or Global)", DevConsoleArgumentType.String));
                return arguments.ToArray();
            }

            private UnityAction<string[]> GetTeleportXYZAction01()
            {
                return args =>
                {
                    try {
                        TeleportToVector3(GameObject.Find(args[0]),
                                          new Vector3(float.Parse(args[1]), float.Parse(args[2]), float.Parse(args[3])),
                                          false);
                    }
                    catch{
                        DevConsole.LogError("One or multiple parameter(s) are  ");
                    }
                };
            }
            private UnityAction<string[]> GetTeleportXYZAction02()
            {
                return args =>
                {
                    try
                    {
                        bool isLocalPositionChange = false;

                        if      (args[4].ToLower().Equals("global")) { isLocalPositionChange = false; }
                        else if (args[4].ToLower().Equals("local"))  { isLocalPositionChange = true; }
                        else                                         { return; }

                        TeleportToVector3(GameObject.Find(args[0]),
                                          new Vector3(float.Parse(args[1]), float.Parse(args[2]), float.Parse(args[3])),
                                          isLocalPositionChange);
                    }
                    catch
                    {
                        DevConsole.LogError("One or multiple parameter(s) are  ");
                    }
                };
            }

            private UnityAction<string[]> GetTeleportToDestinationAction01()
            {
                return args =>
                {
                    try {
                        TeleportToGameObject(GameObject.Find(args[0]), GameObject.Find(args[1]));
                    }
                    catch
                    {
                        DevConsole.LogError("One or multiple parameter(s) are  ");
                    }
                };
            }




            private void TeleportToGameObject(GameObject target, GameObject destination)
            {
                if (target == null || destination == null) return;
                DevConsole.Log($"Teleported {target.name} to {destination.name}");
                TeleportToVector3(target, destination.transform.position, false);
            }

            private void TeleportToVector3(GameObject target, Vector3 vector, bool useLocalPosition)
            {
                if(target == null) return;
                if (useLocalPosition) target.transform.localPosition = vector;
                else                  target.transform.position      = vector;
            }
        }
    }
}
