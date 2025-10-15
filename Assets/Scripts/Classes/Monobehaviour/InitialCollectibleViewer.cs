using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.Interactions.Collectibles
{
    public class InitialCollectibleViewer : CollectibleModelViewer
    {
        [SerializeField] private float rotationSpeed = 20f;
        [SerializeField] private float distance = 10f;

        public override IEnumerator UpdateModelShown(GameObject model, Camera parent)
        {
            if(model != null && parent != null)
            {
                model.SetActive(true);
                model.transform.parent = parent.transform;

                yield return StartCoroutine(UpdateModelShown(model));
                model.SetActive(false);
            }
        }

        private IEnumerator UpdateModelShown(GameObject model)
        {
            while (model != null)
            {
                Transform transform = model.transform;
                transform.localPosition = Vector3.forward * distance;
            
                transform.localEulerAngles += Time.deltaTime * Vector3.up * rotationSpeed;
                yield return null;
            }
        }
    }
}