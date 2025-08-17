using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace RedSilver2.Framework.Scenes
{
    public abstract partial class SceneLoaderManager : MonoBehaviour
    {
        public abstract class SceneData 
        {
            [SerializeField] private int  sceneIndex;
            [SerializeField] private bool isUnlocked;

            private string sceneName;

            private readonly UnityEvent        onLoadStarted;
            private readonly UnityEvent        onLoadFinished;
            private readonly UnityEvent<float> onLoadProgressChanged;

            public string SceneName  => sceneName;
            public int    SceneIndex => sceneIndex;
            public bool   IsUnlocked => isUnlocked;

            protected SceneData()
            {
                this.onLoadStarted         = new UnityEvent();
                this.onLoadFinished        = new UnityEvent();
                this.onLoadProgressChanged = new UnityEvent<float>();
                AddSceneData(this);
            }

            public SceneData(string sceneName, int sceneIndex)
            {
                this.sceneName  = sceneName;
                this.sceneIndex = sceneIndex;
                this.isUnlocked = false;

                InitializeEvents(ref onLoadStarted, ref onLoadFinished, ref onLoadProgressChanged);
                SetSceneLoaderManagerEvents(Instance);
                AddSceneData(this);
            }

            public SceneData(string sceneName, int sceneIndex, bool isUnlocked)
            {
                this.sceneName  = sceneName;
                this.sceneIndex = sceneIndex;
                this.isUnlocked = isUnlocked;

                InitializeEvents(ref onLoadStarted, ref onLoadFinished, ref onLoadProgressChanged);
                SetSceneLoaderManagerEvents(Instance);
                AddSceneData(this);
            }

            private void InitializeEvents(ref UnityEvent event01, ref UnityEvent event02, ref UnityEvent<float> event03)
            {
                event01 = new UnityEvent();
                event02 = new UnityEvent();
                event03 = new UnityEvent<float>();
            }
            protected virtual void SetSceneLoaderManagerEvents(SceneLoaderManager sceneLoaderManager)
            {
                if (sceneLoaderManager != null)
                {
                    sceneLoaderManager.AddOnLoadStartedListener(OnLoadStarted);
                    sceneLoaderManager.AddOnLoadFinishedListener(OnLoadFinished);
                    sceneLoaderManager.AddOnLoadProgressChangedListener(OnLoadProgressChanged);
                }
            }

            private void OnLoadStarted(int sceneIndex)
            {
                if (sceneIndex == this.sceneIndex) { onLoadStarted.Invoke(); }
            }
            private void OnLoadFinished(int sceneIndex)
            {
                if (sceneIndex == this.sceneIndex) { onLoadFinished.Invoke(); }
            }
            private void OnLoadProgressChanged(int sceneIndex, float progress)
            {
                if (sceneIndex == this.sceneIndex) { onLoadProgressChanged.Invoke(progress); }
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
            public bool IsLoaded() => SceneManager.GetSceneByBuildIndex(sceneIndex).isLoaded;

            public bool Compare(string sceneName)
            {
                if (!string.IsNullOrEmpty(sceneName)) return sceneName.ToLower() == this.sceneName.ToLower();
                return false;
            }
            public bool Compare(int sceneIndex)
            {
                return sceneIndex == this.sceneIndex;
            }
        }
    }
}
