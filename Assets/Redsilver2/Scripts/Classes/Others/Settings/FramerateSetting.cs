using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Settings
{
        public sealed class FramerateSetting : IndexSetting {

            public readonly uint[] FramerateLimits = (new uint[] {
               30, 60, 100, 120, 144, 165, 180, 240, 360, uint.MaxValue
            }).Distinct().OrderBy(x => x).ToArray();

            public static readonly FramerateSetting Instance = new FramerateSetting();

            private FramerateSetting() {
                SetFrameRateLimit(this.index);
            }

            public override void SetUI(UnityEngine.UI.Slider slider, UnityEngine.UI.Text displayer, int[] excludedIndexes)
            {
                SetUI(slider, displayer, GetFramerateLimits(excludedIndexes));
            }
            public void SetUI(UnityEngine.UI.Slider slider, UnityEngine.UI.Text displayer, bool allowUnlimitedFrames)
            {
                SetUI(slider, displayer, GetFramerateLimits(allowUnlimitedFrames));
            }
            public void SetUI(UnityEngine.UI.Slider slider, UnityEngine.UI.Text displayer, uint[] values)
            {
                if (slider == null || displayer == null || values == null || values.Length == 0) return;
                slider.onValueChanged.RemoveAllListeners();
                slider.wholeNumbers = true;

                slider.minValue = 0;
                slider.maxValue = values.Length - 1;

                slider.onValueChanged.AddListener(GetOnSliderValueChangedAction(displayer, values));
                displayer.text = GetFramerateString(index);
            }

            public override void SetUI(UnityEngine.UI.Slider slider, TextMeshProUGUI displayer, int[] excludedIndexes)
            {
                SetUI(slider, displayer, GetFramerateLimits(excludedIndexes));
            }
            public void SetUI(UnityEngine.UI.Slider slider, TextMeshProUGUI displayer, bool allowUnlimitedFrames)
            {
                SetUI(slider, displayer, GetFramerateLimits(allowUnlimitedFrames));
            }
            public void SetUI(UnityEngine.UI.Slider slider, TextMeshProUGUI displayer, uint[] values) {
                if (slider == null || displayer == null || values == null || values.Length == 0) return;
                slider.onValueChanged.RemoveAllListeners();
                slider.wholeNumbers = true;

                slider.minValue = 0;
                slider.maxValue = values.Length - 1;

                slider.onValueChanged.AddListener(GetOnSliderValueChangedAction(displayer, values));
            }

            public sealed override void SetUI(UnityEngine.UI.Dropdown dropdown, int[] excludedIndexes)
            {
                SetUI(dropdown, GetFramerateLimits(excludedIndexes));
            }
            public void SetUI(UnityEngine.UI.Dropdown dropdown, bool allowUnlimitedFrames)
            {
                SetUI(dropdown, GetFramerateLimits(allowUnlimitedFrames));
            }
            public void SetUI(UnityEngine.UI.Dropdown dropdown, uint[] values)
            {
                if (dropdown == null || values == null || values.Length == 0) return;

                dropdown.ClearOptions();
                dropdown.onValueChanged.RemoveAllListeners();

                dropdown.AddOptions(GetFramerateLimitsOptions(values));
                dropdown.onValueChanged.AddListener(SetFrameRateLimit);
                dropdown.value = this.index;
            }

            public sealed override void SetUI(TMP_Dropdown dropdown, int[] excludedIndexes) {
                SetUI(dropdown, GetFramerateLimits(excludedIndexes));
            }
            public void SetUI(TMP_Dropdown dropdown, bool allowUnlimitedFrames) {
                SetUI(dropdown, GetFramerateLimits(allowUnlimitedFrames));
            }
            public void SetUI(TMP_Dropdown dropdown, uint[] values)
            {
                if (dropdown == null || values == null || values.Length == 0) return;

                dropdown.ClearOptions();
                dropdown.onValueChanged.RemoveAllListeners();


                dropdown.AddOptions(GetFramerateLimitsTMPOptions(values));
                dropdown.onValueChanged.AddListener(SetFrameRateLimit);
            }

            public sealed override void SetUI(UnityEngine.UI.Button previousButton, UnityEngine.UI.Button nextButton, UnityEngine.UI.Text displayer, int[] excludedIndexes) {
                SetUI(previousButton, nextButton, displayer, GetFramerateLimits(excludedIndexes));
            }
            public void SetUI(UnityEngine.UI.Button previousButton, UnityEngine.UI.Button nextButton, UnityEngine.UI.Text displayer, bool allowUnlimitedFrames) {
                SetUI(previousButton, nextButton, displayer, GetFramerateLimits(allowUnlimitedFrames));
            }
            public void SetUI(UnityEngine.UI.Button previousButton, UnityEngine.UI.Button nextButton, UnityEngine.UI.Text displayer, uint[] values)
            {
                if (previousButton  == null || nextButton == null || displayer == null) return;
                if (values == null || values.Length == 0)             return;

                previousButton.onClick.RemoveAllListeners();
                nextButton.onClick.RemoveAllListeners();

                previousButton.onClick.AddListener(GetOnClickPreviousButtonAction(displayer, values));
                nextButton.onClick.AddListener(GetOnClickNextButtonAction(displayer, values));

                displayer.text = GetFramerateString(index);
            }
           
            public sealed override void SetUI(UnityEngine.UI.Button previousButton, UnityEngine.UI.Button nextButton, TextMeshProUGUI displayer, int[] excludedIndexes)
            {
                SetUI(previousButton, nextButton, displayer, GetFramerateLimits(excludedIndexes));
            }
            public void SetUI(UnityEngine.UI.Button previousButton, UnityEngine.UI.Button nextButton, TextMeshProUGUI displayer, bool allowUnlimitedFrames)
            {
                SetUI(previousButton, nextButton, displayer, GetFramerateLimits(allowUnlimitedFrames));
            }
            public void SetUI(UnityEngine.UI.Button previousButton, UnityEngine.UI.Button nextButton, TextMeshProUGUI displayer, uint[] values)
            {
                if (previousButton == null || nextButton == null || displayer == null) return;
                if (values == null || values.Length == 0) return;

                previousButton.onClick.RemoveAllListeners();
                nextButton.onClick.RemoveAllListeners();

                previousButton.onClick.AddListener(GetOnClickPreviousButtonAction(displayer, values));
                nextButton.onClick.AddListener(GetOnClickNextButtonAction(displayer, values));

                displayer.text = GetFramerateString(index);
            }

            public void SetFrameRateLimit(int index)
            {
                if (index != this.index && SettingManager != null) {
                SettingManager.SettingData data = SettingManager.Data;


                    if (index >= 0 && index < FramerateLimits.Length && data != null) {
                        data.FramerateLimitIndex = index;
                        this.index = index;

                        Application.targetFrameRate = (int)FramerateLimits[index];
                        if (SaveAutomatically) SettingManager.Save();
                    }
                }
            }

            private string GetFramerateString(int index) {
                if (FramerateLimits == null || FramerateLimits.Length == 0 || index < 0 || index >= FramerateLimits.Length)
                    return string.Empty;

                return GetFramerateString(FramerateLimits[index]);
            }
            private string GetFramerateString(uint value)
            {
                if (value == uint.MaxValue) return "Unlimited FPS";
                return $"{value} FPS";
            }

            private uint[] GetFramerateLimits(bool allowUnlimitedFrames)
            {
                if (FramerateLimits != null)
                    return GetFramerateLimits(allowUnlimitedFrames ? new int[0] : new int[] { FramerateLimits.Length - 1 });
                return new uint[0];
            }
            private uint[] GetFramerateLimits(int[] excludedIndexes)
            {
                List<uint> results = new List<uint>();
                if (FramerateLimits != null || excludedIndexes == null) results.ToArray();

                excludedIndexes = excludedIndexes.Distinct().ToArray();

                for (int i = 0; i < FramerateLimits.Length; i++)
                {
                    if (excludedIndexes.Contains(i)) continue;
                    results.Add(FramerateLimits[i]);
                }

                return results.ToArray();
            }

            private UnityAction<float> GetOnSliderValueChangedAction(UnityEngine.UI.Text displayer, uint[] values)
            {
                if(values == null || values.Length == 0 || displayer == null) return null;
                return value =>
                {
                    int index = (int)value;
                    if (displayer != null) displayer.text = GetFramerateString(index);
                    SetFrameRateLimit(index);
                };
            }
            private UnityAction<float> GetOnSliderValueChangedAction(TextMeshProUGUI displayer, uint[] values)
            {
                if (values == null || values.Length == 0 || displayer == null) return null;
                return value =>
                {
                    int index = (int)value;
                    if (displayer != null) displayer.text = GetFramerateString(index);
                    SetFrameRateLimit(index);
                };
            }

            private UnityAction GetOnClickPreviousButtonAction(UnityEngine.UI.Text displayer, uint[] values)
            {
                if(displayer == null || values == null || values.Length == 0) return null;

                return () => {
                    index--;

                    if (index < 0) {
                        if (CanWrapIndex) index = values.Length - 1;
                        else              index = 0;
                    }


                    if (displayer != null) displayer.text = GetFramerateString(index);
                    SetFrameRateLimit(index);
                };
            }
            private UnityAction GetOnClickNextButtonAction(UnityEngine.UI.Text displayer, uint[] values)
            {
                if (displayer == null || values == null || values.Length == 0) return null;

                return () => {
                    index++;

                    if (index >= values.Length) {
                        if (CanWrapIndex) index = 0;
                        else              index = values.Length - 1;
                    }

                    if (displayer != null) displayer.text = GetFramerateString(index);
                    SetFrameRateLimit(index);
                };
            }

            private UnityAction GetOnClickPreviousButtonAction(TextMeshProUGUI displayer, uint[] values)
            {
                if (displayer == null || values == null || values.Length == 0) return null;

                return () => {
                    index--;

                    if (index < 0)
                    {
                        Debug.Log(CanWrapIndex);
                        if (CanWrapIndex) index = values.Length - 1;
                        else index = 0;
                    }

                    if (displayer != null) displayer.text = GetFramerateString(index);
                    SetFrameRateLimit(index);
                };
            }
            private UnityAction GetOnClickNextButtonAction(TextMeshProUGUI displayer, uint[] values)
            {
                if (displayer == null || values == null || values.Length == 0) return null;

                return () => {
                    index++;

                    if (index >= values.Length)
                    {
                        Debug.Log(CanWrapIndex);
                        if (CanWrapIndex) index = 0;
                        else index = values.Length - 1;
                    }

                    if(displayer != null) displayer.text = GetFramerateString(index);
                    SetFrameRateLimit(index);
                };
            }

            private List<UnityEngine.UI.Dropdown.OptionData> GetFramerateLimitsOptions(uint[] framerateValues)
            {
                List<UnityEngine.UI.Dropdown.OptionData> optionDatas = new List<UnityEngine.UI.Dropdown.OptionData>();
                if(framerateValues == null || framerateValues.Length == 0) return optionDatas;

                foreach (uint framerateLimit in framerateValues.Distinct().OrderBy(x => x))
                    optionDatas.Add(new UnityEngine.UI.Dropdown.OptionData(GetFramerateString(framerateLimit)));

                return optionDatas;
            }
            private List<TMP_Dropdown.OptionData> GetFramerateLimitsTMPOptions(uint[] framerateValues)
            {
                List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
                if (framerateValues == null || framerateValues.Length == 0) return optionDatas;

                foreach (uint framerateLimit in framerateValues.Distinct().OrderBy(x => x))
                    optionDatas.Add(new TMP_Dropdown.OptionData(GetFramerateString(framerateLimit)));

                return optionDatas;
            }
        }
}
