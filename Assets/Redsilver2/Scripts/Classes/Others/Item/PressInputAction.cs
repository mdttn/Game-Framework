using RedSilver2.Framework.Inputs;
using System.Linq;

namespace RedSilver2.Framework.Inputs
{
    public sealed class PressInputAction : SingleInputAction
    {
        public PressInputAction(string actionName, PressInput input) : base(actionName, input)
        {

        }

        public PressInputAction(string actionName, PressInput[] inputs) : base(actionName, inputs)
        {

        }


        public PressInputAction(string actionName, float executionDelay, OverrideablePressInput input) : base(actionName, input) {

        }

        public PressInputAction(string actionName, float executionDelay, OverrideablePressInput[] inputs) : base(actionName, inputs)
        {

        }
    }
}
