using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RedSilver2.Framework.Player
{
    public abstract class CameraControllerModule : MonoBehaviour
    {
        private CameraController controller;
        public CameraController Controller => controller;


        private static List<CameraControllerModule> modules;

        private static CameraControllerModule current;

        public static CameraControllerModule  Current => current;
        public  static CameraControllerModule[] Modules
        {
            get
            {
                if (modules == null) return new CameraControllerModule[0];
                return modules.ToArray();
            }
        }

        private void Awake() {
            SetCameraController(GetCameraController());
        }

        private void Start(){
            if (current == null)
            {
                current = this;
                controller?.Enable();
            }
        }

        private void Update()
        {
            if (controller != null) controller.Update();
        }

        private void LateUpdate()
        {
            if (controller != null) controller.LateUpdate();
        }

        private void OnEnable()
        {
            SetCursorVisibility(false);
            controller?.Enable();
        }

        private void OnDisable()
        {
            SetCursorVisibility(true);
            controller?.Disable();
        }

        private void SetCameraController(CameraController cameraController)
        {
            this.controller = cameraController;
        }

        private void SetCursorVisibility(bool isVisible)
        {
            Cursor.lockState = isVisible ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = isVisible;
        }

        protected abstract CameraController GetCameraController();

        public static void SetCurrent(int index) {
            SetCurrent(GetModule(index));
        }

        public static void SetCurrent(string moduleName) {
            SetCurrent(GetModule(moduleName));
        }


        public static void SetCurrent(CameraControllerModule module)
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

        public static CameraControllerModule GetModule(int index) 
        {
            if(modules == null || index < 0 || index >= modules.Count) return null;
            return modules[index];
        }

        public static CameraControllerModule GetModule(string moduleName) 
        {
            if (modules == null || string.IsNullOrEmpty(moduleName)) return null;
            
            var results = modules.Where(x => x != null)
                                 .Where(x => x.name.ToLower() == moduleName.ToLower());

            if (results.Count() > 0) return results.First();
            return null;
        }
    }
}
