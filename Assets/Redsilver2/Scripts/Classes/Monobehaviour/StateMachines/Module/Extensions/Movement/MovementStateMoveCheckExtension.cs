using RedSilver2.Framework.StateMachines.States.Extensions;

namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class MovementStateMoveCheckExtension : MovementStateTransitionExtension
    {
        public override bool Validate() {
            MovementStateMachine stateMachine = this.stateMachine as MovementStateMachine;
            if (stateMachine == null || stateMachine.MovementHandler == null) return false;
            return stateMachine.MovementHandler.GetMoveMagnitude() > 0;  
        }

        protected override string GetExtensionName()
        {
            return "Move Check";
        }

        protected override MovementStateType[] GetInclusiveStates() {
            return new MovementStateType[] { 
                MovementStateType.Idol, MovementStateType.Walk, 
                MovementStateType.Run , MovementStateType.Crouch
            };
        }

        protected override MovementStateType[] GetValidResultStateTypes()
        {
            return new MovementStateType[] {
               MovementStateType.Walk, MovementStateType.Run
            };
        }

        protected sealed override void OnStateExtensionAdded(StateExtension extension) {

        }
    }
}
