using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.StateMachines.States
{
    public abstract class GroundMovementSoundExtension : MovementSoundModule
    {
        [Space]
        [SerializeField] private GroundAudioData[] audioDatas;
        private MovementGroundCheckExtension groundCheckExtension;

        protected override void Awake()
        {
            base.Awake();
            groundCheckExtension = transform.root.GetComponentInChildren<MovementGroundCheckExtension>();
            stateMachine?.AddOnStateModuleAddedListener(OnStateModuleAdded);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            stateMachine?.RemoveOnStateModuleAddedListener(OnStateModuleAdded);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            stateMachine?.AddOnStateModuleAddedListener(OnStateModuleAdded);
        }

        protected void Play(string groundTag) {
            if (!string.IsNullOrEmpty(groundTag)){
                var results =  audioDatas.Where(x => x.groundTag.ToLower().Equals(groundTag.ToLower()));
                if (results.Count() > 0) Play(results.First().clips);
            }
        }

        private void OnStateModuleAdded(StateModule module)
        {
            if(module is MovementGroundCheckExtension) { module = groundCheckExtension; }
        }

        protected bool IsGrounded() {
            if (groundCheckExtension == null) return false;
            return groundCheckExtension.IsGrounded;
        }

        protected string GroundTag()
        {
            if(groundCheckExtension == null) return string.Empty;
            return groundCheckExtension.GroundTag;
        }

        protected sealed override string GetModuleName()
        {
            return "Movement Footstep Sound Extension";
        }
    }
}
