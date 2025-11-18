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
        private static readonly Dictionary<string, InputHandler>                    inputHandlerInstances = new Dictionary<string, InputHandler>();
        private static readonly Dictionary<KeyboardKey  , InputButtonControl>       keyboardKeysDatas     = GetKeyboardKeysDatas();
        private static readonly Dictionary<GamepadButton, InputButtonControl>       gamepadKeysDatas      = GetGamepadKeysDatas();
        private static readonly Dictionary<GamepadStick , InputGamepadStickControl> gamepadSticksDatas    = GetGamepadStickDatas();

        public static bool AnyKeyboardKey {

            get
            {
               if(keyboardKeysDatas == null) return false;
               return keyboardKeysDatas.Keys.Where(x => GetKey(x)).Count() > 0;
            }
        }
        public static bool AnyGamepadButtonKey 
        {
            get
            {
                if(gamepadKeysDatas == null) return false;
                return gamepadKeysDatas.Keys.Where(x => GetKey(x)).Count() > 0;
            }
        }
        public static bool AnyGamepadStickKey
        {
            get
            {
                if (gamepadSticksDatas == null) return false;
                return gamepadSticksDatas.Keys.Where(x => GetKey(x)).Count() > 0;
            }
        }

        public static bool AnyKeyboardKeyDown
        {
            get
            {
                if (keyboardKeysDatas == null) return false;
                return keyboardKeysDatas.Keys.Where(x => GetKeyDown(x)).Count() > 0;
            }
        }
        public static bool AnyGamepadButtonKeyDown
        {
            get
            {
                if (gamepadKeysDatas == null) return false;
                return gamepadKeysDatas.Keys.Where(x => GetKeyDown(x)).Count() > 0;
            }
        }
        public static bool AnyGamepadStickKeyDown
        {
            get
            {
                if (gamepadSticksDatas == null) return false;
                return gamepadSticksDatas.Keys.Where(x => GetKeyDown(x)).Count() > 0;
            }
        }

        public static bool AnyKeyboardKeyUp
        {
            get
            {
                if (keyboardKeysDatas == null) return false;
                return keyboardKeysDatas.Keys.Where(x => GetKeyUp(x)).Count() > 0;
            }
        }
        public static bool AnyGamepadButtonKeyUp
        {
            get
            {
                if (gamepadKeysDatas == null) return false;
                return gamepadKeysDatas.Keys.Where(x => GetKeyUp(x)).Count() > 0;
            }
        }
        public static bool AnyGamepadStickKeyUp
        {
            get
            {
                if (gamepadSticksDatas == null) return false;
                return gamepadSticksDatas.Keys.Where(x => GetKeyUp(x)).Count() > 0;
            }
        }

        public static bool AnyKey     => AnyKeyboardKey     || AnyGamepadButtonKey     || AnyGamepadButtonKey;
        public static bool AnyKeyDown => AnyKeyboardKeyDown || AnyGamepadButtonKeyDown || AnyGamepadButtonKeyDown;
        public static bool AnyKeyUp   => AnyKeyboardKeyUp   || AnyGamepadButtonKeyUp   || AnyGamepadStickKeyUp;


        public const string KEYBOARD_ICONS_PATH     = "Sprites/Inputs/Keyboard/";
        public const string GAMEPAD_ICONS_PATH      = "Sprites/Inputs/Gamepad/";
        public const string XR_ICONS_PATH           = "Sprites/Inputs/XR/";

        public const string KEYBOARD_ROOT_PATH      = "<Keyboard>/";
        public const string GAMEPAD_ROOT_PATH       = "<Gamepad>/";
        public const string XR_CONTROLLER_ROOT_PATH = "<XRController>/";


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

            foreach (GamepadStick key in Enum.GetValues(typeof(GamepadStick))) 
                results.Add(key, new InputGamepadStickControl(key, GetKeyIconFromResources(key.ToString(), GAMEPAD_ICONS_PATH)));

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
            if (gamepadSticksDatas == null || !gamepadSticksDatas.ContainsKey(key)) return null;
            return gamepadSticksDatas[key].Icon;
        }

        public static void OverrideKeyIcon(GamepadStick key, Sprite icon)
        {
            if (gamepadSticksDatas == null || !gamepadSticksDatas.ContainsKey(key)) return;
            gamepadSticksDatas[key].OverrideIcon(icon);
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

        private static InputHandler GetInputHandler(string name)
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
            if (gamepadSticksDatas == null) return false;
            return gamepadSticksDatas[key].GetKey();
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
        public static bool GetKey(KeyboardKey keyboardKey, GamepadStick gamepadStick)
        {
            if (GetKey(keyboardKey) || GetKey(gamepadStick)) return true;
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
            if (gamepadSticksDatas == null) return false;
            return gamepadSticksDatas[key].GetKeyDown();
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
        public static bool GetKeyDown(KeyboardKey keyboardKey, GamepadStick gamepadStick)
        {
            if (GetKeyDown(keyboardKey) || GetKeyDown(gamepadStick)) return true;
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
            if (gamepadSticksDatas == null) return false;
            return gamepadSticksDatas[key].GetKeyUp();
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
        public static bool GetKeyUp(KeyboardKey keyboardKey, GamepadStick gamepadStick)
        {
            if (GetKeyUp(keyboardKey) || GetKeyUp(gamepadStick)) return true;
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
        public static float GetAxis(Vector2GamepadStick stick, bool getAxisX) {
            return GetAxis(GetStickControl(stick), getAxisX);
        }

        private static float GetAxis(Vector2Control control, bool getAxisX)
        {
            float result = 0f;
            if(control == null) return 0f;

            if (getAxisX == true) {
                if (control.x.value > 0.5f) result += 1f;
                if (control.x.value < -0.5f) result -= 1f;
            }
            else {
                if (control.y.value > 0.5f) result += 1f;
                if (control.y.value < -0.5f) result -= 1f;
            }

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

        public static float GetAxisX(Vector2GamepadStick stick) => GetAxis(stick, true);
        public static float GetAxisY(Vector2GamepadStick stick) => GetAxis(stick, false);

        public static float GetAxisX(bool readLeftStick) => GetAxisX(readLeftStick ? Vector2GamepadStick.LeftStick : Vector2GamepadStick.RightStick);
        public static float GetAxisY(bool readLeftStick) => GetAxisY(readLeftStick ? Vector2GamepadStick.LeftStick : Vector2GamepadStick.RightStick);

        public static Vector2 GetKeyboardVector2(KeyboardKey Up, KeyboardKey Down, KeyboardKey Left, KeyboardKey Right) => Vector2.right * GetAxisX(Left, Right) + Vector2.up * GetAxisY(Up, Down);      
        public static Vector2 GetKeyboardVector2(KeyboardVector2Input.Vector2Keyboard keys) => GetKeyboardVector2(keys.Up, keys.Down, keys.Left, keys.Right);

        public static Vector2 GetGamepadVector2(Vector2GamepadStick stick) {
            return GetGamepadVector2(stick == Vector2GamepadStick.LeftStick ? true : false);
        }
        public static Vector2 GetGamepadVector2(bool readLeftStick) {
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
        private static Vector2Control GetStickControl(Vector2GamepadStick stick) {
            return InputSystem.FindControl($"{GAMEPAD_ROOT_PATH}{GetFormattedKey(stick.ToString())}") as Vector2Control;
        }

        public static PressInput GetPressInput(string name)  {
            return GetInputHandler(name) as PressInput;
        }
        public static HoldInput GetHoldInput(string name) {
            return GetInputHandler(name) as HoldInput;
        }
        public static ReleaseInput GetReleaseInput(string name) {
            return GetInputHandler(name) as ReleaseInput;
        }
        public static Vector2Input GetVector2Input(string name) {
            return GetInputHandler(name) as Vector2Input;
        }
        public static MouseVector2Input GetMouseVector2Input(string name) {
            return GetInputHandler(name) as MouseVector2Input;
        }
        public static KeyboardVector2Input GetKeyboardVector2Input(string name)
        {
            return GetInputHandler(name) as KeyboardVector2Input;
        }

        public static OverrideablePressInput   GetOverrideablePressInput(string name) {
            return GetPressInput(name) as OverrideablePressInput;
        }
        public static OverrideableHoldInput    GetOverrideableHoldInput(string name)
        {
            return GetHoldInput(name) as OverrideableHoldInput;
        }
        public static OverrideableReleaseInput GetOverrideableReleaseInput(string name) {
            return GetReleaseInput(name) as OverrideableReleaseInput;
        }
        public static OverrideableVector2Input GetOverrideableVector2Input(string name)
        {
            return GetInputHandler(name) as OverrideableVector2Input;
        }
       
        // I need to add OverreadbleMouseInput Class !!!
        public static OverrideableKeyboardVector2Input GetOverrideableKeyboardVector2Input(string name)
        {
            return GetInputHandler(name) as OverrideableKeyboardVector2Input;
        }


        public static PressInput GetOrCreatePressInput(string name, KeyboardKey defaultKey, GamepadButton defaultButton)
        {
            PressInput result = GetPressInput(name);
            if (result != null) return result;
            return new PressInput(name, defaultKey, defaultButton);
        }
        public static HoldInput GetOrCreateHoldInput(string name, KeyboardKey defaultKey, GamepadButton defaultButton)
        {
            HoldInput result = GetHoldInput(name);
            if (result != null) return result;
            return new HoldInput(name, defaultKey, defaultButton);
        }
        public static ReleaseInput GetOrCreateReleaseInput(string name, KeyboardKey defaultKey, GamepadButton defaultButton)
        {
            ReleaseInput result = GetReleaseInput(name);
            if (result != null) return result;
            return new ReleaseInput(name, defaultKey, defaultButton);
        }
        public static MouseVector2Input GetOrCreateMouseVector2Input(string name, Vector2GamepadStick stick)
        {
            MouseVector2Input result = GetMouseVector2Input(name);
            if (result != null) return result;
            return new MouseVector2Input(name, stick);
        }
        public static KeyboardVector2Input GetOrCreateKeyboardVector2Input(string name, KeyboardVector2Input.Vector2Keyboard keyboard, Vector2GamepadStick stick)
        {
            KeyboardVector2Input result = GetKeyboardVector2Input(name);
            if(result != null) return result;
            return new KeyboardVector2Input(name, keyboard, stick);
        }

        public static OverrideablePressInput GetOrCreateOverrideablePressInput(string name, KeyboardKey defaultKey, GamepadButton defaultButton)
        {
            OverrideablePressInput result = GetOverrideablePressInput(name);
            if (result != null) return result;
            return new OverrideablePressInput(name, defaultKey, defaultButton);
        }
        public static OverrideableHoldInput GetOrCreateOverrideableHoldInput(string name, KeyboardKey defaultKey, GamepadButton defaultButton)
        {
            OverrideableHoldInput result = GetOverrideableHoldInput(name);
            if (result != null) return result;
            return new OverrideableHoldInput(name, defaultKey, defaultButton);
        }
        public static OverrideableReleaseInput GetOrCreateOverrideableReleaseInput(string name, KeyboardKey defaultKey, GamepadButton defaultButton)
        {
            OverrideableReleaseInput result = GetOverrideableReleaseInput(name);
            if (result != null) return result;
            return new OverrideableReleaseInput(name, defaultKey, defaultButton);
        }
        public static OverrideableKeyboardVector2Input GetOrCreateOverrideableKeyboardVector2Input(string name, KeyboardVector2Input.Vector2Keyboard keyboard, Vector2GamepadStick stick)
        {
            OverrideableKeyboardVector2Input result = GetOverrideableKeyboardVector2Input(name);
            if (result != null) return result;
            return new OverrideableKeyboardVector2Input(name, keyboard, stick);
        }

        public static OverrideableKeyboardVector2Input GetOrCreateOverrideableKeyboardVector2Input(string name, KeyboardVector2Input.Vector2Keyboard keyboard)
        {
            return GetOrCreateOverrideableKeyboardVector2Input(name,
                                                               keyboard,
                                                               Vector2GamepadStick.LeftStick);
        }

        public static OverrideableKeyboardVector2Input GetOrCreateOverrideableKeyboardVector2Input(string name, Vector2GamepadStick stick)
        {
            return GetOrCreateOverrideableKeyboardVector2Input(name,
                                                               new KeyboardVector2Input.Vector2Keyboard(KeyboardKey.W, KeyboardKey.S, KeyboardKey.A, KeyboardKey.D),
                                                               stick);
        }

        public static OverrideableKeyboardVector2Input GetOrCreateOverrideableKeyboardVector2Input(string name)
        {
            return GetOrCreateOverrideableKeyboardVector2Input(name,
                   new KeyboardVector2Input.Vector2Keyboard(KeyboardKey.W, KeyboardKey.S, KeyboardKey.A, KeyboardKey.D));
        }
    }
}
