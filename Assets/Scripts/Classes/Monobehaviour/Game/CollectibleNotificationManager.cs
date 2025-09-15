using RedSilver2.Framework.Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


        private InitialCollectibleNotification initialCollectibleNotification;

        private Queue<CollectibleNotification> notifications;
        private Queue<Collectible>             showcases;

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

            initialCollectibleNotification = FindAnyObjectByType<InitialCollectibleNotification>();
            InitializeNotifications(notificationPrefab, notificationParent, maxNotificationPrefabs);
        }

        private void InitializeNotifications(CollectibleNotification notification, Transform parent, uint amount)
        {
            if(!(notification is InitialCollectibleNotification))
             for (uint i = 0; i < amount; i++) InitializeNotifications(notification, parent);
        }

        private void InitializeNotifications(CollectibleNotification notification, Transform parent)
        {
            if (notification != null && parent != null)
                AddNotification(Instantiate(notification, parent));
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
            if (collectible != null)
            {
                if (CanTriggerInitialNotification(collectible.GetData()))
                     yield return StartCoroutine(TriggerIntialNotification(collectible));
                else
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
            if (initialCollectibleNotification != null && collectible != null)
            {
                yield return StartCoroutine(initialCollectibleNotification.UpdateNotification(collectible));
            }
        }
    }
}
