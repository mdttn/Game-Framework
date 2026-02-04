using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States {
    public abstract class StateInitializer : StateModule {

        protected State defaultState;

        protected override void Start()
        {
            base.Start();
            defaultState = GetDefaultState(stateMachine);
            AddDefaultState();
        }


        protected sealed override void OnEnable() {
           base.OnEnable();
           if(didStart) AddDefaultState();
        }
        protected sealed override void OnDisable() {
           base.OnDisable();
           if(didStart) RemoveDefaultState();
        }

        protected sealed override void AddDefaultEvent() {
            AddDefaultState();
            base.AddDefaultEvent();
            stateMachine?.AddStateInitializer(this);
        }

        protected sealed override void RemoveDefaultEvent() {
            RemoveDefaultState();
            base.RemoveDefaultEvent();
            stateMachine?.RemoveStateInitializer(this);
        }
  
        private void AddDefaultState(){
            if (stateMachine == null || defaultState == null)
                return;


            if (CanAddOrRemoveState(stateMachine)) stateMachine?.AddState(defaultState.GetStateName(), defaultState);
            OnStateAdded(defaultState);
        }

        private void RemoveDefaultState() {
            if (stateMachine == null || defaultState == null)
                return;

            OnStateRemoved(defaultState);  
            if (CanAddOrRemoveState(stateMachine)) stateMachine?.RemoveState(defaultState.GetStateName());
        }

        protected abstract State GetDefaultState(StateMachine controller);
        protected abstract bool  CanAddOrRemoveState(StateMachine controller);
    }
}
