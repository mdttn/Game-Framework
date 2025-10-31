using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.Interactions.Collectibles
{
    public class CollectibleModelViewer : MonoBehaviour
    {
        [SerializeField] private Camera parent;

        private IEnumerator showModelCoroutine;
        private UnityEvent<GameObject> onUpdateModel;
        private static Dictionary<string, GameObject> collectibleModels = new Dictionary<string, GameObject>();

        protected virtual void Awake(){
            onUpdateModel = new UnityEvent<GameObject>();
        }

        private void OnDisable() {
            HideModel();
        }

        public void AddOnUpdateModelListener(UnityAction<GameObject> action)
        {
            if (onUpdateModel != null && action != null)
                onUpdateModel.AddListener(action);
        }
        public void RemoveOnUpdateModelListener(UnityAction<GameObject> action)
        {
            if (onUpdateModel != null && action != null)
                onUpdateModel.RemoveListener(action);
        }



        public void ShowModel(string modelName)
        {
            ShowModel(GetCollectibleModel(modelName));
        }

        public void ShowModel(GameObject model)
        {
            if (model != null)
            {
                HideModel();
                showModelCoroutine = UpdateModelShown(model, parent);
                StartCoroutine(showModelCoroutine);
            }
        }

        public void HideModel()
        {      
           if(showModelCoroutine != null) StopCoroutine(showModelCoroutine);
           showModelCoroutine = null;
        }

        private IEnumerator UpdateModelShown(GameObject model, Camera parent)
        {
            if (model != null && parent != null)
            {
                model.SetActive(true);
                model.transform.SetParent(parent.transform);

                yield return StartCoroutine(UpdateModelShown(model));
                model.SetActive(false);
            }
        }

        private IEnumerator UpdateModelShown(GameObject model)
        {
            while (model != null)
            {
                onUpdateModel.Invoke(model);
                yield return null;
            }
        }

        public static void AddCollectibleModel(CollectibleData data)
        {
            if (data != null) AddCollectibleModel(data.Model);
        }

        private static void AddCollectibleModel(GameObject model)
        {
            GameObject clone;
           
            if (model == null && collectibleModels != null) return;
            if (collectibleModels.ContainsKey(model.name.ToLower())) return;

            clone = Instantiate(model);
            clone.name = model.name;

            clone.gameObject.SetActive(false);
            collectibleModels.Add(model.name.ToLower(), clone);
        }

        public static GameObject GetCollectibleModel(string modelName)
        {
            modelName = modelName.ToLower();

            if (collectibleModels == null || !collectibleModels.ContainsKey(modelName)) return null;
            return collectibleModels[modelName];         
        }

        public static GameObject[] GetCollectibleModels(string[] modelNames)
        {
            List<GameObject> models = new List<GameObject>();

            foreach(string modelName in modelNames)
            {
                GameObject model = GetCollectibleModel(modelName);
                if(model != null) models.Add(model);
            }

            return models.ToArray();
        }

        public static GameObject[] GetCollectibleModels()
        {
            if(collectibleModels == null) return null;
            return collectibleModels.Values.ToArray();
        } 
    }
}
