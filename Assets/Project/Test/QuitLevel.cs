using RedSilver2.Framework;
using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Player;
using UnityEngine;

public class QuitLevel : MonoBehaviour
{
    private void Update()
    {
        if(InputManager.GetKeyDown(KeyboardKey.Escape) || InputManager.GetKeyDown(GamepadButton.Start)){
            GameManager.SceneLoaderManager?.LoadSingleScene(0);
            CameraController.SetCursorVisibility(true);
        }
    }
}
