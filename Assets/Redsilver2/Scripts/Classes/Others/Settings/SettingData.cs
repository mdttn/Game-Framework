using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Settings
{
    public partial class SettingManager : MonoBehaviour
    {
        [System.Serializable]
        public class SettingData {
            public int GraphicsQualityIndex  { get; set; }
            public int FramerateLimitIndex   { get; set; }
            public int ResolutionIndex       { get; set; }
            public int FullScreenModeIndex   { get; set; }
            public int ShadowQualityIndex    { get; set; }
            public int ShadowProjectionIndex { get; set; }
            public int ShadowResolutionIndex { get; set; }
        }

        #region Data
        private static readonly UnityEvent<SettingData> onLoadData = new UnityEvent<SettingData>();
        private SettingData data;
        public SettingData  Data => data;

        private const string DATA_FILE_NAME = "SettingData.json";

        public SettingData Load()
        {
            SettingData resultat;
            string settingDataPath = Path.Combine(Application.persistentDataPath, DATA_FILE_NAME);

            if (!File.Exists(settingDataPath)) resultat = new SettingData();
            else resultat = JsonUtility.FromJson<SettingData>(File.ReadAllText(settingDataPath));

            if (onLoadData != null) onLoadData.Invoke(resultat);
            return resultat;
        }

        public void Save()
        {
            string settingDataPath = Path.Combine(Application.persistentDataPath, "SettingData.json");
            File.WriteAllText(settingDataPath, JsonUtility.ToJson(Data));
        }


        #endregion
    }
}
