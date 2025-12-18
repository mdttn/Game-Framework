using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.Settings
{
        public abstract class IndexSetting : Setting
        {
            protected int index;
            public abstract void SetUI(UnityEngine.UI.Dropdown dropdown, int[] excludedIndexes);
            public abstract void SetUI(TMP_Dropdown dropdown, int[] excludedIndexes);
            public abstract void SetUI(UnityEngine.UI.Slider slider, UnityEngine.UI.Text displayer, int[] excludedIndexes);
            public abstract void SetUI(UnityEngine.UI.Slider slider, TextMeshProUGUI displayer, int[] excludedIndexes);
            public abstract void SetUI(UnityEngine.UI.Button previousButton, UnityEngine.UI.Button nextButton, UnityEngine.UI.Text displayer, int[] excludedIndexes);
            public abstract void SetUI(UnityEngine.UI.Button previousButton, UnityEngine.UI.Button nextButton, TextMeshProUGUI displayer, int[] excludedIndexes);
        }
}
