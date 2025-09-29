using RedSilver2.Framework.Achievements;
using RedSilver2.Framework.Interactions.Collectibles;
using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework
{
    [RequireComponent(typeof(AchievementManager))]
    public sealed class GameManager : MonoBehaviour
    {
        public const string GROUND_LAYER_NAME = "Ground";

        private AchievementManager achievementManager;
        private CollectibleNotificationManager collectibleNotificationManager;
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(Instance);

            achievementManager             = GetComponent<AchievementManager>();
            collectibleNotificationManager = FindAnyObjectByType<CollectibleNotificationManager>();
        }

        private void Start()
        {

        }

        public static CollectibleNotificationManager GetCollectibleNotificationManager()
        {
            GameManager instance = Instance;
            if (instance != null) return instance.collectibleNotificationManager;
            return null;
        }

        public static AchievementManager GetAchievementManager()
        {
            GameManager instance = Instance;
            if (instance != null) return instance.achievementManager;
            return null;
        }
    }
}
