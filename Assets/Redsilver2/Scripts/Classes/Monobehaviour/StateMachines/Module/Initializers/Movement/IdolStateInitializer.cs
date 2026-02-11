using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class IdolStateInitializer : MovementStateInitializer
    {
        protected sealed override MovementState GetDefaultState(MovementStateMachine stateMachine)
        {
            if (stateMachine == null) return null;

            if (stateMachine.ContainsState(MovementStateType.Idol))
                return stateMachine.GetState(MovementStateType.Idol) as IdolState;

            return new IdolState(stateMachine);
        }

        protected sealed override string GetModuleName()
        {
            return "Idol state Initializer";
        }
    }
}
