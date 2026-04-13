using RedSilver2.Framework;
using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.Stats;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject      uiDisplayer;
    [SerializeField] private TextMeshProUGUI messageDisplayer;

    [Space]
    [SerializeField] private string[] gameOverTexts;
    [SerializeField] private string[] winGameTexts;

    private UnityEvent<bool> onGameResultShown;
    private bool isGameEnded;

    private void Awake()
    {
        isGameEnded = false;
        onGameResultShown = new UnityEvent<bool>();
    }

    private void Start()
    {
        SetPlayerHealthEvent();
        SetOrbCollectedEvent();

        uiDisplayer?.SetActive(false);
        if (messageDisplayer != null) messageDisplayer.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        var gameManager = CustomGameManager.GetInstance();
        gameManager?.RemoveOnOrbCollectedListener(OnOrbCollected);
    }

    private void SetPlayerHealthEvent()
    {
        if (PlayerController.Current != null)  {
            if (PlayerController.Current.TryGetComponent(out PlayerHealth health))  {
                health.AddOnProgressChangedListener(value => {
                    if (value <= 0) {
                        if (isGameEnded == true) return;
                        StartCoroutine(ApplyGameResults(gameOverTexts, false));
                        CameraController.SetCursorVisibility(true);
                    }
                });
            }
        }
    }

    private void SetOrbCollectedEvent() {
        var gameManager = CustomGameManager.GetInstance();
        gameManager?.AddOnOrbCollectedListener(OnOrbCollected);
    }

    private void OnOrbCollected(int count) {
        if (count == 0 && !isGameEnded) {
            StartCoroutine(ApplyGameResults(winGameTexts, true));
        }
    }

    private string GetRandomText(string[] texts)
    {
        if (texts == null || texts.Length == 0) return string.Empty;
        return texts[Random.Range(0, texts.Length)];
    }

    private IEnumerator ApplyGameResults(string[] texts, bool wonGame)
    {
        string text = GetRandomText(texts);
        isGameEnded = true;

        if (messageDisplayer != null) {
            messageDisplayer.text = "<color=" + (wonGame ? "white" : "red") + $">{text}</color>";
            messageDisplayer.gameObject.SetActive(true);
        }

        if (wonGame) CustomGameManager.GetInstance()?.UnlockNextGameMode();
        uiDisplayer?.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        GameManager.SceneLoaderManager?.LoadSingleScene(0);
    }

    public void AddOnGameResultShown(UnityAction<bool> action) {
        if (action != null) onGameResultShown?.AddListener(action);
    }

    public void RemoveOnGameResultShown(UnityAction<bool> action) {
        if (action != null) onGameResultShown?.AddListener(action);
    }
}
