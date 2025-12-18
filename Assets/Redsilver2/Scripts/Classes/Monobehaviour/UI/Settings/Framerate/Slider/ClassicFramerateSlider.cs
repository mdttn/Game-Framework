using UnityEngine;
using UnityEngine.UI;

namespace RedSilver2.Framework.Settings
{
    public sealed class ClassicFramerateSlider : FramerateSlider
    {
        [SerializeField] private Text displayer;

        protected override void SetUI(Slider slider)
        {
            if(slider != null && displayer != null && framerateSetting != null) {
                framerateSetting.SetUI(slider, displayer, true);
            }
        }
    }
}
