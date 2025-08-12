using System.Collections.Generic;
using UnityEngine;

namespace Redsilver2.Framework.Inputs
{
    public static class InputManager
    {
        private static readonly Dictionary<KeyboardKey, string> keyboardKeysPaths = GetKeyboardKeysPaths();
        private static readonly Dictionary<GamepadKey , string>  gamepadKeysPaths  = null;


        private static Dictionary<KeyboardKey, string> GetKeyboardKeysPaths()
        {
            Dictionary<KeyboardKey, string> results = new Dictionary<KeyboardKey, string>();

            foreach (KeyboardKey key in System.Enum.GetValues(typeof(KeyboardKey)))
                results.Add(key, GetFormattedKey(key));
          
            return results;
        }

        private static Dictionary<GamepadKey, string> GetGamepadKeysPaths()
        {
            Dictionary<GamepadKey, string> results = new Dictionary<GamepadKey, string>();

            foreach (GamepadKey key in System.Enum.GetValues(typeof(GamepadKey)))
                results.Add(key, GetFormattedKey(key));

            return results;
        }

        private static string GetFormattedKey(KeyboardKey key) => $"<keyboard>/{GetFormattedKey(key.ToString())}";

        private static string GetFormattedKey(GamepadKey key)
        {
            return string.Empty;
        }

        private static string GetFormattedKey(string keyString)
        {
            if(string.IsNullOrEmpty(keyString)) return string.Empty;

            char character = char.ToLower(keyString.ToCharArray()[0]);
            keyString      = keyString.Remove(0, 1);

            return $"{character}{keyString}";
        }

        public static string GetPath(KeyboardKey key)
        {
            if (keyboardKeysPaths == null || !keyboardKeysPaths.ContainsKey(key)) return string.Empty;
            return keyboardKeysPaths[key];
        }

        public static string GetPath(GamepadKey key)
        {
            if (gamepadKeysPaths == null || !gamepadKeysPaths.ContainsKey(key)) return string.Empty;
            return gamepadKeysPaths[key];
        }
    }
}
