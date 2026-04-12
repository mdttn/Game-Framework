using RedSilver2.Framework.Inputs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;

    [Space]
    [SerializeField] private Button playButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button modesButton;

    [Space]
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button hintsButton;

    private GameModeMenu gameModeMenu;
    private CustomGameManager gameManager;

    private void Awake()
    {
        gameModeMenu = FindAnyObjectByType<GameModeMenu>();
        gameManager  = CustomGameManager.GetInstance();
    }

    private void Start()
    {
        InitializePlayButton();
       
        InitializeCreditsButton();
        InitializeGamemodeButton();
       
        InitializeOptionsButton();
        InitializeHintsButton();
    }

    private void InitializePlayButton()
    {
        UIHandler.InitializeButton(creditsButton, () => {
            CustomGameManager.GetInstance()?.GetSelectedGameMode();
        }, "PLAY");
    }

    private void InitializeCreditsButton()
    {
        UIHandler.InitializeButton(creditsButton, () =>
        {

        }, "CREDITS");
    }

    private void InitializeGamemodeButton()
    {
        UIHandler.InitializeButton(creditsButton, () =>
        {

        }, "MODES");
    }

    private void InitializeOptionsButton()
    {
        UIHandler.InitializeButton(optionsButton, () =>
        {

        }, "[OPTIONS]");
    }


    private void InitializeHintsButton()
    {
        UIHandler.InitializeButton(hintsButton, () => {

        }, "?");
    }


    private void SetPanelVisibility(bool isVisible)
    {
        mainPanel?.SetActive(isVisible);
    }

    private void OnEnable()
    {
        SetPanelVisibility(true);
    }

    private void OnDisable()
    {
        SetPanelVisibility(false);
    }
}
