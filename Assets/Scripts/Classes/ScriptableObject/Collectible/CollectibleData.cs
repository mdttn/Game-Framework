using UnityEngine;

namespace RedSilver2.Framework.Interactions.Collectibles
{
    public abstract class CollectibleData : ScriptableObject
    {
        [SerializeField] private string collectibleName;
        [SerializeField][TextArea(3,3)] private string collectibleDescription;

        [Space]
        [Header("Notification Settings")]
        [SerializeField] private bool canTriggerInitialNotification;
        [SerializeField] private bool alwaysTriggerInitialNotification;

        [Space]
        [SerializeField] private Vector2 minModelRotation;
        [SerializeField] private Vector2 maxModelRotation;

        [Space]
        [SerializeField] private Sprite     icon;
        [SerializeField] private GameObject model;

        public string  CollectibleName                  => collectibleName; 
        public string  CollectibleDescription           => collectibleDescription;
        public bool    CanTriggerInitialNotification    => canTriggerInitialNotification;
        public bool    AlwaysTriggerInitialNotification => alwaysTriggerInitialNotification;

        public Vector2 MinModelRotation => minModelRotation;
        public Vector2 MaxModelRotation => maxModelRotation;
       
        public Sprite     Icon          => icon;
        public GameObject Model         => model;
    }
}
