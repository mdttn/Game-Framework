using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.Handlers
{
    public abstract class UpdateableStateEventHandler : StateMachineEventHandler
    {
        public void AddOnUpdateListener(UnityAction action) {
              (stateMachine as UpdateableStateMachine)?.AddOnUpdateListener(action);
        }

        public void RemoveOnUpdateListener(UnityAction action) {
            (stateMachine as UpdateableStateMachine)?.RemoveOnUpdateListener(action);
        }

        public void AddOnLateUpdateListener(UnityAction action)
        {
            (stateMachine as UpdateableStateMachine)?.AddOnLateUpdateListener(action);
        }

        public void RemoveOnLateUpdateListener(UnityAction action)
        {
            (stateMachine as UpdateableStateMachine)?.RemoveOnLateUpdateListener(action);
        }
    }
}
