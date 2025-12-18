using UnityEngine;
using UnityEngine.UI;

namespace RedSilver2.Framework.Settings
{
    public abstract class FramerateSlider : FramerateUI
    {
        [SerializeField] private Slider slider;

        protected override void OnDisable() {
            if (Application.isPlaying && slider != null) {
                slider.onValueChanged.RemoveAllListeners();
            }
        }

        protected override void OnEnable() {
            if (Application.isPlaying && slider != null){
                SetUI(slider);
            }
        }

        protected abstract void SetUI(Slider slider);

    }
}