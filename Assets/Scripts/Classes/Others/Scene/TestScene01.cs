using UnityEngine;

namespace RedSilver2.Framework.Scenes
{
    public abstract partial class SceneLoaderManager : MonoBehaviour
    {
        public class TestScene01 : SceneData
        {
            private TestScene01() : base() { }
            public TestScene01(string sceneName, int sceneIndex, bool isUnlocked) : base(sceneName, sceneIndex, isUnlocked)
            {

            }

            protected override void SetSceneLoaderManagerEvents(SceneLoaderManager sceneLoaderManager)
            {
                base.SetSceneLoaderManagerEvents(sceneLoaderManager);
            }
        }
    }
}
