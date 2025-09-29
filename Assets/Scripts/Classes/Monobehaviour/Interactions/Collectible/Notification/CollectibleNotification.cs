using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Collectibles
{
    public abstract class CollectibleNotification : MonoBehaviour
    {
        private UnityEvent<Collectible> onNotificationShown;
        private UnityEvent<Collectible> onNotificationHid;

        private static List<string> registeredCollectibles = new List<string>();

        protected virtual void Awake()
        {
            onNotificationHid   = new UnityEvent<Collectible>();
            onNotificationShown = new UnityEvent<Collectible>();

            onNotificationShown.AddListener(OnNotificationShown);
            onNotificationHid.AddListener(OnNotificationHid);
        }

        public void AddOnNotificationShownListener(UnityAction<Collectible> action)
        {
            if (onNotificationShown != null && action != null) { onNotificationShown.AddListener(action); }
        }
        public void RemoveOnNotificationShownListener(UnityAction<Collectible> action)
        {
            if (onNotificationShown != null && action != null) { onNotificationShown.RemoveListener(action); }
        }

        public void AddOnNotificationHidListener(UnityAction<Collectible> action)
        {
            if (onNotificationHid != null && action != null) { onNotificationHid.AddListener(action); }
        }
        public void RemoveOnNotificationHidListener(UnityAction<Collectible> action)
        {
            if (onNotificationHid != null && action != null) { onNotificationHid.RemoveListener(action); }
        }

        public static bool WasDataRegistered(CollectibleData data)
        {
            GameObject model = GetDataModel(data);
            if (model == null || registeredCollectibles == null) return false;
            return registeredCollectibles.Contains(model.name.ToLower());
        }

        public static void RegisterData(CollectibleData data)
        {
            GameObject model = GetDataModel(data);
          
            if (model != null && registeredCollectibles != null) 
            {
                if (!WasDataRegistered(data))
                {
                    registeredCollectibles.Add(model.name.ToLower());
                }
            }    
        }

        protected static GameObject GetDataModel(CollectibleData data)
        {
            GameObject model;
            if (data == null) return null;

            model = data.Model;
          
            if (model == null) return null;
            return CollectibleModelViewer.GetCollectibleModel(model.name);
        }

        protected virtual void OnNotificationShown(Collectible collectible)
        {
           if (collectible != null) RegisterData(collectible.GetData());
        }

        protected abstract void OnNotificationHid(Collectible collectible);

        public IEnumerator UpdateNotification(Collectible collectible)
        {
            onNotificationShown.Invoke(collectible);
          
            yield return StartCoroutine(DisplayNotification(collectible));
            
            onNotificationHid.Invoke(collectible);
        }

        protected abstract IEnumerator DisplayNotification(Collectible collectible);
    }
}