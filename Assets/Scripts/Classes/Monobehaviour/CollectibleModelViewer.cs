using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RedSilver2.Framework.Interactions.Collectibles
{
    public abstract class CollectibleModelViewer : MonoBehaviour
    {
        [SerializeField] private Camera parent;

        private IEnumerator showModelCoroutine;
        private static Dictionary<string, GameObject> collectibleModels = new Dictionary<string, GameObject>();

        private void OnDisable()
        {
            HideModel();
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

        public abstract IEnumerator UpdateModelShown(GameObject model, Camera parent);

        public  static void AddCollectibleModel(CollectibleData data)
        {
            if (data != null && collectibleModels != null) AddCollectibleModel(data, data.Model);
        }

        private static void AddCollectibleModel(CollectibleData data, GameObject model)
        {
            if(model != null && collectibleModels != null)
            {
                if(!collectibleModels.ContainsKey(model.name.ToLower()))
                {
                    GameObject clone = Instantiate(model);

                    clone.name = model.name;
                    clone.gameObject.SetActive(false);

                    Debug.Log(clone);

                    collectibleModels.Add(model.name.ToLower(), clone);  
                }
            }
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
