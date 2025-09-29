using Steamworks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Achievements
{
    public class AchievementManager : MonoBehaviour
    {
        private List<Achievement>  achievements;

        private void Awake()
        {
            if (!SteamManager.Initialized) return;
            achievements          = new List<Achievement>();
        }

        private void Add(Achievement achievement)
        {
            if (achievements != null && IsValidAchievement(achievement))
                achievements.Add(achievement);
        }

        private Achievement Get(uint achievementIndex)
        {
            if(achievements != null && IsValidAchivementIndex(achievementIndex))
            {
                var results = achievements.Where(x => x.Index == achievementIndex);
                if (results.Count() > 0) return results.First();
            }

            return null;
        }
            
        private bool IsValidAchievement(Achievement achievement)
        {
            if (achievement != null || achievements != null) return false;
            if (IsValidAchivementIndex(achievement.Index))   return false;
            return achievements.Where(x => achievement.Index == x.Index).Count() == 0;
        }

        private bool IsValidAchivementIndex(uint index)
        {
            return index >= 0 && index <= SteamUserStats.GetNumAchievements();
        }


        public static void UnlockAchievement(uint achievementIndex)
        {
            Achievement achievement = GetAchievement(achievementIndex);
            if (achievement != null) { }
        }

        public static string GetAchievementName(uint achievementIndex)
        {
            string[] achievementNames = GetAchievementNames();
            if (achievementNames.Length == 0 || achievementIndex < 0 || achievementIndex >= achievementNames.Length) return string.Empty;
            return achievementNames[achievementIndex];
        }
        public static string[] GetAchievementNames()
        {
            List<string> results = new List<string>();

            for (uint i = 0; i < SteamUserStats.GetNumAchievements(); i++)
                results.Add(SteamUserStats.GetAchievementName(i));

            return results.ToArray();
        }

        public static void AddAchievement(Achievement achievement)
        {
            AchievementManager manager = GameManager.GetAchievementManager();
            if (manager != null) manager.Add(achievement);
        }

        public static Achievement GetAchievement(uint achievementIndex)
        {
            AchievementManager manager = GameManager.GetAchievementManager();
            if (manager != null) return manager.Get(achievementIndex);
            return null;
        }
    }
}
