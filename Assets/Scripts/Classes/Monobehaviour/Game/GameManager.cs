using RedSilver2.Framework.Inputs;
using UnityEngine;

namespace RedSilver2.Framework
{
    public sealed class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public static GameManager Instance => instance;

        private Vector2Input rotationInput;

        private void Awake()
        {
            if (instance != null) { Destroy(gameObject); return; }
            instance = this;
            DontDestroyOnLoad(instance);
        }

        private void Start()
        {
            rotationInput = new KeyboardVector2Input
            (new KeyboardVector2Input.Vector2Keyboard(KeyboardKey.W, KeyboardKey.S, 
                                                      KeyboardKey.A, KeyboardKey.D),
            true);

            rotationInput.AddOnUpdateListener(vector => Debug.Log(vector));
            rotationInput.Enable();
        }

        private void Update()
        {
            rotationInput.Update();
        }
    }
}
