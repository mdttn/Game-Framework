using UnityEngine.Events;

namespace RedSilver2.Framework.Inputs
{
    public abstract class InputHandler
    {
        private bool isEnabled = false;
        private UnityEvent onEnable;
        private UnityEvent onDisable;

        public  bool IsEnabled => isEnabled;

        private InputHandler() { }

        protected InputHandler(string name) 
        { 
            onEnable  = new UnityEvent();
            onDisable = new UnityEvent();
            InputManager.AddInputHandler(name, this);
        }   

        public void Enable() 
        { 
            if (!isEnabled) 
            {
                isEnabled = true;
                onEnable?.Invoke();
            }
        }
        public void Disable() 
        {
            if (isEnabled)
            {
                isEnabled = false;
                onDisable?.Invoke();
            }
        }

        public abstract void Update();
        public abstract string GetPaths();
    }
}
