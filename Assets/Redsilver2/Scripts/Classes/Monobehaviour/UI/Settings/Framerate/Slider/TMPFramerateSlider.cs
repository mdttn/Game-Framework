using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace RedSilver2.Framework.Settings
{
    public sealed class TMPFramerateSlider : FramerateSlider
    {
        [SerializeField] private TextMeshProUGUI displayer;

        protected override void SetUI(Slider slider) {
            if(framerateSetting != null){
                framerateSetting.SetUI(slider, displayer, true);
            }
        }
    }
}
