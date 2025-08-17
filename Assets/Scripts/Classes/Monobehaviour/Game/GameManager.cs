using RedSilver2.Framework.Inputs;
using UnityEngine;

namespace RedSilver2.Framework
{
    public sealed class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public static GameManager Instance => instance;

        private void Awake()
        {
            if (instance != null) { Destroy(gameObject); return; }
            instance = this;
            DontDestroyOnLoad(instance);
        }

        private void Start()
        {

        }

        private void Update()
        {

        }
    }
}
