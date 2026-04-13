using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameModeMenu : MonoBehaviour
{
    [SerializeField] private Transform buttonParent;
    [SerializeField] private Button    buttonPrefab;

    [Space]
    [SerializeField] private GameObject mainPanel;

    [Space]
    [SerializeField] private Button gameModeButton;
    [SerializeField] private Button backButton;

    private Button            selectedButton;
    private CustomGameManager gameManager;

    private Button[] buttons;

    private UnityEvent<Button> onSelected;
    private UnityEvent<Button> onDeselected;

    private void Awake()
    {
        onSelected   = new UnityEvent<Button>();
        onDeselected = new UnityEvent<Button>();

        gameManager  = CustomGameManager.GetInstance();
        InitializeButtons();
        InitializeBackButtonEvent();
    }

    private void Start() {
        if(gameManager != null)
            SelectButton(gameManager.GetSelectedGameModeIndex());

        enabled = false;
    }

    private void OnEnable() {
        SetMainPanelVisibility(true);
    }

    private void OnDisable() {
        SetMainPanelVisibility(false);
    }

    public void AddOnSelectedListener(UnityAction<Button> action)
    {
        if (action != null) onSelected?.AddListener(action);
    }

    public void RemoveOnSelectedListener(UnityAction<Button> action)
    {
        if (action != null) onSelected?.RemoveListener(action);
    }


    public void AddOnDeselectedListener(UnityAction<Button> action)
    {
        if (action != null) onDeselected?.AddListener(action);
    }

    public void RemoveOnDeselectedListener(UnityAction<Button> action)
    {
        if (action != null) onDeselected?.RemoveListener(action);
    }

    private void SetMainPanelVisibility(bool isVisible)
    {
        mainPanel?.SetActive(isVisible);
    }

    private void InitializeBackButtonEvent()
    {
        if (backButton == null) return;


    }


    private void InitializeButtons()
    {
        Gamemode[] gamemodes = CustomGameManager.GetGameModes();
        if (gamemodes == null || gamemodes.Length == 0) return;

        buttons = new Button[gamemodes.Length - 1];
        InitializeButtons(gamemodes);
    }

    private void InitializeButtons(Gamemode[] gamemodes)
    {
        if (gamemodes == null || gameManager == null || buttons == null || buttonPrefab == null)
            return;

        int unlockedIndex = gameManager.GetUnlockedGameModeIndex();

        for (int i = 0; i < gamemodes.Length; i++) {
            Button button = Instantiate(buttonPrefab);
            InitializeButton(button, gamemodes[i]);
            buttons[i] = button;

            if (i <= unlockedIndex) button.gameObject.SetActive(true);
            else                    button.gameObject.SetActive(false);
        }

        buttonPrefab.gameObject.SetActive(false);
    }

    private void InitializeButton(Button button, Gamemode gamemode) {
        if (button == null || gameManager == null || buttonParent == null)
            return;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(GetOnClickListener(button, gamemode));

        button.GetComponentInChildren<TextMeshProUGUI>().text = CustomGameManager.GetGameModeName(gamemode);
        button.transform.SetParent(buttonParent);
    }

    private UnityAction GetOnClickListener(Button button, Gamemode gamemode)
    {
        return () =>
        {
            gameManager?.SetGameMode(gamemode);
            SelectButton(button);
        };
    }

    private void SelectButton(int index)
    {
        if (buttons == null || index < 0 || index >= buttons.Length)
            return;

        SelectButton(buttons[index]);
    }

    private void SelectButton(Button button)
    {
        if (selectedButton == button) return;

        if(selectedButton != null) onDeselected?.Invoke(selectedButton);
        selectedButton = button;
        if (selectedButton != null) onSelected?.Invoke(selectedButton);
    }
}
