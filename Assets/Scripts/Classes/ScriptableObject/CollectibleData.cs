using UnityEngine;

namespace RedSilver2.Framework.Interactions.Collectibles
{
    public abstract class CollectibleData : ScriptableObject
    {
        [SerializeField] private string collectibleName;
        [SerializeField][TextArea(3,3)] private string collectibleDescription;

        [Space]
        [SerializeField] private Sprite     icon;
        [SerializeField] private GameObject model;

        [Space]
        [Header("Notification Settings")]
        [SerializeField] private bool canTriggerInitialNotification;
        [SerializeField] private bool alwaysTriggerInitialNotification;


        public string  CollectibleName                  => collectibleName; 
        public string  CollectibleDescription           => collectibleDescription;
      
        public bool    CanTriggerInitialNotification    => canTriggerInitialNotification;
        public bool    AlwaysTriggerInitialNotification => alwaysTriggerInitialNotification;
      
        public Sprite     Icon          => icon;
        public GameObject Model         => model;
    }
}
