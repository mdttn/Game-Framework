using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace RedSilver2.Framework.Scenes
{
    public partial class SceneLoaderManager : MonoBehaviour
    {
        public class SceneAsset
        {
            private bool isUnlocked;
            public  readonly SceneData Data;

            private readonly UnityEvent onLoadStarted = new UnityEvent();
            private readonly UnityEvent onLoadFinished = new UnityEvent();
            private readonly UnityEvent<float> onLoadProgressChanged = new UnityEvent<float>();

            public bool IsUnlocked => isUnlocked;

            protected SceneAsset(SceneData sceneData, bool isUnlocked) {
                SceneLoaderManager sceneLoaderManager = GameManager.SceneLoaderManager;
                SetSceneLoaderManagerEvents(sceneLoaderManager);

                this.isUnlocked = isUnlocked;
                this.Data = sceneData;

                sceneLoaderManager?.AddSceneAsset(this);
            }

            private void SetSceneLoaderManagerEvents(SceneLoaderManager sceneLoaderManager)
            {
                if (sceneLoaderManager != null)
                {
                    sceneLoaderManager.AddOnSingleSceneLoadStartedListener(OnLoadStarted);
                    sceneLoaderManager.AddOnSingleSceneLoadFinishedListener(OnLoadFinished);
                    sceneLoaderManager.AddOnSingleSceneLoadProgressChangedListener(OnLoadProgressChanged);
                }
            }

            private void OnLoadStarted(int sceneIndex)
            {
                if (Data == null) return;
                else if (sceneIndex == Data.SceneIndex) { onLoadStarted.Invoke(); }
            }
            private void OnLoadFinished(int sceneIndex)
            {
                if (Data == null) return;
                else if (sceneIndex == Data.SceneIndex) { onLoadFinished.Invoke(); }
            }
            private void OnLoadProgressChanged(int sceneIndex, float progress)
            {
                if      (Data == null) return;
                else if (sceneIndex == Data.SceneIndex) { onLoadProgressChanged.Invoke(progress); }
            }

            public void AddOnLoadStartedListener(UnityAction action)
            {
                if (onLoadStarted != null && action != null) onLoadStarted.AddListener(action);
            }
            public void AddOnLoadStartedListener(UnityAction[] actions)
            {
                if (actions != null)
                    foreach (UnityAction action in actions) AddOnLoadStartedListener(action);
            }

            public void RemoveOnLoadStartedListener(UnityAction action)
            {
                if (onLoadStarted != null && action != null) onLoadStarted.RemoveListener(action);
            }
            public void RemoveOnLoadStartedListener(UnityAction[] actions)
            {
                if (actions != null)
                    foreach (UnityAction action in actions) RemoveOnLoadStartedListener(action);
            }

            public void AddOnLoadFinishedListener(UnityAction action)
            {
                if (onLoadFinished != null && action != null) onLoadFinished.AddListener(action);
            }
            public void AddOnLoadFinishedListener(UnityAction[] actions)
            {
                if (actions != null)
                    foreach (UnityAction action in actions) AddOnLoadStartedListener(action);
            }

            public void RemoveOnLoadFinishedListener(UnityAction action)
            {
                if (onLoadFinished != null && action != null) onLoadFinished.RemoveListener(action);
            }
            public void RemoveOnLoadFinishedListener(UnityAction[] actions)
            {
                if (actions != null)
                    foreach (UnityAction action in actions) RemoveOnLoadStartedListener(action);
            }

            public void AddOnLoadProgressChangedListener(UnityAction<float> action)
            {
                if (onLoadStarted != null && action != null) onLoadProgressChanged.AddListener(action);
            }
            public void AddOnLoadProgressChangedListener(UnityAction<float>[] actions)
            {
                if (actions != null)
                    foreach (UnityAction<float> action in actions) AddOnLoadProgressChangedListener(action);
            }

            public void RemoveOnLoadProgressChangedListener(UnityAction<float> action)
            {
                if (onLoadProgressChanged != null && action != null) onLoadProgressChanged.RemoveListener(action);
            }
            public void RemoveOnLoadProgressChangedListener(UnityAction<float>[] actions)
            {
                if (actions != null)
                    foreach (UnityAction<float> action in actions) RemoveOnLoadProgressChangedListener(action);
            }

            public void SetLoadProgression(float progression) { onLoadProgressChanged.Invoke(progression); }
            public bool IsLoaded()
            {
                if (Data == null) return true;
                return SceneManager.GetSceneByBuildIndex(Data.SceneIndex).isLoaded;
            }

            public bool Compare(string sceneName) {
                if (!string.IsNullOrEmpty(sceneName)) return sceneName.ToLower().Equals(Data.SceneName.ToLower());
                return false;
            }

            public bool Compare(int sceneIndex) {
                if (Data == null) return false;
                return sceneIndex == Data.SceneIndex;
            }

            public Sprite GetThumbnail(int index) {
                if(Data == null) return null;
                Sprite[] thumbnails = Data.SceneThumbnails;

                if (index >= 0 && index < thumbnails.Length) return thumbnails[index];
                return null;
            }

            public Sprite GetRandomThumbnail() {
                if (Data == null) return null;
                Sprite[] thumbnails = Data.SceneThumbnails;

                if (thumbnails.Length > 0) return thumbnails[Random.Range(0, thumbnails.Length)];
                return null;
            }

            public static SceneAsset CreateAndGet(SceneData data, bool isUnlocked) {
                if(data == null) return null;
                return new SceneAsset(data, isUnlocked);
            }
        }
    }
}
