using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public class SingleInputAction : InputAction
    {
        private SingleInput[] inputs;

        public SingleInputAction(string actionName, SingleInput input) : base(actionName)
        {
            this.inputs    = new SingleInput[1];
            this.inputs[0] = input;

            AddOnEnabledListener(EnableInputs);
            AddOnDisabledListener(DisableInputs);
        }

        public SingleInputAction(string actionName, SingleInput[] inputs) : base(actionName)
        {
            this.inputs = inputs;

            AddOnEnabledListener(EnableInputs);
            AddOnDisabledListener(DisableInputs);
        }

        private void EnableInputs()
        {
            if (inputs == null) return;
            foreach (SingleInput input in inputs.Where(x => x != null))
                input.Enable();
        }

        private void DisableInputs()
        {
            if(inputs == null) return;
            foreach (SingleInput input in inputs.Where(x => x != null))
                input.Disable();
        }

        protected sealed override bool CanExecute()
        {
            Debug.Log(this + " " + inputs.Length);

            if (inputs == null || inputs.Length == 0) return false;
            return inputs.Where(x => x != null).Where(x => x.Value).Count() == inputs.Where(x => x != null).Count(); 
        }
    }
}
