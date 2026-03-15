using RedSilver2.Framework.Inputs;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Controllers
{
    public abstract class StateMachineController : MonoBehaviour
    {
        private StateMachine stateMachine;
        public  StateMachine StateMachine => stateMachine;

        protected virtual void Awake() {
            InitializeStateMachine(ref stateMachine);
        }

        protected async virtual void Update() {

            if (InputManager.GetKey(GamepadButton.RightStickLeft))
                Debug.Log("Pushing the right stick left");
             if (InputManager.GetKeyUp(GamepadButton.RightStickLeft))
                    Debug.Log("Pushing Up the right stick right");

            stateMachine?.Update();
        }

        protected virtual void LateUpdate() {
            stateMachine?.LateUpdate();
        }

        protected virtual void OnDisable() {
            stateMachine?.Disable();
        }

        protected virtual void OnEnable() {
            stateMachine?.Enable();
        }

        protected abstract void InitializeStateMachine(ref StateMachine stateMachine);
    }

}
