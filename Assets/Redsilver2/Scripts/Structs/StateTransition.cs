using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States {
    public struct StateTransition {
        private bool showOppositeResult;
        private IStateTransition stateTransition;

        public StateTransition(bool showOppositeResult, IStateTransition stateTransition) {
            this.showOppositeResult = showOppositeResult;
            this.stateTransition    = stateTransition;
        }

        public bool GetTransitionResult() {
            if(stateTransition == null) return true;

            bool result = stateTransition.Validate();
            return showOppositeResult ? !result : result; 
        }
    }
}
