using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Radar : MonoBehaviour {

    [SerializeField] private Camera _camera;

    [Space]
    [SerializeField] private Image  backgroundTop;
    [SerializeField] private Image  backgroundBottom;

    [Space]
    [SerializeField] private float height;

    private SingleInput  interactInput;
    private Vector2Input lookInput;

    private uint radarUses;
    private bool isActivated;

    private bool        isWaitDelayFinished;
    private IEnumerator openCoroutine;

    private Camera[] cameras;


    private void Awake() {
        radarUses = 3;
        cameras = Camera.allCameras.Where(x => x != _camera).ToArray();

        SetCameraVisibility(false);
        SetBackgroundTopVisibility(false);
        SetBackgroundBottomVisibility(false);

        interactInput = InputManager.GetOrCreatePressInput("RADAR_INTERACT_INPUT", KeyboardKey.R, GamepadButton.ButtonNorth);
        lookInput     = InputManager.GetOrCreateMouseVector2Input("RADAR_LOOK_INPUT", GamepadStick.RightStick);

        interactInput?.Enable();
        isWaitDelayFinished = true;
    }

    private void Update()
    {
        UpdateRadarInput();
    }

    private void UpdateRadarInput()
    {
        if (interactInput.Value) {
            if ((radarUses == 0 && !isActivated) || !isWaitDelayFinished) {
                return;
            }

            SetIsActivated(!isActivated, 0.5f);
            if (isActivated) radarUses--;
        }
    }

    private void SetIsActivated(bool isActivated, float waitDelay)
    {    
        if (isActivated) {
            Enable();
        }
        else {
            Disable(waitDelay);
        }
    }

    private void Enable()
    {
        if (!isWaitDelayFinished || isActivated) return;
        isActivated = true;

        CameraControllerModule.Disable();
        PlayerController.Disable();

        PlayerController controller = PlayerController.Current;

        SetCameraPosition(controller);
        SetCameraRotation(controller);


        openCoroutine = OpenCoroutine();
        StartCoroutine(openCoroutine);
    }

    public void Disable(float waitDelay) {
        if (!isWaitDelayFinished || !isActivated) return;
        isActivated = false;
       
        lookInput?.Disable();
        StopCoroutine(openCoroutine);
       
        openCoroutine = null;
        StartCoroutine(WaitDelayCoroutine(waitDelay));
       
        SetBackgroundTopVisibility(false);
        SetBackgroundBottomVisibility(false);
        
        SetCameraVisibility(false);

        CameraControllerModule.Enable();
        PlayerController.Enable();
    }

    private void SetCameraPosition(PlayerController controller)
    {
        if (_camera == null || controller == null) return;
        Vector3 playerPosition = controller.transform.position;

        _camera.transform.position = Vector3.right   * playerPosition.x +
                             Vector3.up      * (playerPosition.y + height) +
                             Vector3.forward * playerPosition.z;
    }

    private void SetCameraRotation(PlayerController controller)
    {
        if (_camera == null || controller == null) return;
        Transform transform = _camera.transform;

        transform.eulerAngles = Vector3.right * 90f + Vector3.up * controller.transform.eulerAngles.y;
    }

    private void SetCameraFieldOfView(float fieldOfView)
    {
        if (_camera != null) _camera.fieldOfView = fieldOfView;
    }
    private void SetBackgroundFillValue(Image background, float value)
    {
        value = Mathf.Clamp01(value);
        if (background != null) background.fillAmount = value;
    }


    private void SetBackgroundTopFillValue(float value)
    {
        SetBackgroundFillValue(backgroundTop, value);
    }

    private void SetBackgroundBottomFillValue(float value)
    {
        SetBackgroundFillValue(backgroundBottom, value);
    }

    private void SetBackgroundVisibility(Image background, bool isVisible)
    {
        if (background != null)
            background.gameObject.SetActive(isVisible);
    }


    private void SetBackgroundTopVisibility(bool isVisible)
    {
        SetBackgroundVisibility(backgroundTop, isVisible);
    }

    private void SetBackgroundBottomVisibility(bool isVisible)
    {
        SetBackgroundVisibility(backgroundBottom, isVisible);
    }




    private void SetCameraVisibility(bool isVisible)
    {
        if (_camera != null) _camera.enabled = isVisible;

        foreach(Camera camera in cameras.Where(x => x != null)) {
            if(camera == null) continue;
            camera.enabled = !isVisible;
        }
    }

    private IEnumerator WaitDelayCoroutine(float seconds)
    {
        if (seconds > 0f) {
            isWaitDelayFinished = false;
            yield return StartCoroutine(ProgressiveCoroutine(seconds, null));
        }

        isWaitDelayFinished = true;
    }

    private IEnumerator OpenCoroutine()
    {
        yield return StartCoroutine(BackgroundCoroutine(() =>
        {
            SetCameraFieldOfView(10f);  
            SetCameraVisibility(true);
           
            lookInput?.Enable();
            StartCoroutine(UpdateInput());
        }));

        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(ProgressiveCoroutine(0.15f, value => { SetCameraFieldOfView(Mathf.Lerp(10f, 70f, value)); }));
    }

    private IEnumerator BackgroundCoroutine(UnityAction onFinished)
    {
        SetBackgroundTopFillValue(0f);
        SetBackgroundBottomFillValue(0f);

        SetBackgroundTopVisibility(true);
        SetBackgroundBottomVisibility(true);

        yield return StartCoroutine(ProgressiveCoroutine(0.1f, value => { 
            SetBackgroundTopFillValue(value);
            SetBackgroundBottomFillValue(value);
        }));

        yield return new WaitForSeconds(0.35f);
        onFinished?.Invoke();

        SetBackgroundTopVisibility(false);
        SetBackgroundBottomVisibility(false);
    }

    private IEnumerator UpdateInput()
    {
        while (lookInput != null)
        {
            if (!lookInput.IsEnabled) break;
            transform.localEulerAngles += (Time.deltaTime * lookInput.Value.x * 5f) *  Vector3.up;

            yield return null;
        }
    }

    private IEnumerator ProgressiveCoroutine(float transitionTime, UnityAction<float> action)
    {
        float t = 0f;

        action?.Invoke(0f);

        while (t < transitionTime){
            t += Time.deltaTime;
            action?.Invoke(t / transitionTime);
            yield return null;
        }

        action?.Invoke(1f);
    }
   


}
