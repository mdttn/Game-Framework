using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class StateModule : MonoBehaviour
    {
        protected StateMachine     stateMachine;
        private List<State>        registeredStates;

        private UnityAction<State> onStateAdded;
        private UnityAction<State> onStateRemoved;

        protected virtual void Awake() {
            registeredStates = new List<State>();

            onStateAdded   = GetOnStateAddedAction();
            onStateRemoved = GetOnStateRemovedAction();
        }

        protected virtual void Start() {
            SetStateMachine(ref stateMachine);
            AddDefaultEvent();
        }

        protected virtual void OnEnable() {
           if(didStart) AddDefaultEvent();
        }

        protected virtual void OnDisable() {
           if (didStart) RemoveDefaultEvent();
        }

        protected virtual void AddDefaultEvent() {
            if (stateMachine == null) return;
            foreach (State state in stateMachine.GetStates()) OnStateAdded(state);

            stateMachine?.AddOnStateAddedListener(OnStateAdded);
            stateMachine?.AddOnStateRemovedListener(OnStateRemoved);
        }

        protected virtual void RemoveDefaultEvent() {
            if (stateMachine == null) return;
            foreach (State state in stateMachine.GetStates()) OnStateRemoved(state);

            stateMachine?.RemoveOnStateAddedListener(OnStateAdded);
            stateMachine?.RemoveOnStateRemovedListener(OnStateRemoved);
        }


        protected virtual void OnStateAdded(State state) {
            if (!registeredStates.Contains(state)) {

                registeredStates?.Add(state);
                onStateAdded?.Invoke(state);
            }
        }

        protected virtual void OnStateRemoved(State state) {
            if (registeredStates.Contains(state)) {
                registeredStates?.Remove(state);
                onStateRemoved?.Invoke(state);
            }
        }

        protected abstract void SetStateMachine(ref StateMachine stateMachine);
        protected abstract UnityAction<State> GetOnStateAddedAction();
        protected abstract UnityAction<State> GetOnStateRemovedAction();
    }
}
