using RedSilver2.Framework.StateMachines;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.Handlers;
using UnityEngine;

public abstract class MovementStateExtension : MonoBehaviour
{
    protected MovementStateMachineController movementController;

    protected virtual void Start()
    {
        movementController = transform.root != null ? transform.root.GetComponentInChildren<MovementStateMachineController>()
                                                : GetComponentInChildren<MovementStateMachineController>();
    }



    protected abstract void OnDisable();
    protected abstract void OnEnable();
    private void OnStatMachineAdded(StateMachine stateMachine) {
        OnStateMachineAdded(stateMachine as MovementStateMachine);
    }

    protected abstract void OnStateMachineAdded(MovementStateMachine stateMachine);
}
