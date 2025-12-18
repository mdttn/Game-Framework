using UnityEngine;
using UnityEngine.UI;

namespace RedSilver2.Framework.Settings
{
    public sealed class ClassicFramerateSelector : FramerateSelector
    {
        [SerializeField] private Text displayer;

        protected sealed override void SetUI(Button next, Button previous)
        {
            if (next != null && previous != null && displayer != null && framerateSetting != null) {
                framerateSetting.SetUI(previous, next, displayer, true);
            }
        }
    }
}
