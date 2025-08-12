using UnityEngine;

namespace Redsilver2.Framework
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public static GameManager Instance => instance;

        private void Awake()
        {
            if(instance != null) Destroy(gameObject);
            instance = this;
        }
    }
}
