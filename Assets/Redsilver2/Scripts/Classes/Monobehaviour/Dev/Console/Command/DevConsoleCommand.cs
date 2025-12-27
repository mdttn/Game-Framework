using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Dev
{
    public abstract partial class DevConsole : MonoBehaviour
    {
        public abstract class DevConsoleCommand
        {
            public  readonly string                          prefix; 
            private readonly List<DevConsoleCommandAction>   actions;

            public DevConsoleCommand() {
                actions = new List<DevConsoleCommandAction>();
                SetPrefix (ref prefix);
                SetActions(ref actions);

                DevConsole.commands.Add(this);
            }

            protected abstract void SetPrefix(ref string prefix);
            protected abstract void SetActions(ref List<DevConsoleCommandAction> actions);

            public async Awaitable<bool> IsValid(string input)
            {
                string[] args, prefixs;
                if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(prefix)) return false;

                await Awaitable.BackgroundThreadAsync();

                args    = input.ToLower().Split(' ');
                prefixs = prefix.ToLower().Split(' ');
               
                if (args == null || prefixs == null || args.Length < prefixs.Length)
                {
                    await Awaitable.MainThreadAsync();
                    return false;
                }

                for(int i = 0; i < prefixs.Length; i++)
                    if(!prefixs[i].Contains(args[i])) return false;

                await Awaitable.MainThreadAsync();
                return true;
            }

            public async Awaitable<string[]> GetActionPreviews() {

                List<string> results = new List<string>();
                if (actions == null || actions.Count == 0) return results.ToArray();

                await Awaitable.BackgroundThreadAsync();

                foreach (DevConsoleCommandAction action in actions)
                    results.Add($"{prefix} {action.GetPreview()}");

                await Awaitable.MainThreadAsync();
                return results.ToArray();
            }

            public async Awaitable<string[]> GetActionPreviews(string input)
            {
                List<string> results = new List<string>();
                if (actions == null || actions.Count == 0 || string.IsNullOrEmpty(input) || !await IsValid(input)) return results.ToArray();

                await Awaitable.BackgroundThreadAsync();

                foreach (DevConsoleCommandAction action in actions)
                      results.Add($"{prefix} {action.GetPreview()}");

                await Awaitable.MainThreadAsync();
                return results.ToArray();
            }


            private async Awaitable<string[]> GetFormattedArguments(string input)
            {
                string[] prefixs;
                List<string> results = new List<string>();

                if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(prefix)) return results.ToArray();

                await Awaitable.BackgroundThreadAsync();

                results = input.Split(' ').ToList();
                prefixs = prefix.Split(' ');

                if (results == null || prefixs == null || results.Count < prefixs.Length) {
                    await Awaitable.MainThreadAsync();
                    return results.ToArray();
                }

                List<int> registeredIndexes = new List<int>();

                for (int i = 0; i < prefixs.Length; i++){
                    if (prefixs[i].ToLower().Equals(results[i].ToLower()))
                        registeredIndexes.Add(i);
                }

                if (registeredIndexes.Count != prefixs.Length){
                    await Awaitable.MainThreadAsync();
                    return results.ToArray();
                }

                foreach (int i in registeredIndexes) results.RemoveAt(i);

                await Awaitable.MainThreadAsync();
                return results.ToArray();
            }


            public async void Execute(string input)
            {
                if (await IsValid(input)) {
                    string[] args = await GetFormattedArguments(input);
                    var results   = actions.Where(x => x.IsValid(args));
                    if (results.Count() > 0) results.First().Execute(args);
                }
            }
        }
    }
}
