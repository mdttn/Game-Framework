using RedSilver2.Framework.Interactions.Collectibles;
using UnityEngine;

namespace RedSilver2.Framework
{
    public sealed class GameManager : MonoBehaviour
    {
        public const string GROUND_LAYER_NAME = "Ground";

        private CollectibleNotificationManager collectibleNotification;

        public CollectibleNotificationManager CollectibleNotification => collectibleNotification;
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(Instance);

            collectibleNotification = FindAnyObjectByType<CollectibleNotificationManager>();
        }

        private void Start()
        {

        }

        private void Update()
        {

        }
    }
}
