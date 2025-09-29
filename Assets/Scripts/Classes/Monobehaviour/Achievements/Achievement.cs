using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Achievements
{
    [System.Serializable]
    public class Achievement
    {
        public readonly uint   Index;
        public readonly string Name;
        public readonly string Description;
   
        private bool unlocked;
        private UnityEvent onAchievementUnlocked;

        public bool Unlocked => unlocked;



        public Achievement(uint achievementIndex)
        {
            this.unlocked              = false;
            this.Index                 = achievementIndex;

            this.Description           = string.Empty;

            this.Name                  = AchievementManager.GetAchievementName(achievementIndex);
            this.onAchievementUnlocked = new UnityEvent();

            AchievementManager.AddAchievement(this);
        }

        public Achievement(uint achievementIndex, string achievementDescription)
        {
            this.unlocked              = false;
            this.Index                 = achievementIndex;

            this.Description           = achievementDescription;

            this.Name                  = AchievementManager.GetAchievementName(achievementIndex);
            this.onAchievementUnlocked = new UnityEvent();

            AchievementManager.AddAchievement(this);
        }

        public void Unlock()
        {
            if (IsUnlockRequirementsValid())
            {
                UnlockSteamAchievement();
                unlocked = true;
            }
        }

        private bool IsSteamAchievementUnlocked()
        {
            bool isUnlocked = true;

            if (SteamManager.Initialized)
                SteamUserStats.GetAchievement(Name, out isUnlocked);
     
            return isUnlocked;
        }

        private void UnlockSteamAchievement()
        {
            if (SteamManager.Initialized && !IsSteamAchievementUnlocked())
            {
                SteamUserStats.SetAchievement(Name);
                SteamUserStats.StoreStats();
              
                onAchievementUnlocked.Invoke();
            }
        }

        protected virtual bool IsUnlockRequirementsValid() => true;

        public void AddOnAchievementUnlockedListener(UnityAction action)
        {
            if (onAchievementUnlocked != null && action != null)
                onAchievementUnlocked.AddListener(action);
        }
        public void RemoveOnAchievementUnlockedListener(UnityAction action)
        {
            if (onAchievementUnlocked != null && action != null)
                onAchievementUnlocked.RemoveListener(action);
        }

    }
}