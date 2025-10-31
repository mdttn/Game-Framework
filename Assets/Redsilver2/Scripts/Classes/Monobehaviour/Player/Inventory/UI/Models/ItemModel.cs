using RedSilver2.Framework.Interactions.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RedSilver2.Framework.Player.Inventories.UI
{
    public static class ItemModel
    {
        private static List<GameObject> borrowedModels = new List<GameObject>();
        private static Dictionary<string, Queue<GameObject>> itemModelsInstances = new Dictionary<string, Queue<GameObject>>();
        private const string ITEM_DISPLAYER_LAYER_NAME = "Item Displayer";

        public static void ReturnBorrowedModels(GameObject[] models)
        {
            if (itemModelsInstances == null || borrowedModels == null || models == null) return;

            foreach (GameObject gameObject in models.Where(x => x != null))
            {
                ReturnBorrowedModel(gameObject);
            }
        }

        public static void ReturnBorrowedModels(GameObject[,] models)
        {
            if (itemModelsInstances == null || borrowedModels == null || models == null) return;

            for (int i = 0; i < models.GetLength(0); i++)
                for (int j = 0; j < models.GetLength(1); j++)
                    ReturnBorrowedModel(models[i, j]);
        }

        public static void ReturnBorrowedModels()
        {
            if (itemModelsInstances == null || borrowedModels == null) return;

            foreach (GameObject gameObject in borrowedModels.Where(x => x != null))
            {
                ReturnBorrowedModel(gameObject);
            }

            borrowedModels.Clear();
        }

        private static void ReturnBorrowedModel(GameObject gameObject)
        {
            string modelName;
            if (itemModelsInstances == null || borrowedModels == null || gameObject == null) return;

            modelName = gameObject.name.ToLower();
            if (!borrowedModels.Contains(gameObject)) return;

            borrowedModels.Remove(gameObject);
            if (!itemModelsInstances.ContainsKey(modelName)) itemModelsInstances.Add(modelName, new Queue<GameObject>());

            gameObject.transform.SetParent(null);
            gameObject.SetActive(false);

            itemModelsInstances[modelName].Enqueue(gameObject);
            Object.DontDestroyOnLoad(gameObject);
        }

        public static GameObject[]  GetModels(SimpleInventoryUINavigator navigator)
        {
            if (navigator == null) return new GameObject[0];
            return GetModels(navigator.Items);
        }
        public static GameObject[,] GetModels(VerticalInventoryUINavigator navigator)
        {
            if (navigator == null) return new GameObject[0, 0];
            return GetModels(navigator.Items);
        }

        public static GameObject[]  GetModels(Item[] items)
        {
            List<GameObject> results;
            if(items == null || items.Length == 0) return new GameObject[0];

            results = new List<GameObject>();
          
            foreach(Item item in items)
                results.Add(GetModel(item));

            return results.ToArray();
        }
        public static GameObject[,] GetModels(Item[,] items)
        {
            GameObject[,] results;
            if(items == null || items.GetLength(0) == 0 || items.GetLength(1) == 0) return new GameObject[0,0];     
            
            results = new GameObject[items.GetLength(0), items.GetLength(1)];

            for (int i = 0; i < items.GetLength(0); i++)
                for (int j = 0; j < items.GetLength(1); j++)
                    results[i, j] = GetModel(items[i, j]);

            return results;
        }

        public static GameObject GetModel(Item item)
        {
            if(item == null) return null;
            return GetModel(item.GetData() as ItemData);            
        }
        public static GameObject GetModel(Item item, Transform parent)
        {
           GameObject _item = GetModel(item);
           
           if(_item != null && parent != null) {
                _item.transform.SetParent(parent);
                _item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
           }

           return _item;
        }


        private static GameObject GetModel(ItemData data)
        {
            if(data == null) return null;
            return GetModel(data.Model);       
        }

        private static GameObject GetModel(GameObject model)
        {
            string modelName;  
            if(model == null) return null;

            modelName = model.name.ToLower();

            if (!itemModelsInstances.ContainsKey(modelName))
                itemModelsInstances.Add(modelName, new Queue<GameObject>());

            return itemModelsInstances[modelName].Count > 0 ? GetModel(modelName) : CreateAndGetNewModel(model);
        }

        private static GameObject GetModel(string modelName) {

            GameObject model;
            modelName  =  modelName.ToLower();

            if (itemModelsInstances == null || string.IsNullOrEmpty(modelName) 
            || !itemModelsInstances.ContainsKey(modelName) || itemModelsInstances[modelName].Count <= 0) { return null; }

            model = itemModelsInstances[modelName].Dequeue();
            borrowedModels.Add(model);
            return model;
        }

        private static GameObject CreateAndGetNewModel(GameObject model)
        {

            Debug.Log("Is Model Null: ");


            if (model == null) return null;

            Debug.Log("Model Created..");

            GameObject copy = Object.Instantiate(model);
            copy.name = model.name;
            copy.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            copy.layer = LayerMask.NameToLayer(ITEM_DISPLAYER_LAYER_NAME);
            copy.SetActive(false);

            borrowedModels.Add(copy);
            return copy;
        }

    }
}
