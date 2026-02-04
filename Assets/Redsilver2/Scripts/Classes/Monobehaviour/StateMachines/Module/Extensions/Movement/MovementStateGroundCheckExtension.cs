using RedSilver2.Framework.StateMachines.States.Extensions;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public class MovementStateGroundCheckExtension : MovementStateTransitionExtension {
        [SerializeField] private float groundCheckRange;
       
        private bool    isGrounded;
        private string  groundTag;
        private Vector3 hitPosition;

        public float  GroundCheckRange => groundCheckRange;
        public bool   IsGrounded       => isGrounded;
        public string GroundTag        => groundTag;
        public Vector3 HitPosition     => hitPosition;


        private void Update() {

            Debug.Log(stateMachine);

            if (stateMachine == null || stateMachine.Controller == null)
                return;

            isGrounded = GetIsGrounded(stateMachine.Controller.transform, groundCheckRange, 
                                       out hitPosition, out groundTag);
        }

        protected bool GetIsGrounded(Transform transform, float checkRange, out Vector3 hitPosition, out string groundTag) {
            groundTag = string.Empty;
            hitPosition = Vector3.zero;
            if (transform == null) return false;

            if(Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo, checkRange, ~transform.gameObject.layer)) {
                return GetIsGrounded(hitInfo, out hitPosition, out groundTag);
            }

            return false;
        }

        protected bool GetIsGrounded(RaycastHit hit, out Vector3 hitPosition, out string groundTag) {
            groundTag = string.Empty;
            hitPosition = Vector3.zero;

            if(hit.collider == null) return false;

            GameObject gameObject = hit.collider.gameObject;

            if(gameObject.layer == GameManager.GetGroundLayer()) {
                groundTag = gameObject.tag;
                hitPosition = hit.point;
                return true;
            }

            return false;
        }

        protected sealed override MovementStateType[] GetInclusiveStates() {
            return new MovementStateType[] {
                MovementStateType.Walk  , MovementStateType.Run , MovementStateType.Fall, MovementStateType.Land,
                MovementStateType.Crouch, MovementStateType.Idol, MovementStateType.Jump
            };
        }

        protected override MovementStateType[] GetValidResultStateTypes() {
            return new MovementStateType[] {
                MovementStateType.Walk  , MovementStateType.Run , MovementStateType.Land,
                MovementStateType.Crouch, MovementStateType.Idol, MovementStateType.Jump
            };
        }

        public sealed override bool Validate() {
            return isGrounded;
        }


        protected sealed override string GetExtensionName() {
            return "Ground Check";
        }

        protected sealed override void OnStateExtensionAdded(StateExtension extension) {

        }
    }
}
