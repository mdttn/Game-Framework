using RedSilver2.Framework.Inputs;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;

    [Space]
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private bool isUIOpened;
  
    private void Awake() {
        isUIOpened = false;
        SetMainPanelVisibility(false);    

        UIHandler.InitializeButton(yesButton, () => {
            Application.Quit(); 
        }, "YES");

        UIHandler.InitializeButton(noButton, () => {
            SetMainPanelVisibility(false);
            isUIOpened = false;
        }, "NO");
    }

    void Update()
    {
        if (!isUIOpened) {
            UpdateInput();
        }
    }

    private void UpdateInput()
    {
        if (InputManager.GetKeyDown(KeyboardKey.Escape) || InputManager.GetKeyDown(GamepadButton.Start)) {
            SetMainPanelVisibility(true);
            isUIOpened = true;
        }
    }

    private void SetMainPanelVisibility(bool isVisible) {
        mainPanel?.SetActive(isVisible);    
    }
}
