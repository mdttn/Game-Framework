using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.StateMachines.States;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    public class PlayerCrouchStateIntializer : CrouchStateInitializer
    {
        private PressInput pressInput;
        private HoldInput  holdInput;

        public PressInput PressInput => pressInput;
        public HoldInput HoldInput => holdInput;

        protected sealed override void Awake()
        {
            base.Awake();
            pressInput = CrouchState.GetPressInput();
            holdInput  = CrouchState.GetHoldInput();
        }

        protected sealed override void Start()
        {
            base.Start();
            pressInput?.Enable();
            holdInput?.Enable();
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


        protected sealed override void OnUpdate()
        {
            if (CanResetCrouch) {
                if (CrouchState.HasToHoldInput()) OnUpdateHoldInput();
                else OnUpdatePressInput();
            }
            else {
                transitionState = true;
            }

            base.OnUpdate();
        }

        private void OnUpdateHoldInput()
        {
            if (holdInput == null || !holdInput.IsEnabled)
            {
                transitionState = false;
                return;
            }

            transitionState = holdInput.Value;
        }

        private void OnUpdatePressInput()
        {
            if (pressInput == null || !pressInput.IsEnabled)
            {
                transitionState = false;
                return;
            }

            if(pressInput.Value) transitionState = !transitionState;
        }
    }
}
