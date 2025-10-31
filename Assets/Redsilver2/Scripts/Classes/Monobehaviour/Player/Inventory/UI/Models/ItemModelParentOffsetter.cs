using System.Collections;
using UnityEngine;


namespace RedSilver2.Framework.Player.Inventories.UI
{
    public class ItemModelParentOffsetter : InventoryUI
    {
        [Space]
        [SerializeField] private float offsetX;
        [SerializeField] private float offsetY;
        [SerializeField] private float offsetZ;

        [Space]
        [SerializeField] private float offsetSpeed;

        protected override void Start()
        {
            if(navigator != null)
                navigator.AddOnLateUpdateListener(UpdateParentPosition);
        }

        protected override void OnDestroy() 
        {
            if (navigator != null)
                navigator.RemoveOnLateUpdateListener(UpdateParentPosition);
        }

        private void UpdateParentPosition()
        {
            if (navigator != null && enabled)
                UpdateParentPosition(navigator.ModelParentTransform);
        }

        private void UpdateParentPosition(Transform transform)
        {
            if (transform != null)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition,
                                                       Vector3.right   * offsetX +
                                                       Vector3.up      * offsetY +
                                                       Vector3.forward * offsetZ,
                                                       Time.deltaTime  * offsetSpeed);
                                                                                
            }
        }
    }
}
