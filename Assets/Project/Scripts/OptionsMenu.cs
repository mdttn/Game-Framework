using Mirror.BouncyCastle.Tsp;
using RedSilver2.Framework.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Resolution = UnityEngine.Resolution;

public class OptionsMenu : MonoBehaviour {
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject menuPanel;

    [Space]
    [SerializeField] private QuitGame quitGame;

    [Space]
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button backButton;

    [Space]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private const string RESOLUTION_KEY_NAME = "RESOLUTION KEY";

    [Space]
    [SerializeField] private TMP_Dropdown fullscreenModeDropdown;
    private const string FULLSCREEN_MODE_KEY_NAME = "FULLSCREEN MODE KEY";

    [Space]
    [SerializeField] private Slider sensitivityXSlider;
    [SerializeField] private TextMeshProUGUI sensitivityXDisplayer;
    private const string SENSITIVITY_X_KEY_NAME = " SENSITIVITY X KEY";

    [Space]
    [SerializeField] private Slider sensitivityYSlider;
    [SerializeField] private TextMeshProUGUI sensitivityYDisplayer;
    private const string SENSITIVITY_Y_KEY_NAME = " SENSITIVITY Y KEY";

    [Space]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeDisplayer;
    private const string VOLUME_KEY_NAME = "VOLUME KEY";

    private bool isUIOpen = false;


    private void Start()
    {
        InitializeResoltuionDropdown();
        InitializeFullScreenModeDropdown();
        InitializeVolumeSlider();

        InitializeSensitivitySlider(sensitivityXSlider, sensitivityXDisplayer, SENSITIVITY_X_KEY_NAME, 1f, 100f);
        InitializeSensitivitySlider(sensitivityYSlider, sensitivityYDisplayer, SENSITIVITY_Y_KEY_NAME, 1f, 100f);

        UIHandler.InitializeButton(optionsButton, () =>
        {
            if (isUIOpen) return;
            if (quitGame != null) quitGame.enabled = false;

            menuPanel?.SetActive(false);
            mainPanel?.SetActive(true);

            isUIOpen = true;
        }, "OPTIONS");

        UIHandler.InitializeButton(backButton, () => {
            if (!isUIOpen) return;                      
            mainPanel?.SetActive(false);

            menuPanel?.SetActive(true);
            if (quitGame != null) quitGame.enabled = true;

            isUIOpen = false;
        }, "BACK");

        mainPanel?.SetActive(false);
    }

    private void InitializeResoltuionDropdown()
    {
        Resolution[] resolutions = Screen.resolutions.Distinct().Reverse().ToArray();

        UIHandler.InitializeDropdown(resolutionDropdown, value => {
            Resolution resolution = resolutions[value];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);

            PlayerPrefs.SetInt(RESOLUTION_KEY_NAME, value);
            PlayerPrefs.Save();
        }, GetStringResolutions(resolutions), PlayerPrefs.GetInt(RESOLUTION_KEY_NAME, 0));

        var res = resolutions[Mathf.Clamp(PlayerPrefs.GetInt(RESOLUTION_KEY_NAME, 0), 0, resolutions.Length - 1)];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);

    }

    private void InitializeFullScreenModeDropdown()
    {
        FullScreenMode[] fullScreenModes = (FullScreenMode[])Enum.GetValues(typeof(FullScreenMode));

        UIHandler.InitializeDropdown(fullscreenModeDropdown, value => {

            Screen.fullScreenMode = (FullScreenMode)value;
            PlayerPrefs.SetInt(FULLSCREEN_MODE_KEY_NAME, value);
            PlayerPrefs.Save();
        }, GetStringResolutions(fullScreenModes), PlayerPrefs.GetInt(FULLSCREEN_MODE_KEY_NAME, 0));

        Screen.fullScreenMode = (FullScreenMode)PlayerPrefs.GetInt(FULLSCREEN_MODE_KEY_NAME, 0);
    }

    private void InitializeSensitivitySlider(Slider slider, TextMeshProUGUI displayer, string keyName, float minValue, float maxValue)
    {
        if (slider == null || displayer == null) return;
        slider.onValueChanged.RemoveAllListeners();


        slider.onValueChanged.AddListener(value => {
            if(displayer != null) displayer.text = value.ToString();
            PlayerPrefs.SetFloat(keyName, value);
            PlayerPrefs.Save();
        });

        slider.wholeNumbers = true;
        slider.minValue = minValue;
        
        slider.maxValue = maxValue;
        slider.value = PlayerPrefs.HasKey(keyName) ? PlayerPrefs.GetFloat(keyName) : 50f;
    }

    private void InitializeVolumeSlider()
    {
        if (volumeSlider == null || volumeDisplayer == null) return;
        volumeSlider.onValueChanged.RemoveAllListeners();


        volumeSlider.onValueChanged.AddListener(value => {
            if (volumeDisplayer != null) volumeDisplayer.text = value.ToString();
            AudioListener.volume = Mathf.Clamp01(value / volumeSlider.maxValue);
            PlayerPrefs.SetFloat(VOLUME_KEY_NAME, AudioListener.volume);
            PlayerPrefs.Save();
        });

        volumeSlider.wholeNumbers = true;
        volumeSlider.minValue = 0f;
       
        volumeSlider.maxValue = 100f;
        volumeSlider.value = PlayerPrefs.HasKey(VOLUME_KEY_NAME) ? PlayerPrefs.GetFloat(VOLUME_KEY_NAME) : 50f;
    }

    private string[] GetStringResolutions(Resolution[] resolutions)
    {
        List<string> results = new List<string>();
        if(resolutions == null) return results.ToArray();   

        foreach(Resolution resolution in resolutions) 
           results.Add($"{resolution.width}x{resolution.height}");

        return results.ToArray();
    }

    private string[] GetStringResolutions(FullScreenMode[] fullScreenModes)
    {
        List<string> results = new List<string>();
        if (fullScreenModes == null) return results.ToArray();

        foreach (FullScreenMode fullScreenMode in fullScreenModes)
            results.Add($"{fullScreenMode.ToString()}");

        return results.ToArray();
    }

    public static float GetSensitivityX()
    {
        return PlayerPrefs.HasKey(SENSITIVITY_X_KEY_NAME) ? PlayerPrefs.GetFloat(SENSITIVITY_X_KEY_NAME) : 50f;
    }

    public static float GetSensitivityY(){
        return PlayerPrefs.HasKey(SENSITIVITY_Y_KEY_NAME) ? PlayerPrefs.GetFloat(SENSITIVITY_Y_KEY_NAME) : 50f;
    }
}
