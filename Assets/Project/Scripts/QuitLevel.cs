using RedSilver2.Framework;
using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

public class QuitLevel : MonoBehaviour
{
    private void Update()
    {
        if(InputManager.GetKeyDown(KeyboardKey.Escape) || InputManager.GetKeyDown(GamepadButton.Start)){
            PlayerController.Disable();
            CameraController.Disable();
            GameManager.SceneLoaderManager?.LoadSingleScene(0);
        }
    }
}
