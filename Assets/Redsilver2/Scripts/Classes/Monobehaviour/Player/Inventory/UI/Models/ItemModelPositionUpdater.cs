using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public class ItemModelPositionUpdater : ItemModelUpdater
    {
        [Space]
        [SerializeField] protected float modelOffsetX;
        [SerializeField] protected float modelOffsetY;

        [Space]
        [SerializeField] protected float modelPositionLerpSpeed;

        protected sealed override void UpdateModels(int horizontalIndex, GameObject model, SimpleInventoryUINavigator navigator) {
            UpdateModelPosition(horizontalIndex, model);
        }

        protected sealed override void UpdateModels(int verticalIndex, int horizontalIndex, GameObject model, VerticalInventoryUINavigator navigator) {
            UpdateModelPosition(verticalIndex, horizontalIndex, model);
        }

        private void UpdateModelPosition(int horizontalIndex, GameObject model) {
            UpdateModelPosition(0, horizontalIndex, model);
        }

        protected virtual void UpdateModelPosition(int verticalIndex, int horizontalIndex, GameObject model) 
        {
            Transform transform;
            if (model == null && !enabled) return;

            transform = model.transform;
            transform.localPosition = Vector3.Lerp(transform.localPosition, -Vector3.up      * (verticalIndex   * modelOffsetY) + 
                                                                             Vector3.right   * (horizontalIndex * modelOffsetX), 
                                                                             Time.deltaTime  * modelPositionLerpSpeed);
        }
    }

}
