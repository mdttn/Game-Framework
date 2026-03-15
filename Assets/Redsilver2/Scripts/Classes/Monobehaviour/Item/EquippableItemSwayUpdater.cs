using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions.Items;
using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.StateMachines.States.Movement;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEngine.Rendering.DebugUI;

namespace RedSilver2.Framework.Items
{
    public class EquippableItemSwayUpdater : MonoBehaviour {

        [SerializeField] private Vector2 minPosition;
        [SerializeField] private Vector2 maxPosition;

        [Space]
        [SerializeField] private Vector2 minRotation;
        [SerializeField] private Vector2 maxRotation;

        [Space]
        [SerializeField] private float positionUpdateSpeed;

        [Space]
        [SerializeField] private float inputRotationStrenght;
        [SerializeField] private float rotationUpdateSpeed;

        private EquippableItem item;

        private PlayerCameraController cameraController;
        private PlayerMovementHandler  handler;

        private Vector3 originalPosition;
        private Vector3 originalRotation;

        private Vector2 desiredPosition;
        private Vector2 desiredRotation;

        private void Start()
        {
            item = GetItem();
            EnableEvents();
        }

        private void OnEnable()
        {
            EnableEvents();
        }

        private void OnDisable()
        {
            if(item != null)
            {
                item.transform.localPosition    = originalPosition;
                item.transform.localEulerAngles = originalRotation;
            }

            DisableEvents();
        }

        private void EnableEvents()
        {
            item?.AddOnEquippedListener(OnEquipped);
            item?.AddOnUpdateListener(OnUpdate);
            item?.AddOnLateUpdateListener(OnLateUpdate);
        }

        private void DisableEvents()
        {
            item?.RemoveOnEquippedListener(OnEquipped);
            item?.RemoveOnUpdateListener(OnUpdate);
            item?.RemoveOnLateUpdateListener(OnLateUpdate);
        }

        protected virtual void OnEquipped() {
            if(item == null) return;
            originalPosition = item.ParentPosition;
            originalRotation = item.ParentRotation;

            desiredPosition = originalPosition;
            desiredRotation = originalRotation;

            cameraController = GetCameraController();
            handler          = GetMovementHandler();

        }

        private void OnUpdate()
        {
            if (item == null) return;
            UpdateRotation();
            UpdatePosition();
        }

        private void OnLateUpdate()
        {
            if (item == null) return;
            Transform transform = item.transform;
            Vector3 newPosition = Vector3.right * desiredPosition.x + Vector3.up * desiredPosition.y + Vector3.forward * originalPosition.z;
            Vector3 newRotation = Vector3.right * desiredRotation.x + Vector3.up * desiredRotation.y + Vector3.forward * originalRotation.z;

            transform.localPosition    = newPosition;
            transform.localEulerAngles =  newRotation;
        }

        private PlayerCameraController GetCameraController()
        {
            if (item == null || item.transform.root == null) return null;
            CameraControllerModule module  = item.transform.root.GetComponentInChildren<CameraControllerModule>();
           
            if (module == null) return null;
            
            if(module.Controller is PlayerCameraController) {
                return module.Controller as PlayerCameraController;
            }

            return null;
        }

        private PlayerMovementHandler GetMovementHandler()
        {
            if (item == null || item.transform.root == null) return null;

            if(item.transform.root.TryGetComponent(out MovementStateMachineController controller))
                return controller.MovementHandler as PlayerMovementHandler;    

            return null;
        }
        
        private EquippableItem GetItem()
        {
            if (transform.root == null) return transform.GetComponentInChildren<EquippableItem>();
            return transform.root.GetComponentInChildren<EquippableItem>();
        }

        private void UpdateRotation() {
            if(cameraController == null) return;
       
            float x = 0f, y = 0f;
            UpdateRotation(cameraController.MouseInput, ref x, ref y);

            float newPositionX = Mathf.Lerp(desiredRotation.x, x, Time.deltaTime * rotationUpdateSpeed);
            float newPositionY = Mathf.Lerp(desiredRotation.y, y, Time.deltaTime * rotationUpdateSpeed);

            desiredRotation = Vector2.right * newPositionX + Vector2.up * newPositionY;
        }

        private void UpdateRotation(MouseVector2Input input, ref float x, ref float y)
        {
            if (input == null || input.Value.magnitude <= 0f) {
                x = originalRotation.x;
                y = originalRotation.y;
            }
            else {
                Vector2 value = input.Value;
                x = Mathf.Clamp(desiredRotation.x - value.y * inputRotationStrenght, minRotation.x, maxRotation.x);
                y = Mathf.Clamp(desiredRotation.y + value.x * inputRotationStrenght, minRotation.y, maxRotation.y); 
            }
        }

        private void UpdatePosition() {
            float x = originalPosition.x, y = originalPosition.y;
           
            if (handler != null && item != null)  {
                KeyboardVector2Input moveInput = handler.MoveInput;
                if (moveInput == null) return;

                if (moveInput.Value.magnitude > 0 && item.IsEquipped)
                {
                    float sin = Mathf.Abs(Mathf.Sin(Time.time * positionUpdateSpeed));
                    x = Mathf.Lerp(minPosition.x, maxPosition.x, sin);
                    y = Mathf.Lerp(minPosition.y, maxPosition.y, sin);
                }
            }

            float newPositionX = Mathf.Lerp(desiredPosition.x, x, Time.deltaTime);
            float newPositionY = Mathf.Lerp(desiredPosition.y, y, Time.deltaTime);


            desiredPosition = Vector2.right * newPositionX + Vector2.up * newPositionY;
        }
    }
}
