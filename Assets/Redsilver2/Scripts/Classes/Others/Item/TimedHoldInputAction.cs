using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Inputs
{
    public class TimedHoldInputAction : HoldInputAction
    {
        private float triggerTime, currentTriggerTime;
        private bool  isInstantTriggerTimeReset;
        private UnityEvent<float> onUpdated;


        public TimedHoldInputAction(string actionName, float triggerTime, HoldInput input) : base(actionName, input)
        {
            this.triggerTime = Mathf.Clamp(triggerTime, Mathf.Epsilon, float.MaxValue);
            this.currentTriggerTime = 0f;
         
            this.isInstantTriggerTimeReset = false;
            this.onUpdated = new UnityEvent<float>();

            this.AddOnDisabledListener(() => {
                this.currentTriggerTime = 0f;
                this.onUpdated.Invoke(currentTriggerTime);
            });

            this.AddOnResetedListener(() => {
                this.currentTriggerTime = 0f;
                this.onUpdated.Invoke(currentTriggerTime);
            });
        }

        public TimedHoldInputAction(string actionName, float triggerTime, OverrideableHoldInput input) : base(actionName, input)
        {
            this.triggerTime = Mathf.Clamp(triggerTime, 0f, float.MaxValue);
            this.currentTriggerTime = 0f;
            
            this.isInstantTriggerTimeReset = false;
            this.onUpdated = new UnityEvent<float>();


            this.AddOnDisabledListener(() => {
                this.currentTriggerTime = 0f;
                this.onUpdated.Invoke(currentTriggerTime);
            });

            this.AddOnResetedListener(() => {
                this.currentTriggerTime = 0f;
                this.onUpdated.Invoke(currentTriggerTime);
            });
        }

        public TimedHoldInputAction(string actionName, float triggerTime, bool isInstantTriggerTimeReset, HoldInput input) : base(actionName, input)
        {
            this.triggerTime = Mathf.Clamp(triggerTime, Mathf.Epsilon, float.MaxValue);
            this.currentTriggerTime = 0f;
          
            this.isInstantTriggerTimeReset = isInstantTriggerTimeReset;
            this.onUpdated                 = new UnityEvent<float>();

            this.AddOnDisabledListener(() => {
                this.currentTriggerTime = 0f;
                this.onUpdated.Invoke(currentTriggerTime);
            });

            this.AddOnResetedListener(() => {
                this.currentTriggerTime = 0f;
                this.onUpdated.Invoke(currentTriggerTime);
            });
        }

        public TimedHoldInputAction(string actionName, float triggerTime, bool isInstantTriggerTimeReset, OverrideableHoldInput input) : base(actionName, input)
        {
            this.triggerTime = Mathf.Clamp(triggerTime, 0f, float.MaxValue);
            this.currentTriggerTime = 0f;
           
            this.isInstantTriggerTimeReset = isInstantTriggerTimeReset;
            this.onUpdated                 = new UnityEvent<float>();


            this.AddOnDisabledListener(() => {
                this.currentTriggerTime = 0f;
                this.onUpdated.Invoke(currentTriggerTime);
            });

            this.AddOnResetedListener(() => {
                this.currentTriggerTime = 0f;
                this.onUpdated.Invoke(currentTriggerTime);
            });
        }

        public TimedHoldInputAction(string actionName, float triggerTime,  HoldInput[] inputs) : base(actionName, inputs)
        {
            this.triggerTime = Mathf.Clamp(triggerTime, Mathf.Epsilon, float.MaxValue);
            this.currentTriggerTime = 0f;
          
            this.isInstantTriggerTimeReset = false;
            this.onUpdated                 = new UnityEvent<float>();

            this.AddOnDisabledListener(() => {
                this.currentTriggerTime = 0f;
                this.onUpdated.Invoke(currentTriggerTime);
            });

            this.AddOnResetedListener(() => {
                this.currentTriggerTime = 0f;
                this.onUpdated.Invoke(currentTriggerTime);
            });
        }

        public TimedHoldInputAction(string actionName, float triggerTime, OverrideableHoldInput[] inputs) : base(actionName, inputs)
        {
            this.triggerTime = Mathf.Clamp(triggerTime, 0f, float.MaxValue);
            this.currentTriggerTime = 0f;
            
            this.isInstantTriggerTimeReset = false;
            this.onUpdated = new UnityEvent<float>();
           
            this.AddOnDisabledListener(() => {
                this.currentTriggerTime = 0f;
                this.onUpdated.Invoke(currentTriggerTime);
            });

            this.AddOnResetedListener(() => {
                this.currentTriggerTime = 0f;
                this.onUpdated.Invoke(currentTriggerTime);
            });
        }

        public TimedHoldInputAction(string actionName, float triggerTime, bool isInstantTriggerTimeReset, HoldInput[] inputs) : base(actionName, inputs)
        {
            this.triggerTime = Mathf.Clamp(triggerTime, Mathf.Epsilon, float.MaxValue);
            this.currentTriggerTime = 0f;
           
            this.isInstantTriggerTimeReset = false;
            this.onUpdated = new UnityEvent<float>();
           
            this.AddOnDisabledListener(() => {
                this.currentTriggerTime = 0f;
                this.onUpdated.Invoke(currentTriggerTime);
            });

            this.AddOnResetedListener(() => {
                this.currentTriggerTime = 0f;
                this.onUpdated.Invoke(currentTriggerTime);
            });
        }

        public TimedHoldInputAction(string actionName, float triggerTime, bool isInstantTriggerTimeReset, OverrideableHoldInput[] inputs) : base(actionName, inputs)
        {
            this.triggerTime             = Mathf.Clamp(triggerTime, 0f, float.MaxValue);
            this.currentTriggerTime      = 0f;
         
            this.isInstantTriggerTimeReset = isInstantTriggerTimeReset;
            this.onUpdated = new UnityEvent<float>();

            this.AddOnDisabledListener(() => {
                this.currentTriggerTime = 0f;
            });

            this.AddOnResetedListener(() => {
                this.currentTriggerTime = 0f;
            });
        }


        public override void Update()
        {
            if (IsEnabled) {
                UpdateTriggerTime();
            }

            base.Update();
        }

        private void UpdateTriggerTime()
        {
            if (base.CanExecute()) {
                currentTriggerTime = Mathf.Clamp(Time.deltaTime + currentTriggerTime, 0f, triggerTime);
            }
            else
            {
                if (isInstantTriggerTimeReset)
                    currentTriggerTime = 0f;
                else
                    currentTriggerTime = Mathf.Clamp(currentTriggerTime - Time.deltaTime, 0f, triggerTime);
            }

            onUpdated?.Invoke(Mathf.Clamp01(currentTriggerTime / triggerTime));
        }
    }
}
