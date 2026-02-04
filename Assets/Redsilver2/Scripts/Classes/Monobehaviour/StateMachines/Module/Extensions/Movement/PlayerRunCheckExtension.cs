using RedSilver2.Framework.Inputs;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public class PlayerRunCheckExtension : MovementStateRunCheckExtension
    {
        private PressInput pressInput;
        private HoldInput holdInput;
        public PressInput PressInput => pressInput;
        public HoldInput HoldInput => holdInput;

        protected sealed override void Start()
        {
            pressInput = RunState.GetPressInput();
            holdInput = RunState.GetHoldInput();
            holdInput?.Enable();

            base.Start();
        }


        protected sealed override void OnDisable()
        {
            base.OnDisable();
            pressInput?.Disable();
            holdInput?.Disable();
        }

        protected sealed override void OnEnable()
        {
            base.OnEnable();
            holdInput?.Enable();
            pressInput?.Enable();
        }

        protected sealed override void CheckRun(IStateTransition groundStateTransition, bool ignoreGroundedCondition, ref bool isRunning)
        {
            if (ignoreGroundedCondition) {
                UpdateInput(ref isRunning);
            }
            else if(groundStateTransition != null) {
                if (groundStateTransition.Validate()) UpdateInput(ref isRunning);
                else                                  isRunning = false;                            
            }
        }

        private void UpdateInput(ref bool isRunning) {
            if (RunState.HasToHoldInput()){
                holdInput?.Update();
                if (holdInput != null) isRunning = holdInput.Value;
            }
            else
            {
                pressInput?.Update();
                if(pressInput != null) {
                    if (pressInput.Value) isRunning = !isRunning;
                }
            }
        }
    }
}
