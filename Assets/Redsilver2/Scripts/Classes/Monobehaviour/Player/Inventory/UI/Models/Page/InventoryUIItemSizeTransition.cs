using System.Collections;
using UnityEngine;



namespace RedSilver2.Framework.Player.Inventories.UI
{
    public class InventoryUIItemSizeTransition : InventoryUITransition
    {
        [Space]
        [SerializeField] private float resizeDuration;
        [SerializeField] private float showSize;

        protected override IEnumerator Transition(GameObject model, bool isShowingModel)
        {
            if (model != null) {
                yield return StartCoroutine(UpdateModelSize(model, model.transform.localScale, isShowingModel));
                SetSize(model, isShowingModel);
            }

            yield return null;
        }

        private IEnumerator UpdateModelSize(GameObject model, Vector3 currentSize, bool isShowingModel)
        {
            float duration = 0;

            while (model != null && duration < resizeDuration) {
                SetSize(model, currentSize, duration, isShowingModel);
                duration += Time.deltaTime;
                yield return null;
            }
        }

        private void SetSize(GameObject model, Vector3 currentSize, float duration, bool isShowingModel)
        {
            if (model != null)
                model.transform.localScale = Vector3.Lerp(currentSize, GetDesiredSize(isShowingModel), duration / resizeDuration);
        }

        private void SetSize(GameObject model, bool isShowingModel)
        {
            if (model != null)
                model.transform.localScale = GetDesiredSize(isShowingModel);
        }

        private Vector3 GetDesiredSize(bool isShowingModel)
        {
            float size = isShowingModel ? showSize : Mathf.Epsilon;
            return Vector3.right * size + Vector3.up * size + Vector3.forward * size; 
        }
    }
}
