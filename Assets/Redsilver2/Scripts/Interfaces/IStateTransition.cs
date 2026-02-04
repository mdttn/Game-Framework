using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public interface IStateTransition {
        bool Validate();
    }
}
