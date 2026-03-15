using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Player
{
    public abstract class CameraController
    {
        protected readonly Transform head;
        protected readonly Transform body;

        private   readonly UnityEvent onUpdate;
        private   readonly UnityEvent onLateUpdate;

        public Transform Head => head;
        public Transform Body => body;

        protected CameraController() { }
        protected CameraController(Transform body, Transform head)
        {
            this.body = body;
            this.head = head;

            this.onUpdate     = new UnityEvent();
            this.onLateUpdate = new UnityEvent();

            AddOnUpdateListener(OnUpdate);
            AddOnLateUpdateListener(OnLateUpdate);
        }

        protected abstract void OnUpdate();
        protected abstract void OnLateUpdate();

        public void Update()
        {
            onUpdate.Invoke();
        }
        public void LateUpdate()
        {
            onLateUpdate.Invoke();
        }      

        public void AddOnUpdateListener(UnityAction action) 
        {
            if(onUpdate != null && action != null) onUpdate.AddListener(action);
        }
        public void RemoveOnUpdateListener(UnityAction action)
        {
            if (onUpdate != null && action != null) onUpdate.AddListener(action);
        }

        public void AddOnLateUpdateListener(UnityAction action)
        {
            if (onLateUpdate != null && action != null) onLateUpdate.AddListener(action);
        }
        public void RemoveOnLateUpdateListener(UnityAction action)
        {
            if (onLateUpdate != null && action != null) onLateUpdate.AddListener(action);
        }

        public abstract void Enable();
        public abstract void Disable();

        public static float GetSensitivityX()
        {
            return 5f;
        }

        public static float GetSensitivityY()
        {
            return 5f;
        }
    }
}
