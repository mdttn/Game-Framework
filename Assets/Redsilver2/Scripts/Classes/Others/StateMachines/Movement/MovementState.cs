using RedSilver2.Framework.StateMachines.States.Movement;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{

    public abstract class MovementState : UpdateableState
    {
        public readonly MovementStateType   Type;
        public readonly MovementStateType[] IncompatibleTransitionStates;
        public readonly MovementHandler     MovementHandler;

        protected MovementState(MovementStateMachine owner) : base(owner) {
            SetMovementHandler(owner, ref MovementHandler);

            SetPlayerStateType(ref Type);
            SetIncompatibleStateTransitions(ref IncompatibleTransitionStates);
            
            IncompatibleTransitionStates = IncompatibleTransitionStates.Distinct().ToArray();
            owner?.AddState(Type, this);
        }


        private void SetMovementHandler(MovementStateMachine stateMachine, ref MovementHandler handler) {
            if (stateMachine != null) handler = stateMachine.MovementHandler;
        }

        public sealed override void AddTransitionState(State state)
        {
            if(state is MovementState) base.AddTransitionState(state);
        }


        protected bool IsValidTransitionState(MovementStateType stateType)  {
            if (IncompatibleTransitionStates == null || owner == null) return false;
            return !IncompatibleTransitionStates.Contains(stateType);
        }

        protected sealed override bool IsValidTransitionState(State state) {
            return IsValidTransitionState(state as MovementState);
        }

        protected bool IsValidTransitionState(MovementState state) {
            if  (state == null || IncompatibleTransitionStates == null) return false;
            return IsValidTransitionState(state.Type);
        }

        protected sealed override void AddRequiredTransitionStates(StateMachine stateMachine) {
           // AddRequiredTransitionStates(stateMachine as MovementStateMachine);
        }

        public sealed override void AddTransitionCheck(string transitionName, IStateTransition transition, bool showOppositeResult) {
             if(transition is MovementStateTransitionExtension) 
                 base.AddTransitionCheck(transitionName, transition, showOppositeResult);
        }

        public sealed override string GetStateName()
        {
            return Type.ToString();
        }

        protected abstract void AddRequiredTransitionStates(MovementStateMachine stateMachine);
        protected abstract void SetPlayerStateType(ref MovementStateType type);
        protected abstract void SetIncompatibleStateTransitions(ref MovementStateType[] results);
        public static MovementStateType[] GetStateTypes(){
            return ((MovementStateType[])Enum.GetValues(typeof(MovementStateType)));
        }

        public static MovementStateType[] GetExcludedStateTypes(MovementStateType[] ignoredStates) {
            if(ignoredStates == null || ignoredStates.Length == 0) return GetStateTypes();
            MovementStateType[] results = GetStateTypes();

            foreach(MovementStateType type in ignoredStates) 
                results = results.Where(x => x != type).Distinct().ToArray();

            return results;
        }

        public static MovementStateType[] GetIncludedStateTypes(MovementStateType[] includedStates)
        {
            if (includedStates == null || includedStates.Length == 0) return GetStateTypes();
            MovementStateType[] results = GetStateTypes();

            foreach (MovementStateType type in includedStates) results = results.Where(x => x == type).Distinct().ToArray();
            return results;
        }
    }
}
