using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States;
using RedSilver2.Framework.StateMachines.States.Extensions;
using RedSilver2.Framework.StateMachines.States.Movement;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines {
    [System.Serializable]
    public class MovementStateMachine : StateMachine
    {
        public readonly MovementHandler MovementHandler;

        public MovementStateMachine(StateMachineController controller, MovementHandler movementHandler) : base(controller) {
            MovementHandler = movementHandler;
            movementHandler?.SetStateMachine(this);

            AddOnStateEnteredListener(state => {
                if(state != null)
                Debug.LogWarning("Current State: " + state.GetStateName());
            });

            AddOnStateExitedListener(state => {
                if (state != null)
                    Debug.LogWarning("Previous State: " + state.GetStateName());
            });

            AddOnStateAddedListener(state =>
            {
                if (state == null) return;

                if(state is FallState || state is LandState || state is JumpState)
                {
                    string message = $"State Name:{state.GetStateName()}  \nState Transitions:\n"; 

                    foreach(string name in state.GetTransitionStateNames()) 
                        message += name + "\n";

                    Debug.Log(message);
                }

            });
        }

        public sealed override void AddStateInitializer(StateInitializer stateInitializer) {
           if(stateInitializer is MovementStateInitializer) base.AddStateInitializer(stateInitializer);
        }

        public sealed override void AddStateExtension(StateExtension stateExtension) {
            base.AddStateExtension(stateExtension);
        }

        public void ChangeState(MovementStateType stateType) {
            ChangeState(stateType.ToString());
        }

        public bool ContainsState(MovementStateType stateType) {
            return ContainsState(stateType.ToString()); 
        }

        public void AddState(MovementStateType stateType, State state) {
            AddState(stateType.ToString(), state);  
        }

        public sealed override void AddState(string stateName, State state) {
            if(state is MovementState)  base.AddState(stateName, state);
        }

        public MovementState GetState(MovementStateType stateType) {
            return GetState(stateType.ToString()) as MovementState; 
        }
    }
}