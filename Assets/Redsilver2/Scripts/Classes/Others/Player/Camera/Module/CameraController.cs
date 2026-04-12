using RedSilver2.Framework.Inputs.Settings;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RedSilver2.Framework.Player
{
    public abstract class CameraController : Vector2MouseConfigurationEvent
    {

        [Space]
        [SerializeField] private Transform body;
        [SerializeField] private Transform head;

        [Space]
        [SerializeField] private float defaultSensitivityX;
        [SerializeField] private float defaultSensitivityY;

        private float headRotation;
        private float bodyRotation;

        private Vector3 originalHeadRotation;

        public float HeadRotation => headRotation;
        public float BodyRotation => bodyRotation;


        private static List<CameraController> modules;

        private static CameraController current;

        public static CameraController  Current => current;
        public static CameraController[] Modules
        {
            get
            {
                if (modules == null) return new CameraController[0];
                return modules.ToArray();
            }
        }

        protected override void OnLateUpdate()
        {
            if (head != null) head.localEulerAngles = Vector3.right * headRotation + Vector3.up * originalHeadRotation.y + Vector3.forward * originalHeadRotation.z;
            if (body != null) body.localEulerAngles = Vector3.right * originalHeadRotation.x + Vector3.up * bodyRotation + Vector3.forward * originalHeadRotation.z;
        }

        protected override void OnUpdate(Vector2 vector)
        {
            base.OnUpdate(vector);
            UpdateBodyRotation(body, ref bodyRotation);
            UpdateHeadRotation(head, ref headRotation); 
        }

        protected virtual void UpdateBodyRotation(Transform body, ref float rotation) {
            if (body == null) {
                rotation = 0f;
                return;
            }

            rotation += Time.deltaTime * Input.x * OptionsMenu.GetSensitivityX();
        }

        protected virtual void UpdateHeadRotation(Transform head, ref float rotation)
        {
            if (head == null) {
                rotation = 0f;
                return;
            }

            rotation += Time.deltaTime * -Input.y * OptionsMenu.GetSensitivityY();
        }

        public void SetOriginalHeadRotation(Vector3 rotation)
        {
            originalHeadRotation = rotation;
        }

        public void SetOriginalBodyRotation(Vector3 rotation)
        {
            originalHeadRotation = rotation;
        }



        private bool IsActifCameraController() {
            if (current == null) return false;
            return current.Equals(this);
        }

        public static void SetCursorVisibility(bool isVisible)
        {
            Cursor.lockState = isVisible ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = isVisible;
        }

        public static void SetCurrent(int index) {
            SetCurrent(GetModule(index));
        }

        public static void SetCurrent(string moduleName) {
            SetCurrent(GetModule(moduleName));
        }


        public static void SetCurrent(CameraController module)
        {
            Disable();
            current = module;
            Enable();
        }

        public static void Enable() {
            if (current != null) current.enabled = true;
        }


        public static void Disable() {
            if(current != null) current.enabled = false;
        }

        public static void CleanModules() {
            if (modules != null) modules = modules.Where(x => x != null).ToList();
        }

        public static CameraController GetModule(int index) 
        {
            if(modules == null || index < 0 || index >= modules.Count) return null;
            return modules[index];
        }

        public static CameraController GetModule(string moduleName) 
        {
            if (modules == null || string.IsNullOrEmpty(moduleName)) return null;
            
            var results = modules.Where(x => x != null)
                                 .Where(x => x.name.ToLower() == moduleName.ToLower());

            if (results.Count() > 0) return results.First();
            return null;
        }
    }
}
