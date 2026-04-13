using RedSilver2.Framework.StateMachines;
using RedSilver2.Framework.StateMachines.Controllers;

public class StalkerStateMachine : StateMachine
{
    private float phaseTriggerDuration;
    public  float PhaseTriggerDuration => phaseTriggerDuration;

    public StalkerStateMachine(AIMovementController controller) : base(controller) {
    
    }
}
