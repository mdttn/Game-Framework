using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Settings
{
    public abstract class Setting
    {
        private SettingManager settingManager;
        private bool saveAutomatically, canWrapIndex;

        protected SettingManager SettingManager => settingManager;

        public bool CanWrapIndex
        {
            get { return canWrapIndex; }

            set {
                this.canWrapIndex = value;
            }
        }

        public bool SaveAutomatically
        {
            get { return saveAutomatically; }

            set {
                this.saveAutomatically = value;
            }
        }

        protected Setting()
        {
            SettingManager.AddOnAwakeHookListener(settingManager => { this.settingManager = settingManager; });
        }          
    }
}
