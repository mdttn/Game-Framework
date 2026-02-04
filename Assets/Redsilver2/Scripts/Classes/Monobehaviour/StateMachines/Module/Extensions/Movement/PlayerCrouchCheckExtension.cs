using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.StateMachines.States.Extensions;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public class PlayerCrouchCheckExtension : MovementStateCrouchCheckExtension
    {
        private PressInput pressInput;
        private HoldInput holdInput;
        public PressInput PressInput => pressInput;
        public HoldInput HoldInput => holdInput;

        protected override void Start()
        {
            pressInput = CrouchState.GetPressInput();
            holdInput = CrouchState.GetHoldInput();

            pressInput.Disable();
            holdInput.Disable();

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
            pressInput?.Enable();
            holdInput?.Enable();
        }

        protected sealed override void CheckCrouch(IStateTransition groundStateTransition, bool ignoreGroundedCondition, ref bool isCrouching)
        {
            if(groundStateTransition != null) {
                if (!groundStateTransition.Validate()) {
                    isCrouching = false;
                    return;
                } 
            }

            if (CrouchState.HasToHoldInput()) {
                holdInput?.Update();
                if (holdInput != null) isCrouching = holdInput.Value; 
            }
            else
            {
                pressInput?.Update();   
                if(pressInput != null) {
                    if (pressInput.Value) isCrouching = !isCrouching;
                }
            }
        }

        protected sealed override void OnStateExtensionAdded(StateExtension extension) {

        }
    }
}
