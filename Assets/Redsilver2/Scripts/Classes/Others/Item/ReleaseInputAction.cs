using System.Linq;

namespace RedSilver2.Framework.Inputs
{
    public class ReleaseInputAction : SingleInputAction
    {
        public ReleaseInputAction(string actionName, ReleaseInput input) : base(actionName, input) {

        }

        public ReleaseInputAction(string actionName, ReleaseInput[] inputs) : base(actionName, inputs) {

        }

        public ReleaseInputAction(string actionName, OverrideableReleaseInput input) : base(actionName, input) {

        }

        public ReleaseInputAction(string actionName, OverrideableReleaseInput[] inputs) : base(actionName, inputs) {

        }
    }
}
