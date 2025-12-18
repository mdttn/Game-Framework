using UnityEngine.Events;
using UnityEngine.UI;

namespace RedSilver2.Framework.UI
{
    public static class UIExtension
    {
        public static void AddOnClickListener   (this Button button, UnityAction onClick)
        {
            if (button != null && onClick != null)
                 button.onClick.AddListener(onClick);
        }
        public static void RemoveOnClickListener(this Button button, UnityAction onClick)
        {
            if (button != null && onClick != null)
                button.onClick.RemoveListener(onClick);
        }

        public static void AddOnValueChangedListener   (this Slider slider, UnityAction<float> onValueChanged) 
        {
            if(slider != null && onValueChanged != null)
                slider.onValueChanged.AddListener(onValueChanged);
        }
        public static void RemoveOnValueChangedListener(this Slider slider, UnityAction<float> onValueChanged)
        {
            if (slider != null && onValueChanged != null)
                slider.onValueChanged.RemoveListener(onValueChanged);
        }

        public static void AddOnValueChangedListener   (this Toggle toggle, UnityAction<bool> onValueChanged)
        {
            if(toggle != null && onValueChanged != null)
                toggle.onValueChanged.AddListener(onValueChanged);
        }
        public static void RemoveOnValueChangedListener(this Toggle toggle, UnityAction<bool> onValueChanged)
        {
            if (toggle != null && onValueChanged != null)
                toggle.onValueChanged.RemoveListener(onValueChanged);
        }
    }
}
