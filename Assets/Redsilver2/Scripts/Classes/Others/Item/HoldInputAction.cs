using System.Linq;
using UnityEngine;
using UnityEngine.Windows;


namespace RedSilver2.Framework.Inputs
{
    public class HoldInputAction : SingleInputAction
    {
        public HoldInputAction(string actionName, HoldInput input) : base(actionName, input)
        {

        }

        public HoldInputAction(string actionName, HoldInput[] inputs) : base(actionName, inputs)
        {
           
        }

        public HoldInputAction(string actionName, OverrideableHoldInput input) : base(actionName, input)
        {

        }

        public HoldInputAction(string actionName, OverrideableHoldInput[] inputs) : base(actionName, inputs)
        {

        }
    }
}
