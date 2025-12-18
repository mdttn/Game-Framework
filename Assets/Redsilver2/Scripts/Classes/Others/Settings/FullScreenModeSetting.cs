using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace RedSilver2.Framework.Settings
{
    public partial class SettingManager : MonoBehaviour
    {
        public class FullScreenModeSetting : IndexSetting
        {
            public readonly FullScreenMode[] FullScreenModes = (FullScreenMode[])Enum.GetValues(typeof(FullScreenMode));

            private FullScreenModeSetting() {

            }

            public void SetFullScreenMode(int index) {

                if (index != this.index){
                    if (index >= 0 && index < FullScreenModes.Length) {

                    }
                }
            }

            public override void SetUI(Dropdown dropdown, int[] excludedIndexes)
            {
                throw new NotImplementedException();
            }

            public override void SetUI(TMP_Dropdown dropdown, int[] excludedIndexes)
            {
                throw new NotImplementedException();
            }

            public override void SetUI(Slider slider, Text displayer, int[] excludedIndexes)
            {
                throw new NotImplementedException();
            }

            public override void SetUI(Slider slider, TextMeshProUGUI displayer, int[] excludedIndexes)
            {
                throw new NotImplementedException();
            }

            public override void SetUI(Button previousButton, Button nextButton, Text displayer, int[] excludedIndexes)
            {
                throw new NotImplementedException();
            }

            public override void SetUI(Button previousButton, Button nextButton, TextMeshProUGUI displayer, int[] excludedIndexes)
            {
                throw new NotImplementedException();
            }
        }
    }
}
