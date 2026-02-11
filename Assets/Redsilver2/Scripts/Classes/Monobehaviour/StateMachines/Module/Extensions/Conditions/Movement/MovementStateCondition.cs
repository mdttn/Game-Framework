using RedSilver2.Framework.StateMachines.Controllers;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class MovementStateCondition : MovementStateModule, ICheckableStateTransition
    {
        [SerializeField] private bool isEnabled = true;

        [Space]
        [SerializeField] private MovementStateTransitionCheck[] checkableStates;
        public bool IsEnabled => isEnabled;

        protected override void Start()
        {
            base.Start();
            AddAllTransitionChecks();
            stateMachine?.AddOnStateModuleAddedListener(OnStateModuleAdded);

        }

        protected override void OnDisable()
        {
            base.OnDisable();
            RemoveAllTransitionChecks();
            stateMachine?.RemoveOnStateModuleAddedListener(OnStateModuleAdded);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            AddAllTransitionChecks();
            stateMachine?.AddOnStateModuleAddedListener(OnStateModuleAdded);
        }

        private bool TryGetCheckStateResult(MovementStateType type, out bool returnOppositeResult)
        {
            var results = checkableStates.Where(x => x.type == type);

            if (results.Count() > 0)
            {
                returnOppositeResult = results.First().returnOppositeResult;
                return true;
            }

            returnOppositeResult = false;
            return false;
        }


        private void AddAllTransitionChecks()
        {
            if (stateMachine == null) return;
            foreach (State state in stateMachine.GetStates())  OnStateAdded(state);
        }


        private void RemoveAllTransitionChecks() {
            if(stateMachine == null) return;
            foreach(State state in stateMachine.GetStates()) OnStateRemoved(state);
        }


        protected sealed override void OnStateAdded(MovementState state)
        {
            if (state == null) return;

            if (TryGetCheckStateResult(state.Type, out bool returnOppositeResult))
            {
                state?.RemoveTransitionCheck(ModuleName);
                state?.AddTransitionCheck(ModuleName, this, returnOppositeResult);
            }
        }

        protected sealed override void OnStateRemoved(MovementState state)
        {
            state?.RemoveTransitionCheck(ModuleName);
        }

        public bool Check() {
            if (!isEnabled) {  return false; }
            return GetTransitionState();
        }

        public abstract bool GetTransitionState();
        protected abstract void OnStateModuleAdded(StateModule module);
    }
}
