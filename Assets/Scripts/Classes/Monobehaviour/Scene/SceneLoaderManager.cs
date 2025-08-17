using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace RedSilver2.Framework.Scenes
{
    public abstract partial class SceneLoaderManager : MonoBehaviour   
    {
        private UnityEvent<int>        onLoadStarted;
        private UnityEvent<int>        onLoadFinished;
        private UnityEvent<int, float> onLoadProgressChanged;


        private static List<SceneData>              SceneDatasInstances    = new List<SceneData>();
        private static Dictionary<int, IEnumerator> sceneLoadingOperations = new Dictionary<int, IEnumerator>();

        public static bool IsLoadingSingleScene { get; private set; } = false;
        public static SceneLoaderManager Instance { get; private set; }


        protected abstract void OnValidate();

        private void Awake()
        {
            if(Instance != null) { Destroy(this); }
            Instance = this;

            onLoadStarted         = new UnityEvent<int>();
            onLoadFinished        = new UnityEvent<int>();
            onLoadProgressChanged = new UnityEvent<int, float>();

            AddOnLoadStartedListener(OnLoadStarted);
            AddOnLoadFinishedListener(OnLoadFinished);
        }

        protected virtual void Start()
        {
            AddOnLoadStartedListener(sceneIndex => Debug.Log($"Loading Scene.. (Scene Index: {sceneIndex})"));
            AddOnLoadProgressChangedListener((sceneIndex, progression) => Debug.Log($"Scene Loaded At {(int)(progression * 100)}% (Scene Index: {sceneIndex})"));
            AddOnLoadFinishedListener(sceneIndex => Debug.Log($"Loaded Scene!! (Scene Index: {sceneIndex})"));          
        }

        private void OnLoadStarted(int sceneIndex)
        {
            if (IsLoadingSingleScene) { StopAllSceneLoadingOperations();       }
            else                      { StopSceneLoadingOperation(sceneIndex); }

            IEnumerator operation = SceneLoadingOperation(sceneIndex);
            sceneLoadingOperations.Add(sceneIndex, operation);
          
            StartCoroutine(operation);
        }

        private void OnLoadFinished(int sceneIndex)
        {
            if (IsLoadingSingleScene)
            {
                // Loading Screen Fade Out;
                IsLoadingSingleScene = false;
            }

            if(sceneLoadingOperations.ContainsKey(sceneIndex))
                sceneLoadingOperations.Remove(sceneIndex);
        }
 


        private IEnumerator SceneLoadingOperation(int sceneIndex)
        {
            if (IsLoadingSingleScene) { } // Loading Screen Fade In;
            yield return StartCoroutine(WaitOperationLoading(sceneIndex));
            onLoadFinished.Invoke(sceneIndex);       
        }

        private IEnumerator WaitOperationLoading(int sceneIndex)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, IsLoadingSingleScene ? LoadSceneMode.Single : LoadSceneMode.Additive);
            SceneData      data      = GetSceneData(sceneIndex);

            operation.allowSceneActivation = false;
            yield return StartCoroutine(WaitOperationLoading(operation, data));       
        }

        private IEnumerator WaitOperationLoading(AsyncOperation operation, SceneData data)
        {
            while (operation.progress < 0.9f)
            {
                SetOperationProgress(operation.progress / 0.9f, data);
                yield return null;
            }

            operation.allowSceneActivation = true;
        }

        private void SetOperationProgress(float progression, SceneData data)
        {
            if (data != null)
            {
                if (IsLoadingSingleScene) onLoadProgressChanged.Invoke(data.SceneIndex, progression);
                else data.SetLoadProgression(progression);
            }
        }

        private void StopSceneLoadingOperation(int sceneIndex)
        {
            if (sceneLoadingOperations.ContainsKey(sceneIndex))
            {
                IEnumerator operation = sceneLoadingOperations[sceneIndex];
                if(operation != null) StopCoroutine(operation);
            }
        }

        private void StopAllSceneLoadingOperations()
        {
            foreach(IEnumerator operation in sceneLoadingOperations.Values) StopCoroutine(operation);
            sceneLoadingOperations.Clear();
        }

        protected void AddOnLoadStartedListener(UnityAction<int> action)
        {
            if (onLoadStarted != null && action != null) onLoadStarted.AddListener(action);
        }
        protected void AddOnLoadFinishedListener(UnityAction<int> action)
        {
            if (onLoadFinished != null && action != null) onLoadFinished.AddListener(action);
        }
        protected void AddOnLoadProgressChangedListener(UnityAction<int, float> action)
        {
            if (onLoadStarted != null && action != null) onLoadProgressChanged.AddListener(action);
        }

        public void LoadSingleScene(int sceneIndex) 
        {
            if (IsValidSceneIndex(sceneIndex) && CanLoadScene(sceneIndex))
            {
                IsLoadingSingleScene = true;
                onLoadStarted.Invoke(sceneIndex);
            }
        }

        public void LoadScene(int sceneIndex) 
        {
            if (IsValidSceneIndex(sceneIndex) && CanLoadScene(sceneIndex))
            {
                onLoadStarted.Invoke(sceneIndex);
            }
        }

        public static bool CanLoadScene(int sceneIndex)
        {
            Debug.Log(IsSceneUnlocked(sceneIndex));
            Debug.Log(IsSceneLoaded(sceneIndex));
            Debug.Log(!sceneLoadingOperations.ContainsKey(sceneIndex));

            if (!IsLoadingSingleScene && !sceneLoadingOperations.ContainsKey(sceneIndex) 
             && !IsSceneLoaded(sceneIndex) && IsSceneUnlocked(sceneIndex)) return true;

            return false;
        }

        public static bool IsSceneLoaded(int sceneIndex)
        {
            if(!IsValidSceneIndex(sceneIndex)) return false; 
            return SceneManager.GetSceneByBuildIndex(sceneIndex).isLoaded;
        }

        public static bool IsSceneUnlocked(int sceneIndex)
        {
            SceneData data = GetSceneData(sceneIndex);
            if (data != null) return data.IsUnlocked;
            return false;
        }

        public static bool IsValidSceneIndex(int sceneIndex)
        {
            if(sceneIndex < 0 || sceneIndex >= SceneManager.sceneCountInBuildSettings) return false;
            return true;
        }

        public static void AddSceneData(SceneData sceneData)
        {
            if (SceneDatasInstances.Where(x => x.Compare(sceneData.SceneIndex)).Count() == 0)
                SceneDatasInstances.Add(sceneData);
        }
        public static void AddSceneData(SceneData[] sceneDatas)
        {
            if(sceneDatas != null)
            {
                sceneDatas = sceneDatas.Where(x => x != null).Distinct().ToArray();
                foreach(SceneData sceneData in sceneDatas) AddSceneData(sceneData);
            }
        }

        public static string[] GetSceneDatas()
        {
            List<string> results = new List<string>();
            SceneData[]  datas   = SceneDatasInstances.OrderBy(x => x.SceneIndex).ToArray();  
            
            foreach(SceneData data in datas) { results.Add($"{data.ToString()}\n"); }
            return results.ToArray();
        }

        public static SceneData   GetSceneData(string sceneName)
        {
            var results = SceneDatasInstances.Where(x => x.Compare(sceneName));
            if (results.Count() > 0) return results.First();
            return null;
        }
        public static SceneData[] GetScenesDatas(string[] sceneNames)
        {
            List<SceneData> results = new List<SceneData>();
            if (sceneNames == null) return results.ToArray();

            foreach (string sceneName in sceneNames)
            {
                SceneData data = GetSceneData(sceneName);
                if(data != null) { results.Add(data); }
            }

            return results.ToArray();
        }

        public static SceneData GetSceneData(int sceneIndex)
        {
            var results = SceneDatasInstances.Where(x => x.Compare(sceneIndex));
            if (results.Count() > 0) return results.First();
            return null;
        }
        public static SceneData[] GetScenesDatas(int[] sceneIndexes)
        {
            List<SceneData> results = new List<SceneData>();
            if (sceneIndexes == null) return results.ToArray();

            foreach (int sceneIndex in sceneIndexes)
            {
                SceneData data = GetSceneData(sceneIndex);
                if (data != null) { results.Add(data); }
            }

            return results.ToArray();
        }
    }
}