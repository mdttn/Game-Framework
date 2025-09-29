using UnityEngine;

namespace RedSilver2.Framework.Achievements
{

    public class AchievementUnlocker : MonoBehaviour
    {
        [SerializeField] private uint achievementIndex;
        private Achievement achievement;

        private void Start()
        {
            CreateAchievement(ref achievement);
        }

        public void Unlock()
        {
            if (achievement != null)
            {
                achievement.Unlock();
            }
        }

        public Achievement GetAchievement()
        {
            return AchievementManager.GetAchievement(achievementIndex);
        }

        protected virtual void CreateAchievement(ref Achievement achievement)
        {
            achievement = GetAchievement();

            if(achievement == null)
                achievement = new Achievement(achievementIndex);
        }
    }
}