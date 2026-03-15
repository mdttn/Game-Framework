using RedSilver2.Framework.Inputs;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class PlayerRunStateInitializer : RunStateInitializer
    {
        private PressInput pressInput;
        private HoldInput  holdInput;

        public PressInput PressInput => pressInput;
        public HoldInput HoldInput => holdInput;    

        protected sealed override void Awake()
        {
            base.Awake();
            pressInput  = RunState.GetPressInput();
            holdInput   = RunState.GetHoldInput();
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
            if (RunState.HasToHoldInput()) OnUpdateHoldInput();
            else OnUpdatePressInput();

            base.OnUpdate();
        }


        private void OnUpdateHoldInput()
        {
            if (holdInput == null) return;
            transitionState = holdInput.Value;
        }

        private void OnUpdatePressInput()
        {
            if (pressInput == null) return;
            if (pressInput.Value) transitionState = !transitionState;
        }


        protected sealed override string GetModuleName()  {
            return "Player " + base.GetModuleName();
        }
    }
}
