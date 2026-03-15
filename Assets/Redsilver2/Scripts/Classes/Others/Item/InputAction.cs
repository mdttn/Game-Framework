using UnityEngine.Events;


namespace RedSilver2.Framework.Inputs {
    public abstract class InputAction {
        public readonly string Name;
        private bool isEnabled;
        private bool isExecuted;

        private readonly UnityEvent onEnabled  , onDisabled, 
                                    onExecuted , onReseted;
        public  bool IsEnabled => isEnabled;
        public bool  IsExecuted => isExecuted;

        protected InputAction(string actionName)
        {
            this.Name = actionName;

            this.onEnabled  = new UnityEvent();
            this.onDisabled = new UnityEvent();
            this.onExecuted = new UnityEvent();
            this.onReseted  = new UnityEvent();

            AddOnDisabledListener(() => {
                isEnabled = false;
                isExecuted = false;
            });

            AddOnEnabledListener(()  => { isEnabled = true; });
            AddOnExecutedListener(() => { isExecuted = true; });
            AddOnResetedListener(()  => { isExecuted = false; });
        }

        public virtual void Update() 
        {
            if (isEnabled == false) return;

            isExecuted = false;

            if (this.isEnabled && CanExecute())
                onExecuted?.Invoke();
        }

        public bool Compare(string name)
        {
            if(string.IsNullOrEmpty(name)) return false;
            return this.Name.ToLower().Equals(name.ToLower());
        }

        public void AddOnEnabledListener(UnityAction action) 
        {
            if (action != null) onEnabled?.AddListener(action);
        }

        public void RemoveOnEnabledListener(UnityAction action) 
        {
            if (action != null) onEnabled?.RemoveListener(action);
        }

        public void AddOnDisabledListener(UnityAction action)
        {
            if (action != null) onDisabled?.AddListener(action);
        }
        public void RemoveOnDisabledListener(UnityAction action)
        {
            if (action != null) onDisabled?.RemoveListener(action);
        }

        public void AddOnExecutedListener(UnityAction action)
        {
            if (action != null) onExecuted?.AddListener(action);
        }
        public void RemoveOnExecutedListener(UnityAction action)
        {
            if (action != null) onExecuted?.RemoveListener(action);
        }

        public void AddOnResetedListener(UnityAction action)
        {
            if (action != null) onReseted?.AddListener(action);
        }
        public void RemoveOnResetedListener(UnityAction action)
        {
            if (action != null) onReseted?.RemoveListener(action);
        }

        public void Enable() {
            if (!isEnabled) onEnabled?.Invoke();
        }
        public void Disable() {
            if (isEnabled) onDisabled?.Invoke();    
        }

        public void Reset() {
            onReseted?.Invoke();
        }

        protected abstract bool CanExecute();
    }
}
