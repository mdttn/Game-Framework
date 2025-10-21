
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{
    public class TimedHoldInteractionModule : InteractionModule
    {
        [Space]
        [SerializeField] private float interactionSpeed;
        [SerializeField] private float maxInteractionTime;

        [Space]
        [SerializeField] private float resetInteractionSpeed;
        [SerializeField] private float resetInteractionDelay;

        [Space]
        [SerializeField] private bool clearTimerValue         ;
        [SerializeField] private bool resetTimerOnInputRelease;

        [Space]
        [SerializeField] private UnityEvent        onInputPressed;
        [SerializeField] private UnityEvent        onInputReleased;
        [SerializeField] private UnityEvent<float> onInteract;

        private bool  isResettingInteractionTimer;
        private float interactionTime;

        private IEnumerator resetTimeCoroutine;

        public  float InteractionTime             => interactionTime;
        public  bool  IsResettingInteractionTimer => isResettingInteractionTimer;

        private void Start() {
            isResettingInteractionTimer = false;
            interactionTime             = 0;
            resetTimeCoroutine          = StartResetInteractionTime();

            AddOnInputPressedListener(OnInputPressed);
            AddOnInputReleasedListener(OnInputReleased);
        }

        private void OnDisable() {
           if(resetTimeCoroutine != null) StopCoroutine(resetTimeCoroutine);
        }

        public void AddOnInteractListener(UnityAction<float> action) 
        {
            if(onInteract != null && action != null)
                onInteract.AddListener(action);
        }
        public void RemoveOnInteractListener(UnityAction<float> action)
        {
            if (onInteract != null && action != null)
                onInteract.RemoveListener(action);
        }

        public void AddOnInputReleasedListener(UnityAction action)
        {
            if (onInputReleased != null && action != null)
                onInputReleased.AddListener(action);
        }
        public void RemoveOnInputReleasedListener(UnityAction action)
        {
            if (onInputReleased != null && action != null)
                onInputReleased.RemoveListener(action);
        }

        public void AddOnInputPressedListener(UnityAction action)
        {
            if (onInputReleased != null && action != null)
                onInputReleased.AddListener(action);
        }
        public void RemoveOnInputPressedListener(UnityAction action)
        {
            if (onInputReleased != null && action != null)
                onInputReleased.RemoveListener(action);
        }

        public sealed override void Interact(InteractionHandler handler) {
            if (handler != null && enabled)
            {
               if      (handler.IsInputPressed)  UpdateInputPress();
               else if (handler.IsInputReleased) Release();
                else if (handler.IsInputHeld)    UpdateInputHold();
            }
        }

        private void UpdateInputPress()
        {
            if (onInputPressed != null) onInputPressed.Invoke();
        }

        private void UpdateInputHold()
        {
            interactionTime += Time.deltaTime * interactionSpeed;
            if (interactionTime >= maxInteractionTime) interactionTime = maxInteractionTime;
            if (onInteract != null) onInteract.Invoke(interactionTime / maxInteractionTime);
        }

        private void OnInputPressed() {
            isResettingInteractionTimer = false;
            if (resetTimeCoroutine != null) StopCoroutine(resetTimeCoroutine);
        }

        private void OnInputReleased()  
        {
            if (resetTimerOnInputRelease && enabled && resetInteractionSpeed > 0) 
                StartCoroutine(resetTimeCoroutine);
            else if (clearTimerValue) {
                interactionTime = 0f;
                onInteract.Invoke(interactionTime / maxInteractionTime);
            }
        }

        public void Release() {
            if(onInputReleased != null) onInputReleased.Invoke();
        }

        private IEnumerator StartResetInteractionTime() {
            float t = 0f;

            while (t < resetInteractionDelay) {
                t += Time.deltaTime;
                yield return null;
            }

            isResettingInteractionTimer = true;
            yield return StartCoroutine(ResetInteractionTime());   
        }

        private IEnumerator ResetInteractionTime() 
        {
            while(interactionTime > 0f){
                interactionTime -= Time.deltaTime * resetInteractionSpeed;
                if(onInteract != null && interactionTime > 0f) onInteract.Invoke(interactionTime / maxInteractionTime);
                yield return null;
            }

            interactionTime = 0f;
            isResettingInteractionTimer = false;

            if (onInteract != null) onInteract.Invoke(interactionTime / maxInteractionTime);
        }
    }
}
