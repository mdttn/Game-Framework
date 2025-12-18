using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RedSilver2.Framework.Settings
{
    public sealed class TMPFramerateSelector : FramerateSelector
    {
        [SerializeField] private TextMeshProUGUI displayer;

        protected sealed override void SetUI(Button next, Button previous)
        {
            if (next != null && previous != null && displayer != null) {
                framerateSetting.SetUI(previous, next, displayer, true);
            }
        }
    }
}
