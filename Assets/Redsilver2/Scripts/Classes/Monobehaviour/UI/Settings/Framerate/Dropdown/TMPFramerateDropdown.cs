using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.Settings
{
    public sealed class TMPFramerateDropdown : FramerateUI
    {
        [SerializeField] private TMP_Dropdown dropdown;

        protected sealed override void OnDisable()
        {
            if (Application.isPlaying && framerateSetting != null && dropdown != null)
            {
                dropdown.ClearOptions();
                dropdown.onValueChanged.RemoveAllListeners();
            }
        }

        protected sealed override void OnEnable()
        {
            if (Application.isPlaying && framerateSetting != null && dropdown != null) {
                framerateSetting.SetUI(dropdown, true);
            }
        }
    }
}
