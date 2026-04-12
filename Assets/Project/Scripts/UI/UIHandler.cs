using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public static class UIHandler 
{
    public static void InitializeButton(Button button, UnityAction onClick, string name)
    {
        if (button == null || onClick == null || string.IsNullOrEmpty(name)) return;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(onClick);

        TextMeshProUGUI displayer = button.GetComponentInChildren<TextMeshProUGUI>();   
        if(displayer != null) displayer.text = name.ToUpper();
    }

    public static void InitializeDropdown(TMP_Dropdown dropdown, UnityAction<int> onValueChanged, string[] values, int defaultValue) {
        if (dropdown == null || values == null) return;

        dropdown.ClearOptions();
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener(onValueChanged);

        foreach (string value in values) dropdown.options.Add(new TMP_Dropdown.OptionData(value));
        dropdown.value = defaultValue;
    }
}
