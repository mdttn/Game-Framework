using UnityEngine;

namespace RedSilver2.Framework.Player
{
    public abstract class PlayerExtensionModule : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log(transform.root);
            Setup(transform.root.GetComponent<PlayerController>());
        }

        private void Setup(PlayerController owner)
        {
            if (owner != null) Setup(owner.StateMachine);
        }

        private void Setup(PlayerStateMachine stateMachine)
        {
            if (stateMachine != null)
            {
                PlayerStateMachine.PlayerExtension extension = GetExtension(stateMachine);

                if (extension != null && enabled)
                {
                    stateMachine.AddExtension(this);
                    extension.Enable();
                }

                SetExtension(extension);
            }
        }

        private void OnDisable()
        {
            PlayerStateMachine.PlayerExtension extension = GetExtension();
            if (extension != null) extension.Disable(); 
        }
        private void OnEnable()
        {
            PlayerStateMachine.PlayerExtension extension = GetExtension();
            if (extension != null) extension.Enable();
        }

        protected abstract void SetExtension(PlayerStateMachine.PlayerExtension extension);
        protected abstract PlayerStateMachine.PlayerExtension GetExtension(PlayerStateMachine owner);

        public abstract PlayerStateMachine.PlayerExtension GetExtension();
    }
}
