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
        public static bool AnyKeyboardKey     => keyboardKeysDatas.Keys.Where(x => GetKey(x)).Count() > 0;
        public static bool AnyGamepadKey      => gamepadKeysDatas.Keys.Where(x => GetKey(x)).Count() > 0;

        public static bool AnyKeyboardKeyDown => keyboardKeysDatas.Keys.Where(x => GetKeyDown(x)).Count() > 0;
        public static bool AnyGamepadKeyDown  => gamepadKeysDatas.Keys.Where(x => GetKeyDown(x)).Count() > 0;

        public static bool AnyKeyboardKeyUp   => keyboardKeysDatas.Keys.Where(x => GetKeyUp(x)).Count() > 0;
        public static bool AnyGamepadKeyUp    => gamepadKeysDatas.Keys.Where(x => GetKeyUp(x)).Count() > 0;

        public static bool AnyKey     => AnyKeyboardKey     || AnyGamepadKey;
        public static bool AnyKeyDown => AnyKeyboardKeyDown || AnyGamepadKeyDown;
        public static bool AnyKeyUp   => AnyKeyboardKeyUp   || AnyGamepadKeyUp;

        public const string KEYBOARD_ICONS_PATH  = "Sprites/Inputs/Keyboard/";
        public const string GAMEPAD_ICONS_PATH   = "Sprites/Inputs/Gamepad/";
        public const string XR_ICONS_PATH        = "Sprites/Inputs/XR/";

        public const string KEYBOARD_ROOT_PATH      = "<Keyboard>/";
        public const string GAMEPAD_ROOT_PATH       = "<Gamepad>/";
        public const string XR_CONTROLLER_ROOT_PATH = "<XRController>/";

        private static readonly Dictionary<string      , InputHandler>              inputHandlerInstances = new Dictionary<string, InputHandler>();
        private static readonly Dictionary<KeyboardKey , InputButtonControl>        keyboardKeysDatas     = GetKeyboardKeysDatas();
        private static readonly Dictionary<GamepadButton  , InputButtonControl>     gamepadKeysDatas      = GetGamepadKeysDatas();
        private static readonly Dictionary<GamepadStick, InputGamepadStickControl>  gamepadStickDatas     = GetGamepadStickDatas();


        #region Input Datas

        #region Initialization
        private static Dictionary<KeyboardKey, InputButtonControl> GetKeyboardKeysDatas()
        {
            Dictionary<KeyboardKey, InputButtonControl> results = new Dictionary<KeyboardKey, InputButtonControl>();

            foreach (KeyboardKey key in Enum.GetValues(typeof(KeyboardKey)))
                results.Add(key, new InputButtonControl(GetFormattedKey(key), GetKeyIconFromResources(key.ToString(), KEYBOARD_ICONS_PATH)));

            return results;
        }
        private static Dictionary<GamepadButton, InputButtonControl> GetGamepadKeysDatas()
        {
            Dictionary<GamepadButton, InputButtonControl> results = new Dictionary<GamepadButton, InputButtonControl>();

            foreach (GamepadButton key in Enum.GetValues(typeof(GamepadButton)))
            {
                string keyString = key.ToString();
                results.Add(key, new InputButtonControl(GetFormattedKey(key), GetKeyIconFromResources(key.ToString(), GAMEPAD_ICONS_PATH)));
            }


            return results;
        }

        private static Dictionary<GamepadStick, InputGamepadStickControl> GetGamepadStickDatas()
        {
            Dictionary<GamepadStick, InputGamepadStickControl> results = new Dictionary<GamepadStick, InputGamepadStickControl>();

            foreach (GamepadStick key in Enum.GetValues(typeof(GamepadStick))) {
                Debug.Log(key);
                results.Add(key, new InputGamepadStickControl(key, GetKeyIconFromResources(key.ToString(), GAMEPAD_ICONS_PATH)));
            }

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
        private static string GetFormattedKey(KeyboardKey key) => $"{KEYBOARD_ROOT_PATH}{GetFormattedKey(key.ToString())}";
        #endregion

        #region Gamepad
        private static string GetFormattedKey(GamepadButton key)
        {
            if (TryFormatGamepadDpadKey  (key, out string currentDpad))  { return currentDpad; }
            return GetFormattedKey($"{GAMEPAD_ROOT_PATH}{GetFormattedKey(key.ToString())}");
        }

        private static string GetGamepadStickAxisPath(bool isLeftStick, bool isAxisX)
        {
            string axis = isAxisX ? "x" : "y";
            return $"{GetPath(isLeftStick ? GamepadStick.LeftStick : GamepadStick.RightStick)}/" + axis;
        }
        private static bool TryFormatGamepadDpadKey(GamepadButton key, out string result)
        {
            string currentKey = key.ToString();
            if (!currentKey.Contains("Dpad")) { result = string.Empty; return false; }

            result = $"{GAMEPAD_ROOT_PATH}dpad/{GetFormattedKey(currentKey.Split("Dpad")[1])}";
            return true;
        }
        #endregion


        #endregion

        #region Get

        #region Keyboard
        public static InputButtonControl[] GetKeyboardPaths() => GetGamepadDatas(null);
        public static InputButtonControl[] GetKeyboardDatas(KeyboardKey[] excludedKeys)
        {
            List<InputButtonControl> results = new List<InputButtonControl>();
            if (excludedKeys != null) excludedKeys = excludedKeys.Distinct().ToArray();

            foreach (KeyboardKey key in Enum.GetValues(typeof(KeyboardKey)))
            {
                if (excludedKeys != null)
                    if (excludedKeys.Contains(key)) continue;
               
                results.Add(keyboardKeysDatas[key]);
            }

            return results.ToArray();
        }
        #endregion


        #region Gamepad
        public static InputButtonControl[] GetGamepadDatas() => GetGamepadDatas(null);
        public static InputButtonControl[] GetGamepadDatas(GamepadButton[] excludedKeys)
        {
            List<InputButtonControl> results = new List<InputButtonControl>();
            if (excludedKeys != null) excludedKeys = excludedKeys.Distinct().ToArray();

            foreach (GamepadButton key in Enum.GetValues(typeof(GamepadButton)))
            {
                if (excludedKeys != null)
                    if (excludedKeys.Contains(key)) continue;
                results.Add(gamepadKeysDatas[key]);
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
        public static string GetPath(GamepadButton key)
        {
            if (gamepadKeysDatas == null || !gamepadKeysDatas.ContainsKey(key)) return string.Empty;
            return gamepadKeysDatas[key].Path;
        }

        public static string GetPath(GamepadStick key) => $"{GAMEPAD_ROOT_PATH}{GetFormattedKey(key.ToString())}";

        public static string[] GetGamepadPaths() => GetGamepadPaths(null);
        public static string[] GetGamepadPaths(GamepadButton[] excludedKeys)
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

        public static void OverrideKeyIcon(KeyboardKey key, Sprite icon)
        {
            if (keyboardKeysDatas == null || !keyboardKeysDatas.ContainsKey(key)) return;
            keyboardKeysDatas[key].OverrideIcon(icon);
        }

        #endregion

        #region Gamepad
        
        public static Sprite GetKeyIcon(GamepadButton key)
        {
            if (gamepadKeysDatas == null || !gamepadKeysDatas.ContainsKey(key)) return null;
            return gamepadKeysDatas[key].Icon;
        }

        public static void OverrideKeyIcon(GamepadButton key, Sprite icon)
        {
            if (keyboardKeysDatas == null || !gamepadKeysDatas.ContainsKey(key)) return;
            gamepadKeysDatas[key].OverrideIcon(icon);
        }

        public static Sprite GetKeyIcon(GamepadStick key)
        {
            if (gamepadStickDatas == null || !gamepadStickDatas.ContainsKey(key)) return null;
            return gamepadStickDatas[key].Icon;
        }

        public static void OverrideKeyIcon(GamepadStick key, Sprite icon)
        {
            if (gamepadStickDatas == null || !gamepadStickDatas.ContainsKey(key)) return;
            gamepadStickDatas[key].OverrideIcon(icon);
        }

        #endregion


        #endregion

        public static void AddInputHandler(string name, InputHandler handler)
        {
            if(!string.IsNullOrEmpty(name) && handler != null && inputHandlerInstances != null)
            {
                name = name.ToLower();

                if (!inputHandlerInstances.ContainsKey(name))
                    inputHandlerInstances.Add(name, handler);
            }
        }

        public static string[] GetInputHandlerNames()
        {
            if(inputHandlerInstances == null) { return null; }
            return inputHandlerInstances.Keys.Distinct().ToArray();
        }

        public static InputHandler GetInputHandler(string name)
        {
            if(inputHandlerInstances != null && !string.IsNullOrEmpty(name))
            {
                name = name.ToLower();

                if (inputHandlerInstances.ContainsKey(name))
                    return inputHandlerInstances[name];
            }

            return null;
        }

        public static InputHandler[] GetInputHandlers(string[] names)
        {
            List<InputHandler> results = new List<InputHandler>();
            if(names == null) { return results.ToArray(); }
           
            foreach(string name in names)
            {
                InputHandler current = GetInputHandler(name);
                if (current != null) results.Add(current);
            }

            return results.ToArray();
        }



        public static bool GetKey(KeyboardKey key)
        {
            if (keyboardKeysDatas == null) return false;
            return keyboardKeysDatas[key].GetKey();
        }
        public static bool GetKey(GamepadButton key)
        {
            if (gamepadKeysDatas == null) return false;
            return gamepadKeysDatas[key].GetKey();
        }

        public static bool GetKey(GamepadStick key) {
            if (gamepadStickDatas == null) return false;
            return gamepadStickDatas[key].GetKey();
        }


        public static bool GetKey(KeyboardKey[] keys)
        {
            if (keys != null) keys = keys.Distinct().ToArray();
            return keys.Where(x => GetKey(x)).Count() > 0;
        }
        public static bool GetKey(GamepadButton[] keys)
        {
            if (keys != null) keys = keys.Distinct().ToArray();
            return keys.Where(x => GetKey(x)).Count() > 0;
        }

        public static bool GetKey(GamepadStick[] keys)
        {
            if (keys != null) keys = keys.Distinct().ToArray();
            return keys.Where(x => GetKey(x)).Count() > 0;
        }

        public static bool GetKey(KeyboardKey keyboardKey, GamepadButton gamepadKey)
        {
            if (GetKey(keyboardKey) || GetKey(gamepadKey)) return true;
            return false;
        }

        public static bool GetKeyDown(KeyboardKey key)
        {
            if (keyboardKeysDatas == null) return false;
            return keyboardKeysDatas[key].GetKeyDown();
        }
        public static bool GetKeyDown(GamepadButton key)
        {
            if (gamepadKeysDatas == null) return false;
            return gamepadKeysDatas[key].GetKeyDown();
        }

        public static bool GetKeyDown(GamepadStick key) {
            if (gamepadStickDatas == null) return false;
            return gamepadStickDatas[key].GetKeyDown();
        }

        public static bool GetKeyDown(KeyboardKey[] keys)
        {
            if (keys != null) keys = keys.Distinct().ToArray();
            return keys.Where(x => GetKeyDown(x)).Count() > 0;
        }
        public static bool GetKeyDown(GamepadButton[] keys)
        {
            if (keys != null) keys = keys.Distinct().ToArray();
            return keys.Where(x => GetKeyDown(x)).Count() > 0;
        }
        public static bool GetKeyDown(GamepadStick[] keys)
        {
            if (keys != null) keys = keys.Distinct().ToArray();
            return keys.Where(x => GetKeyDown(x)).Count() > 0;
        }

        public static bool GetKeyDown(KeyboardKey keyboardKey, GamepadButton gamepadKey)
        {
            if (GetKeyDown(keyboardKey) || GetKeyDown(gamepadKey)) return true;
            return false;
        }

        public static bool GetKeyUp(KeyboardKey key)
        {
            if (keyboardKeysDatas == null) return false;
            return keyboardKeysDatas[key].GetKeyUp();
        }

        public static bool GetKeyUp(GamepadButton key)
        {
            if (gamepadKeysDatas == null) return false;
            return gamepadKeysDatas[key].GetKeyUp();
        }

        public static bool GetKeyUp(GamepadStick key)
        {
            if (gamepadStickDatas == null) return false;
            return gamepadStickDatas[key].GetKeyUp();
        }

        public static bool GetKeyUp(KeyboardKey[] keys)
        {
            if (keys != null) keys = keys.Distinct().ToArray();
            return keys.Where(x => GetKeyUp(x)).Count() > 0;
        }
        public static bool GetKeyUp(GamepadButton[] keys)
        {
            if (keys != null) keys = keys.Distinct().ToArray();
            return keys.Where(x => GetKeyUp(x)).Count() > 0;
        }
        public static bool GetKeyUp(GamepadStick[] keys)
        {
            if (keys != null) keys = keys.Distinct().ToArray();
            return keys.Where(x => GetKeyUp(x)).Count() > 0;
        }

        public static bool GetKeyUp(KeyboardKey keyboardKey, GamepadButton gamepadKey)
        {
            if (GetKeyUp(keyboardKey) || GetKeyUp(gamepadKey)) return true;
            return false;
        }

        public static float GetAxis(KeyboardKey posititveKey, KeyboardKey negativeKey)
        {
            float result = 0f;
            if (GetKey(posititveKey)) result += 1f;
            if (GetKey(negativeKey))  result -= 1f;
            return result;
        }
        public static float GetAxis(GamepadButton posititveKey, GamepadButton negativeKey)
        {
            float result = 0f;
            if (GetKey(posititveKey)) result += 1f;
            if (GetKey(negativeKey)) result -= 1f;
            return result;
        }
        public static float GetAxis(GamepadStick posititveKey, GamepadStick negativeKey)
        {
            float result = 0f;
            if (GetKey(posititveKey)) result += 1f;
            if (GetKey(negativeKey)) result -= 1f;
            return result;
        }

        public static float GetAxis(bool readLeftStick, bool getAxisX)
        {
            Vector2 result = GetGamepadVector2(readLeftStick);
            if (getAxisX) return result.x;
            return result.y;
        }

        public static float GetAxisX(KeyboardKey left, KeyboardKey right) => GetAxis(right, left);
        public static float GetAxisY(KeyboardKey up, KeyboardKey down)    => GetAxis(up, down);

        public static float GetAxisX(GamepadButton left, GamepadButton right) => GetAxis(right,left);
        public static float GetAxisY(GamepadButton up  , GamepadButton down)  => GetAxis(up, down);

        public static float GetAxisX(GamepadStick left, GamepadStick right) => GetAxis(right, left);
        public static float GetAxisY(GamepadStick up  , GamepadStick down) => GetAxis(up, down);

        public static float GetGamepadAxisX(bool readLeftStick) => GetAxis(readLeftStick, true);
        public static float GetGamepadAxisY(bool readLeftStick) => GetAxis(readLeftStick, false);

        public static Vector2 GetVector2(KeyboardKey Up, KeyboardKey Down, KeyboardKey Left, KeyboardKey Right) => Vector2.right * GetAxisX(Left, Right) + Vector2.up * GetAxisY(Up, Down);      
        public static Vector2 GetVector2(KeyboardVector2Input.Vector2Keyboard keys) => GetVector2(keys.Up, keys.Down, keys.Left, keys.Right);
      
        public static Vector2 GetGamepadVector2(bool readLeftStick) 
        {
            Gamepad gamepad = Gamepad.current;
            if (gamepad == null) return Vector2.zero;
            return readLeftStick ? gamepad.leftStick.value : gamepad.rightStick.value;
        }

        public static Vector2 GetMouseVector2()
        {
           Mouse mouse = Mouse.current;
           if(mouse == null) return Vector2.zero;
           return mouse.delta.value;
        }
    }
}
