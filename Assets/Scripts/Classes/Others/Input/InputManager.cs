using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace RedSilver2.Framework.Inputs
{
    public static class InputManager
    {
        public const string KEYBOARD_ICONS_PATH = "Sprites/Inputs/Keyboard/";
        public const string GAMEPAD_ICONS_PATH  = "Sprites/Inputs/Gamepad/";
        public const string XR_ICONS_PATH       = "Sprites/Inputs/XR/";

        private static readonly Dictionary<KeyboardKey, (Sprite Icon, string Path)>  keyboardKeysDatas = GetKeyboardKeysDatas();
        private static readonly Dictionary<GamepadKey , (Sprite Icon, string Path)>  gamepadKeysPaths  = GetGamepadKeysDatas();

        #region Input Datas

        #region Initialization
        private static Dictionary<KeyboardKey, (Sprite Icon, string Path)> GetKeyboardKeysDatas()
        {
            Dictionary<KeyboardKey, (Sprite Icon, string Path)> results = new Dictionary<KeyboardKey, (Sprite, string)>();

            foreach (KeyboardKey key in System.Enum.GetValues(typeof(KeyboardKey)))
                results.Add(key, (GetKeyIconFromResources(key.ToString(), KEYBOARD_ICONS_PATH), GetFormattedKey(key)));

            return results;
        }
        private static Dictionary<GamepadKey, (Sprite Icon, string Path)> GetGamepadKeysDatas()
        {
            Dictionary<GamepadKey, (Sprite, string)> results = new Dictionary<GamepadKey, (Sprite, string)>();

            foreach (GamepadKey key in Enum.GetValues(typeof(GamepadKey)))
                results.Add(key, (GetKeyIconFromResources(key.ToString(), GAMEPAD_ICONS_PATH), GetFormattedKey(key)));

            return results;
        }
        #endregion

        #region Key Formatting

        private static string GetFormattedKey(string keyString)
        {
            if (string.IsNullOrEmpty(keyString)) return string.Empty;

            char character = char.ToLower(keyString.ToCharArray()[0]);
            keyString = keyString.Remove(0, 1);

            return $"{character}{keyString}";
        }

        #region Keyboard 
        private static string GetFormattedKey(KeyboardKey key) => $"<Keyboard>/{GetFormattedKey(key.ToString())}";
        #endregion

        #region Gamepad
        private static string GetFormattedKey(GamepadKey key)
        {
            if      (TryFormateGamepadStickKey(key, out string currentStick)) { return currentStick; }
            else if (TryFormatGamepadDpadKey  (key, out string currentDpad))  { return currentDpad; }
            return GetFormattedKey($"<Gamepad>/{GetFormattedKey(key.ToString())}");
        }
        private static bool TryFormateGamepadStickKey(GamepadKey key, out string result)
        {
            string currentStick, leftStick = GamepadKey.LeftStick.ToString(), rightStick = GamepadKey.RightStick.ToString(), currentKey = key.ToString();
           
            if(key == GamepadKey.LeftStick || key == GamepadKey.RightStick || key == GamepadKey.LeftStickPress || key == GamepadKey.RightStickPress)
            { result = string.Empty; return false; }

            currentStick = currentKey.Contains(leftStick) ? leftStick : currentKey.Contains(rightStick) ? rightStick : string.Empty;
            if (currentStick == string.Empty){ result = string.Empty; return false; }

            result = $"<Gamepad>/{GetFormattedKey(currentStick)}/{char.ToLower(currentKey[currentKey.Length - 1])}";
            return true;
        }
        private static bool TryFormatGamepadDpadKey(GamepadKey key, out string result)
        {
            string currentKey = key.ToString();
            if (!currentKey.Contains("Dpad")) { result = string.Empty; return false; }

            result = $"<Gamepad>/dpad/{GetFormattedKey(currentKey.Split("Dpad")[1])}";
            return true;
        }
        #endregion


        #endregion

        #region Get

        #region Keyboard
        public static (Sprite Icon, string Path)[] GetKeyboardPaths() => GetGamepadDatas(null);
        public static (Sprite Icon, string Path)[] GetKeyboardDatas(KeyboardKey[] excludedKeys)
        {
            List<(Sprite Icon, string Path)> results = new List<(Sprite Icon, string Path)>();
            if (excludedKeys != null) excludedKeys = excludedKeys.Distinct().ToArray();

            foreach (KeyboardKey key in Enum.GetValues(typeof(KeyboardKey)))
            {
                if (excludedKeys != null)
                    if (excludedKeys.Contains(key)) continue;

                (Sprite Icon, string Path) result = keyboardKeysDatas[key];
                results.Add((result.Icon, result.Path));
            }

            return results.ToArray();
        }
        #endregion


        #region Gamepad
        public static (Sprite Icon, string Path)[] GetGamepadDatas() => GetGamepadDatas(null);
        public static (Sprite Icon, string Path)[] GetGamepadDatas(GamepadKey[] excludedKeys)
        {
            List<(Sprite Icon, string Path)> results = new List<(Sprite Icon, string Path)>();
            if (excludedKeys != null) excludedKeys = excludedKeys.Distinct().ToArray();

            foreach (GamepadKey key in Enum.GetValues(typeof(GamepadKey)))
            {
                if (excludedKeys != null)
                    if (excludedKeys.Contains(key)) continue;

                (Sprite Icon, string Path) result = gamepadKeysPaths[key];
                results.Add((result.Icon, result.Path));
            }

            return results.ToArray();
        }
        #endregion

        #endregion

        #endregion

        #region Input Path

        #region Keyboard 
        public static string GetPath(KeyboardKey key)
        {
            if (keyboardKeysDatas == null || !keyboardKeysDatas.ContainsKey(key)) return string.Empty;
            return keyboardKeysDatas[key].Path;
        }
        #endregion

        #region Gamepad
        public static string GetPath(GamepadKey key)
        {
            if (gamepadKeysPaths == null || !gamepadKeysPaths.ContainsKey(key)) return string.Empty;
            return gamepadKeysPaths[key].Path;
        }

        public static string[] GetGamepadPaths() => GetGamepadPaths(null);
        public static string[] GetGamepadPaths(GamepadKey[] excludedKeys)
        {
            List<string> results = new List<string>();

            foreach (var result in GetGamepadDatas(excludedKeys))  results.Add(result.Path);
            return results.ToArray();
        }
        #endregion


        #endregion

        #region Input Icon
        private static Sprite GetKeyIconFromResources(string fileName, string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            return Resources.Load<Sprite>($"{path}{fileName}");
        }

        #region Keyboard 

        public static Sprite GetKeyIcon(KeyboardKey key)
        {
            if (keyboardKeysDatas == null || !keyboardKeysDatas.ContainsKey(key)) return null;
            return keyboardKeysDatas[key].Icon;
        }

        #endregion

        #region Gamepad
        
        public static Sprite GetKeyIcon(GamepadKey key)
        {
            if (gamepadKeysPaths == null || !gamepadKeysPaths.ContainsKey(key)) return null;
            return gamepadKeysPaths[key].Icon;
        }

        #endregion


        #endregion

        public static bool GetKey(KeyboardKey key)
        {
            var button = InputSystem.FindControl(GetPath(key)) as ButtonControl;

            if (button != null) return button.isPressed;
            return false;
        }
        public static bool GetKey(GamepadKey key)
        {
            var button = InputSystem.FindControl(GetPath(key)) as ButtonControl;

            if (button != null) return button.isPressed;
            return false;
        }

        public static bool GetKeyDown(KeyboardKey key)
        {
            var button = InputSystem.FindControl(GetPath(key)) as ButtonControl;

            if (button != null) return button.wasPressedThisFrame;
            return false;
        }
        public static bool GetKeyDown(GamepadKey key)
        {
            var button = InputSystem.FindControl(GetPath(key)) as ButtonControl;

            if (button != null) return button.wasPressedThisFrame;
            return false;
        }

        public static bool GetKeyUp(KeyboardKey key)
        {
            var button = InputSystem.FindControl(GetPath(key)) as ButtonControl;

            if (button != null) return button.wasReleasedThisFrame;
            return false;
        }
        public static bool GetKeyUp(GamepadKey key)
        {
            var button = InputSystem.FindControl(GetPath(key)) as ButtonControl;

            if (button != null) return button.wasReleasedThisFrame;
            return false;
        }
    }
}
