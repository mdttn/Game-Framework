using RedSilver2.Framework.StateMachines.Controllers;
using System.Linq;
using UnityEngine.Events;
using UnityEngine;


namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class MovementStateInitializer : MovementStateModule
    {
        [SerializeField] private MovementStateType[] transitionStates;
        private State defaultState;


        protected override void Start() {
            base.Start();   
            defaultState = GetDefaultState(stateMachine);

            stateMachine?.AddState(defaultState);
            AddAllTransitionStates();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            AddAllTransitionStates();
            stateMachine?.AddState(defaultState);   
        }

        protected  override void OnDisable()
        {
            base.OnDisable();
            RemoveAllTransitionStates();
            stateMachine?.RemoveState(defaultState);
        }

        private State GetDefaultState(StateMachine stateMachine)
        {
            if (!CanAddOrRemoveState(stateMachine)) return null;
            return GetDefaultState(stateMachine as MovementStateMachine);
        }

        protected sealed override void OnStateAdded(State state) {
            AddTransitionState(state);
        }
        protected sealed override void OnStateRemoved(State state) { 
            RemoveTransitionState(state);   
        }

        protected sealed override void OnStateAdded(MovementState state)
        {
            return;
        }

        protected sealed override void OnStateRemoved(MovementState state)
        {
            return;
        }

        private bool CanAddOrRemoveState(StateMachine controller) {
            return controller is MovementStateMachine;
        }

        private void AddTransitionState(State state)
        {
            if (IsTransitionState(state as MovementState))
            {
                defaultState?.AddTransitionState(state);
            }
        }

        private void RemoveTransitionState(State state)
        {
            defaultState?.RemoveTransitionState(state);
        }


        private void AddAllTransitionStates() {
            if (stateMachine == null) return;
            foreach(State state in stateMachine.GetStates()) AddTransitionState(state);
        }

        private void RemoveAllTransitionStates() {
            if (stateMachine == null) return;
            foreach (State state in stateMachine.GetStates()) RemoveTransitionState(state);
        }

        public bool IsTransitionState(MovementState state)
        {
            if (state == null || transitionStates == null) return false;
            return transitionStates.Contains(state.Type);
        }


        protected abstract MovementState GetDefaultState(MovementStateMachine stateMachine);
    }
}