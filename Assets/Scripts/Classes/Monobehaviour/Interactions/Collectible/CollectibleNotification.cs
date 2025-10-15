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

        private static List<string> registeredCollectible = new List<string>();

        protected virtual void Awake()
        {
            onNotificationHid = new UnityEvent<Collectible>();
            onNotificationShown = new UnityEvent<Collectible>();

            onNotificationShown.AddListener(OnNotificationShown);
            onNotificationHid.AddListener(OnNotificationHid);
        }

        public static bool WasDataRegistered(CollectibleData data)
        {
            GameObject model = GetDataModel(data);
            if (model == null) return false;
            return registeredCollectible.Contains(model.name.ToLower());
        }

        public static void RegisterData(CollectibleData data)
        {
            GameObject model = GetDataModel(data);
          
            if (model != null && registeredCollectible != null) 
            {
                if (!WasDataRegistered(data))
                {
                    registeredCollectible.Contains(model.name.ToLower());
                }
            }    
        }

        protected static GameObject GetDataModel(CollectibleData data)
        {
            if (data == null || data.Model == null) return null;
            return CollectibleModelViewer.GetCollectibleModel(data.Model.name);
        }

        protected abstract void OnNotificationShown(Collectible collectible);
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