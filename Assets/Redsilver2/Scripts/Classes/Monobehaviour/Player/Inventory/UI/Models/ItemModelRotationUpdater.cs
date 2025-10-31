using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public class ItemModelRotationUpdater : ItemModelUpdater
    {
        [Space]
        [SerializeField] private float rotationSpeed;

        protected sealed override void UpdateModels(int horizontalIndex, GameObject model, SimpleInventoryUINavigator navigator) {
            UpdateModelRotation(model);
        }

        protected sealed override void UpdateModels(int verticalIndex, int horizontalIndex, GameObject model, VerticalInventoryUINavigator navigator) {
            UpdateModelRotation(model);
        }

        protected virtual void UpdateModelRotation(GameObject model)
        {
            if(model == null || !enabled) return;
            Transform transform = model.transform;
            transform.localEulerAngles += Time.deltaTime * Vector3.up * rotationSpeed;
        }
    }
}
