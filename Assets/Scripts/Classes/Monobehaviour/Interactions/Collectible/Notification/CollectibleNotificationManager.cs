using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RedSilver2.Framework.Interactions.Collectibles
{
    public sealed class CollectibleNotificationManager : MonoBehaviour
    {
        [SerializeField]                private Transform               notificationParent;
        [SerializeField]                private CollectibleNotification notificationPrefab;

        [Space]
        [SerializeField][Range(1, 100)] private uint maxNotificationPrefabs = 99;

        [Space]
        [SerializeField] private KeyboardKey keyboardKey;
        [SerializeField] private GamepadKey  gamepadKey;

        [Space]
        [SerializeField] private bool canPressAnyKey;

        [Space]
        [SerializeField] private Collectible[] collectibles;


        private InitialCollectibleNotification initialNotification;

        private Queue<CollectibleNotification> notifications;
        private Queue<Collectible>             showcases;

        public InitialCollectibleNotification InitialNotification => initialNotification;



        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (notificationPrefab is InitialCollectibleNotification)
                notificationPrefab = null;
        }
        #endif


        private void Awake()
        {
            notifications          = new Queue<CollectibleNotification>();
            showcases              = new Queue<Collectible>();

            initialNotification = FindAnyObjectByType<InitialCollectibleNotification>();
            InitializeNotifications(notificationPrefab, notificationParent, maxNotificationPrefabs);
        }


        private void Start()
        {
            SetInitialNotificationEvents();

            foreach (var collectible in collectibles)
                if (collectible != null) collectible.Interact();
        }

        private void InitializeNotifications(CollectibleNotification notification, Transform parent, uint amount)
        {
            if(notification != null)
             for (uint i = 0; i < amount; i++) InitializeNotifications(notification, parent);
        }

        private void InitializeNotifications(CollectibleNotification notification, Transform parent)
        {
            if (notification != null && parent != null)
                AddNotification(Instantiate(notification, parent));
        }

        private void SetInitialNotificationEvents()
        {
            if (initialNotification != null)
            {
                initialNotification.AddOnNotificationShownListener(OnInitialNotificationShown);
                initialNotification.AddOnNotificationHidListener(OnInitialNotificationHid);
            }
        }

        private void OnInitialNotificationShown(Collectible collectible)
        {
           if(collectible != null) SetPlayerEnableState(false);
        }
        private void OnInitialNotificationHid(Collectible collectible)
        {
           if(collectible != null) SetPlayerEnableState(true);
        }

        private void SetPlayerEnableState(bool isEnabled)
        {
            PlayerController       current = PlayerController.Current;
            CameraControllerModule camera = CameraControllerModule.Current;

            if (current != null) current.enabled = isEnabled;
            if (camera  != null) camera.enabled = isEnabled;

            Debug.Log(camera);
        }

        public void Notify(Collectible collectible)
        {
            if (collectible != null && showcases != null)
            {
                showcases.Enqueue(collectible);
                if (showcases.Count == 1) StartCoroutine(UpdateNotifications());
            }
        }

        public bool CanCloseIntialNotification() 
        {
            if (canPressAnyKey) return InputManager.AnyKeyDown;
            return InputManager.GetKeyDown(keyboardKey, gamepadKey);
        }
        public bool CanCloseIntialNotification(bool isClosingNotification)
        {
            return isClosingNotification || CanCloseIntialNotification();
        }

        private void AddNotification(CollectibleNotification notification)
        {
            if(notification != null && this.notifications != null)
            {
                notification.gameObject.SetActive(false);
                this.notifications.Enqueue(notification);
            }
        }

        private bool CanTriggerInitialNotification(CollectibleData data)
        {
            if(data == null) return false;

            if (data.CanTriggerInitialNotification)
            {
                if(data.AlwaysTriggerInitialNotification) return true;
                return !CollectibleNotification.WasDataRegistered(data);
            }

            return false;
        }

        public IEnumerator CloseInitialNotification()
        {
            while (true)
            {
                if (CanCloseIntialNotification()) break;
                yield return null;
            }
        }

        public IEnumerator CloseInitialNotification(Button button)
        {
            bool canCloseNotification = false;
            if (button != null) button.onClick.AddListener(() => canCloseNotification = true);

            while (true)
            {
                if (CanCloseIntialNotification(canCloseNotification)) break;
                yield return null;
            }

            if (button != null) button.onClick.RemoveAllListeners();
        }

        private IEnumerator UpdateNotifications()
        {
            if (showcases != null)
            {
                while (showcases.Count > 0)
                {
                    yield return StartCoroutine(UpdateNotification(showcases.Dequeue()));
                    yield return null;
                }
            }
        }

        private IEnumerator UpdateNotification(Collectible collectible)
        {
            if (collectible != null && notifications != null)
            {
                if (CanTriggerInitialNotification(collectible.GetData()))
                     yield return StartCoroutine(TriggerIntialNotification(collectible));
                else if(notifications.Count > 0)
                     yield return StartCoroutine(TriggerNotification(notifications.Dequeue(), collectible));
            }
        }

        private IEnumerator TriggerNotification(CollectibleNotification notification, Collectible collectible)
        {
            if (notification != null && collectible != null)
            {
                yield return StartCoroutine(notification.UpdateNotification(collectible));
                notifications.Enqueue(notification);
            }
        }

        private IEnumerator TriggerIntialNotification(Collectible collectible)
        {
            if (initialNotification != null && collectible != null)
                yield return StartCoroutine(initialNotification.UpdateNotification(collectible));
        }
    }
}
