using UnityEngine;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private QuitGame quitGame;

    private void Start()
    {
        UIHandler.InitializeButton(playButton, () =>
        {
            if (quitGame != null) quitGame.enabled = false;
            CustomGameManager.GetInstance()?.LoadGameMode();
        }, "PLAY");
    }
}
