using RedSilver2.Framework.StateMachines.States.Extensions;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [RequireComponent(typeof(MovementStateGroundCheckExtension))]
    public abstract class MovementStateJumpCheckExtension : MovementStateTransitionExtension
    {
        [SerializeField] private bool ignoreGroundedCondition;

        private bool isJumping;
        private IStateTransition groundStateTransition;

        public bool IsJumping => isJumping;
        public bool IgnoreGroundedCondition => ignoreGroundedCondition;


        protected override void Start() {
            base.Start();
            groundStateTransition = transform.root.GetComponentInChildren<MovementStateGroundCheckExtension>();
        }

        private void Update()
        {
            CheckJump(groundStateTransition, ignoreGroundedCondition, ref isJumping);
        }

        protected abstract void CheckJump(IStateTransition groundStateTransition, bool ignoreGroundedCondition, ref bool isJumping);

        protected sealed override void OnStateExtensionAdded(StateExtension stateExtension)
        {
            if (stateExtension is MovementStateGroundCheckExtension) { groundStateTransition = stateExtension as MovementStateGroundCheckExtension; }
        }


        public sealed override bool Validate() {
            return isJumping;
        }

        protected sealed override string GetExtensionName() {
            return "Jump Check";
        }

        protected override MovementStateType[] GetInclusiveStates()
        {
            return new MovementStateType[] {
               MovementStateType.Jump, MovementStateType.Walk, MovementStateType.Run,
            };
        }

        protected override MovementStateType[] GetValidResultStateTypes()
        {
            return new MovementStateType[] {
               MovementStateType.Jump
            };
        }
    }
}
