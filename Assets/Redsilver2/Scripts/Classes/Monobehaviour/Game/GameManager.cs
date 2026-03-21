using RedSilver2.Framework.Interactions.Collectibles;
using RedSilver2.Framework.Performance.Lights;
using RedSilver2.Framework.Scenes;
using RedSilver2.Framework.Settings;
using RedSilver2.Framework.StateMachines.Controllers;
using RedSilver2.Framework.Subtitles;
using UnityEngine;

namespace RedSilver2.Framework
{
    [RequireComponent(typeof(SteamManager))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CollectibleNotificationManager collectibleNotification;
        [SerializeField] private SceneLoaderManager sceneLoaderManager;
        [SerializeField] private SubtitleManager subtitleManager;
        [SerializeField] private SettingManager settingManager;
        [SerializeField] private LightManager lightManager;

        protected static GameManager instance;

        public static CollectibleNotificationManager CollectibleNotification {
            get {
                GameManager manager = instance;
                if (manager != null) return manager.collectibleNotification;
                return null;
            }
        }

        public static SceneLoaderManager SceneLoaderManager {
            get {
                GameManager manager = instance;
                if (manager != null) return manager.sceneLoaderManager;
                return null;
            }
        }

        public static SubtitleManager SubtitleManager
        {
            get {
                GameManager manager = instance;
                if (manager != null) return manager.subtitleManager;
                return null;
            }
        }

        public static SettingManager SettingManager
        {
            get
            {
                GameManager manager = instance;
                if (manager != null) return manager.settingManager;
                return null;
            }
        }

        public static LightManager LightManager {
            get {
                GameManager manager = instance;
                if (manager != null) return manager.lightManager;
                return null;
            }
        }

        public const string GROUND_LAYER = "Ground";
        public const string PLAYER_LAYER = "Player";


        protected virtual void Awake()
        {
            if (instance != null) { Destroy(gameObject); return; }
            instance = this;
            DontDestroyOnLoad(instance);
            gameObject.name = "GameManager";
        }

        public static int GetGroundLayer() {
            return LayerMask.NameToLayer(GROUND_LAYER);
        }

        public static int GetPlayerLayer() {
            return LayerMask.NameToLayer(PLAYER_LAYER);
        }
    }
}
