using RedSilver2.Framework.Inputs;
using UnityEngine;

namespace RedSilver2.Framework
{
    public sealed class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public static GameManager Instance => instance;

        private HoldInput    holdInput;
        private PressInput   pressInput;
        private ReleaseInput releaseInput;

        private void Awake()
        {
            if (instance != null) { Destroy(gameObject); return; }
            instance = this;
            DontDestroyOnLoad(instance);
        }

        private void Start()
        {
            holdInput    = new HoldInput (KeyboardKey.Alpha1, GamepadKey.ButtonEast);
            pressInput   = new PressInput(KeyboardKey.Alpha2, GamepadKey.ButtonNorth);
            releaseInput = new ReleaseInput(KeyboardKey.Alpha3, GamepadKey.ButtonSouth);

            holdInput.AddOnUpdateListener(() => Debug.Log($"Is Holding Input | {holdInput.GetKeysPaths()}"));
            pressInput.AddOnUpdateListener(() => Debug.Log($"Is Pressing Input | {pressInput.GetKeysPaths()}"));
            releaseInput.AddOnUpdateListener(() => Debug.Log($"Is Input Released | {releaseInput.GetKeysPaths()}"));
           
            holdInput.Enable();
            pressInput.Enable();
            releaseInput.Enable();  
        }

        private void Update()
        {
             holdInput.Update();
             pressInput.Update();
             releaseInput.Update();
        }
    }
}
