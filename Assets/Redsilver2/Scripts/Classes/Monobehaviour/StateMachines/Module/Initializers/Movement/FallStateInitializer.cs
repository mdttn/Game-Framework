using RedSilver2.Framework.StateMachines.States;
using UnityEngine.Events;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines
{
    [RequireComponent(typeof(LandStateInitializer))]
    public sealed class FallStateInitializer : MovementStateInitializer
    {
        [Space]
        [SerializeField] private float defaultFallSpeed;
        [SerializeField] private float defaultFallTransitiionSpeed;


        [Space]
        [SerializeField] private float fallSpeed;
        [SerializeField] private float fallTransitionSpeed;

        [Space]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float movementTransitionSpeed;

        private float currentFallSpeed;

        public float MovementTransitionSpeed    => movementTransitionSpeed;
        public float DefaultFallTransitionSpeed => defaultFallTransitiionSpeed;
        public float FallTransitionSpeed        => fallTransitionSpeed;
        public float DefaultFallSpeed           => defaultFallSpeed;
        public float FallSpeed                  => fallSpeed;
        public float CurrentFallSpeed           => currentFallSpeed;
        public float MoveSpeed                  => moveSpeed;

        #if UNITY_EDITOR

        private void OnValidate() {
            defaultFallSpeed = Mathf.Clamp(defaultFallSpeed, float.MinValue, 0f);
            fallSpeed        = Mathf.Clamp(fallSpeed       , float.MinValue, defaultFallSpeed);
            moveSpeed        = Mathf.Clamp(moveSpeed       , Mathf.Epsilon, float.MaxValue);
        }

#endif

        protected override void Start()
        {
            base.Start();
            currentFallSpeed = defaultFallSpeed;
        }

        public void SetFallSpeed(float fallSpeed)
        {
            this.currentFallSpeed = Mathf.Clamp(fallSpeed, float.MinValue, 0f);
        }

        public void SetJumpHeight(float jumpHeight)
        {
            this.currentFallSpeed = Mathf.Clamp(jumpHeight, 0f, float.MaxValue);
        }

        public void UpdateFallSpeed(float nextFallSpeed, float updateSpeed)
        {
            if (nextFallSpeed <= 0f)
                this.currentFallSpeed = Mathf.Lerp(currentFallSpeed, nextFallSpeed, Time.deltaTime * updateSpeed);
        }

        protected sealed override MovementState GetDefaultState(MovementStateMachine stateMachine)
        {
            if (stateMachine == null) return null;

            if (stateMachine.ContainsState(MovementStateType.Fall)) 
                return stateMachine.GetState(MovementStateType.Fall) as FallState;

            return new FallState(stateMachine, this);
        }

        protected sealed override string GetModuleName()
        {
            return "Fall State Initializer";
        }
    }
}
