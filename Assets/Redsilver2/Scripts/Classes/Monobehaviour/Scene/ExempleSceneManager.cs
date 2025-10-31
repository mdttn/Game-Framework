using RedSilver2.Framework.Inputs;
using UnityEngine;

namespace RedSilver2.Framework.Scenes
{
    public sealed class ExempleSceneManager : SceneLoaderManager
    {
        private bool flip = false;

        protected void Start()
        {
            Debug.Log("WOW");

            AddSceneData(new SceneData[]
            {
                new TestScene01(0, true),
            });
        }

        private void Update()
        {
            if (InputManager.GetKeyDown(KeyboardKey.Space))
            {
                //flip = !flip;
                //LoadSingleScene(flip ? 1 : 0);
            }
        }
    }

   
}
