using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    public class MovementGroundCheck3D : MovementGroundCheckExtension
    {

        protected sealed override void OnUpdate(float groundCheckRange, ref bool isGrounded, ref string groundTag, ref Vector3 hitPosition)
        {
            if (stateMachine == null || stateMachine.Controller == null)
                return;

            Transform transform = stateMachine.Controller.transform;


            isGrounded = GetIsGrounded(transform, groundCheckRange,
                                       out groundTag, out hitPosition);
        }

        private bool GetIsGrounded(Transform transform, float checkRange, out string groundTag, out Vector3 hitPosition)
        {
            hitPosition = Vector3.zero;
            groundTag = string.Empty;
            if (transform == null) return false;

            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo, checkRange, ~transform.gameObject.layer))
                return GetIsGrounded(hitInfo, out groundTag, out hitPosition);

            return false;
        }

        private bool GetIsGrounded(RaycastHit hit, out string groundTag, out Vector3 hitPosition)
        {
            hitPosition = Vector3.zero;
            groundTag = string.Empty;
            if (hit.collider == null) return false;

            GameObject gameObject = hit.collider.gameObject;

            if (gameObject.layer == GameManager.GetGroundLayer()) {
                hitPosition = hit.point;
                groundTag = gameObject.tag;
                return true;
            }

            return false;
        }
    }

}
