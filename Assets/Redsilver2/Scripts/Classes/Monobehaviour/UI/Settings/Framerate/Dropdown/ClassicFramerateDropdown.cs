using UnityEngine;
using UnityEngine.UI;

namespace RedSilver2.Framework.Settings
{
    public sealed class ClassicFramerateDropdown : FramerateUI
    {
        [SerializeField] private Dropdown dropdown;

        protected sealed override void OnDisable()
        {
            if (Application.isPlaying && dropdown != null) {
                dropdown.ClearOptions();
                dropdown.onValueChanged.RemoveAllListeners();
            }
        }

        protected sealed override void OnEnable()
        {
            if(Application.isPlaying && dropdown != null) {
                framerateSetting.SetUI(dropdown, true);
            }
        }
    }
}
