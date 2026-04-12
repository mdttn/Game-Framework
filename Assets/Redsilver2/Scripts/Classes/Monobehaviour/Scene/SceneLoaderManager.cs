using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace RedSilver2.Framework.Scenes
{
    public partial class SceneLoaderManager : MonoBehaviour   
    {

        [SerializeField] private LoadingScreen[] loadingScreens;
        [SerializeField] private int defaultLoadingScreenIndex;
        
        private LoadingScreen currentLoadingScreen;


        private readonly UnityEvent<SceneAsset> onSceneAssetAdded   = new UnityEvent<SceneAsset>();
        private readonly UnityEvent<SceneAsset> onSceneAssetRemoved = new UnityEvent<SceneAsset>();

        private readonly UnityEvent<int>        onSingleSceneLoadStarted         = new UnityEvent<int>();
        private readonly UnityEvent<int>        onSingleSceneLoadFinished        = new UnityEvent<int>();
        private readonly UnityEvent<int, float> onSingleSceneLoadProgressChanged = new UnityEvent<int, float>();

        private SceneData[] sceneDatas;
        public const string SCENE_ROOT_PATH = "Scenes/";
       
        private static List<SceneAsset>             sceneAssetsInstances   = new List<SceneAsset>();
        private static Dictionary<int, IEnumerator> sceneLoadingOperations = new Dictionary<int, IEnumerator>();

        public static bool IsLoadingSingleScene { get; private set; } = false;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            defaultLoadingScreenIndex = Mathf.Clamp(defaultLoadingScreenIndex, 0, loadingScreens != null ? loadingScreens.Length - 1 : 0);
        }
        #endif


        private void Awake()
        {
            AddOnSingleSceneLoadStartedListener(OnSingleSceneLoadStarted);
            AddOnSingleSceneLoadFinishedListener(OnSingleSceneLoadFinished);

            if (loadingScreens.Length > 0) currentLoadingScreen = loadingScreens[defaultLoadingScreenIndex];
        }

        private void Start()
        {

        }

        private void OnEnable()
        {
            LoadSceneDatas();
        }

        private void LoadSceneDatas() {

            sceneDatas = Resources.FindObjectsOfTypeAll<SceneData>().Reverse()
                                            .OrderBy(x => x.SceneIndex)
                                            .ToArray();


            foreach (SceneData sceneData in sceneDatas)
            {
                Debug.Log(sceneData);
                sceneData.Load();
            }

        }

        private void OnSingleSceneLoadStarted(int sceneIndex)
        {
            PlayerController.Disable();
            StopAllSceneLoadingOperations();
            IsLoadingSingleScene = true;
        }

        private void OnSingleSceneLoadFinished(int sceneIndex) {
            PlayerController.Enable();
            IsLoadingSingleScene = false;
        }

        private void StartSceneLoad(int sceneIndex)
        {
            IEnumerator operation;
            StopSceneLoadingOperation(sceneIndex);

            operation = SceneLoadingOperation(sceneIndex, IsLoadingSingleScene);
            sceneLoadingOperations.Add(sceneIndex, operation);
          
            StartCoroutine(operation);
        }

        private void FinishSceneLoad(int sceneIndex, bool isLoadingSingleScene)
        {
            if(sceneLoadingOperations.ContainsKey(sceneIndex))
                sceneLoadingOperations.Remove(sceneIndex);

            if (isLoadingSingleScene) 
            { 
                onSingleSceneLoadFinished.Invoke(sceneIndex);
                if (currentLoadingScreen) currentLoadingScreen.gameObject.SetActive(false);
            }
        }
 
        private IEnumerator WaitForLoadingScreen(bool isVisible)
        {
            if(currentLoadingScreen != null && IsLoadingSingleScene)
            {
                if (isVisible) currentLoadingScreen.Show();
                else           currentLoadingScreen.Hide();

                while (!currentLoadingScreen.IsUIVisibilitySet()) yield return null;
            }
        }

        private IEnumerator SceneLoadingOperation(int sceneIndex, bool isLoadingSingleScene)
        {
                yield return WaitForLoadingScreen(true);
                yield return StartCoroutine(WaitOperationLoading(sceneIndex, isLoadingSingleScene));
                yield return WaitForLoadingScreen(false);

                FinishSceneLoad(sceneIndex, isLoadingSingleScene);  
        }

        private IEnumerator WaitOperationLoading(int sceneIndex, bool isLoadingSingleScene)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, IsLoadingSingleScene ? LoadSceneMode.Single : LoadSceneMode.Additive);

            operation.allowSceneActivation = false;
            yield return StartCoroutine(WaitOperationLoading(operation, GetSceneData(sceneIndex), isLoadingSingleScene));       
        }

        private IEnumerator WaitOperationLoading(AsyncOperation operation, SceneAsset data, bool isLoadingSingleScene)
        {
            if (operation != null)
            {
                while (operation.progress < 0.9f)
                {
                    UpdateOperationProgress(data, operation.progress / 0.9f, isLoadingSingleScene);
                    yield return null;
                }

                UpdateOperationProgress(data, 1f, isLoadingSingleScene);
                operation.allowSceneActivation = true;
            }
        }

        private void UpdateOperationProgress(SceneAsset asset, float progress, bool isLoadingSingleScene)
        {
            progress = Mathf.Clamp01(progress);

            if (asset != null)
            {
                if (!isLoadingSingleScene) SetOperationProgress(progress, asset);
                else SetOperationProgress(asset.Data.SceneIndex, progress);
            }
        }

        private void SetOperationProgress(float progression, SceneAsset data)
        {
            if (data != null) data.SetLoadProgression(progression);           
        }

        private void SetOperationProgress(int sceneIndex, float progress)
        {
            if (IsLoadingSingleScene) onSingleSceneLoadProgressChanged.Invoke(sceneIndex, progress);
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

        public void AddOnSceneAssetAddedListener(UnityAction<SceneAsset> action)
        {
            if(action != null && onSceneAssetAdded != null)
                onSceneAssetAdded.AddListener(action);
        }
        public void RemoveOnSceneAssetAddedListener(UnityAction<SceneAsset> action)
        {
            if (action != null && onSceneAssetAdded != null)
                onSceneAssetAdded.RemoveListener(action);
        }


        public void AddOnSceneAssetRemovedListener(UnityAction<SceneAsset> action)
        {
            if (action != null && onSceneAssetRemoved != null)
                onSceneAssetRemoved.AddListener(action);
        }

        public void RemoveOnSceneAssetRemovedListener(UnityAction<SceneAsset> action)
        {
            if (action != null && onSceneAssetRemoved != null)
                onSceneAssetRemoved.RemoveListener(action);
        }

        public void AddOnSingleSceneLoadStartedListener(UnityAction<int> action)
        {
            if (onSingleSceneLoadStarted != null && action != null) onSingleSceneLoadStarted.AddListener(action);
        }
        public void AddOnSingleSceneLoadFinishedListener(UnityAction<int> action)
        {
            if (onSingleSceneLoadFinished != null && action != null) onSingleSceneLoadFinished.AddListener(action);
        }
        public void AddOnSingleSceneLoadProgressChangedListener(UnityAction<int, float> action)
        {
            if (onSingleSceneLoadStarted != null && action != null) onSingleSceneLoadProgressChanged.AddListener(action);
        }

        public void RemoveOnSingleSceneLoadStartedListener(UnityAction<int> action)
        {
            if (onSingleSceneLoadStarted != null && action != null)  onSingleSceneLoadStarted.RemoveListener(action);
        }
        public void RemoveOnSingleSceneLoadFinishedListener(UnityAction<int> action)
        {
            if (onSingleSceneLoadFinished != null && action != null) onSingleSceneLoadFinished.RemoveListener(action);
        }
        public void RemoveOnSingleSceneLoadProgressChangedListener(UnityAction<int, float> action)
        {
            if (onSingleSceneLoadStarted != null && action != null)  onSingleSceneLoadProgressChanged.RemoveListener(action);
        }

        public void LoadSingleScene(string scenePath)
        {
            SceneAsset asset = GetSceneData(scenePath);
            if (asset != null)  LoadSingleScene(asset.Data.ScenePath); 
        }

        public void LoadSingleScene(int sceneIndex) 
        {
            if (IsValidSceneIndex(sceneIndex) && CanLoadScene(sceneIndex))
            {
                if (currentLoadingScreen) currentLoadingScreen.gameObject.SetActive(true);
               
                onSingleSceneLoadStarted.Invoke(sceneIndex);
                StartSceneLoad(sceneIndex);
            }
        }

        public void LoadScene(string sceneName)
        {
            SceneAsset asset = GetSceneData(sceneName);
            if (asset != null) { LoadScene(asset.Data.SceneIndex); }
        }

        public void LoadScene(int sceneIndex) 
        {
            if (IsValidSceneIndex(sceneIndex) && CanLoadScene(sceneIndex))
            {
                onSingleSceneLoadStarted.Invoke(sceneIndex);
            }
        }

        public bool CanLoadScene(int sceneIndex)
        {
            Debug.Log(IsSceneUnlocked(sceneIndex));
     
            if (!IsLoadingSingleScene && !sceneLoadingOperations.ContainsKey(sceneIndex) 
             && !IsSceneLoaded(sceneIndex) && IsSceneUnlocked(sceneIndex)) return true;

            return false;
        }

        public bool IsSceneLoaded(int sceneIndex)
        {
            if(!IsValidSceneIndex(sceneIndex)) return false; 
            return SceneManager.GetSceneByBuildIndex(sceneIndex).isLoaded;
        }

        public bool IsSceneUnlocked(int sceneIndex)
        {
            return true;

            SceneAsset data = GetSceneData(sceneIndex);

            if (data != null) return data.IsUnlocked;
            return false;
        }

        public bool IsValidSceneIndex(int sceneIndex)
        {
            if(sceneIndex < 0 || sceneIndex >= SceneManager.sceneCountInBuildSettings) return false;
            return true;
        }

        public void AddSceneAsset(SceneAsset asset)
        {
            Debug.Log(asset);
            if (asset == null) return;

            if (!sceneAssetsInstances.Contains(asset))
            {
                Debug.Log(sceneAssetsInstances.Count + " Add");

                sceneAssetsInstances?.Add(asset);
                onSceneAssetAdded?.Invoke(asset);
            }
        }

        public string[] GetSceneDatas()
        {
            List<string> results = new List<string>();
            SceneAsset[] assets  = sceneAssetsInstances.OrderBy(x => x.Data).ToArray();  
            
            foreach(SceneAsset data in assets) { results.Add($"{data.ToString()}\n"); }
            return results.ToArray();
        }

        public SceneAsset GetSceneData(string scenePath)
        {
            var results = sceneAssetsInstances.Where(x => x.Compare(scenePath));
            if (results.Count() > 0) return results.First();
            return null;
        }

        public SceneAsset[] GetScenesDatas(string[] sceneNames)
        {
            List<SceneAsset> results = new List<SceneAsset>();
            if (sceneNames == null) return results.ToArray();

            foreach (string sceneName in sceneNames)
            {
                SceneAsset data = GetSceneData(sceneName);
                if(data != null) { results.Add(data); }
            }

            return results.ToArray();
        }

        public SceneAsset GetSceneData(int sceneIndex)
        {

            var results = sceneAssetsInstances.Where(x => x.Compare(sceneIndex));
            if (results.Count() > 0) return results.First();
            return null;
        }

        public SceneAsset[] GetScenesDatas(int[] sceneIndexes)
        {
            List<SceneAsset> results = new List<SceneAsset>();
            if (sceneIndexes == null) return results.ToArray();

            foreach (int sceneIndex in sceneIndexes)
            {
                SceneAsset data = GetSceneData(sceneIndex);
                if (data != null) { results.Add(data); }
            }

            return results.ToArray();
        }

        public SceneAsset[] GetAllScenesDatas() => sceneAssetsInstances.ToArray();
        
    }
}