using UnityEngine;

namespace RedSilver2.Framework.Interactions.Collectibles
{
    public abstract class CollectibleModel
    {
        private Transform transform;

        private float rotationTrackerX;
        private float rotationTrackerY;
        private float rotationSpeed;

        protected CollectibleModel() {  }

        protected CollectibleModel(Transform transform)
        {
            this.transform        = transform;
            this.rotationSpeed    = 1f;

            this.rotationTrackerX = 0f;
            this.rotationTrackerY = 0f;
        }

        protected CollectibleModel(Transform transform, float rotationSpeed)
        {
            this.transform        = transform;
            this.rotationSpeed    = Mathf.Clamp(rotationSpeed, 1f, float.MaxValue);

            this.rotationTrackerX = 0f;
            this.rotationTrackerY = 0f;
        }

        public void ResetRotation()
        {
            rotationTrackerX            = 0f;
            rotationTrackerY            = 0f;
            transform.localEulerAngles  = Vector3.zero;
        }

        public virtual void Update(Vector3 input)
        {
            rotationTrackerY += Time.deltaTime * input.y * rotationSpeed;
            rotationTrackerX += Time.deltaTime * input.x * rotationSpeed;
        }

        public virtual void LateUpdate()
        {
            if(transform != null) transform.localEulerAngles = Vector3.right * rotationTrackerY + Vector3.up * rotationTrackerX;
        }
    }
}
