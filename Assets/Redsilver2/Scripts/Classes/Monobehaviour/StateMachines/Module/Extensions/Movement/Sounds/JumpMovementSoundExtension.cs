using UnityEngine;


namespace RedSilver2.Framework.StateMachines.States
{
    public sealed class JumpMovementSoundExtension : MovementSoundModule
    {
        [Space]
        [SerializeField] private AudioClip[] clips;

        [Space]
        [SerializeField] private float minPitch;
        [SerializeField] private float maxPitch;

        [Space]
        [SerializeField] private float minVolume;
        [SerializeField] private float maxVolume;

        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            minPitch = Mathf.Clamp(minPitch, 0.0f    , float.MaxValue);
            maxPitch = Mathf.Clamp(maxPitch, minPitch, float.MaxValue);

            minVolume = Mathf.Clamp(minVolume, 0.0f,      float.MaxValue);
            maxVolume = Mathf.Clamp(maxVolume, minVolume, float.MaxValue);
        }
        #endif



        protected sealed override void Start() {
            base.Start();

            if(stateMachine != null)  {
                foreach(State state in stateMachine.GetStates())
                     OnStateAdded(state);   
            }
        }


        protected sealed override void OnStateAdded(MovementState state)
        {
            if(state is JumpState) {
                state?.AddOnStateEnteredListener(OnStateEntered);
            }
        }

        protected sealed override void OnStateRemoved(MovementState state)
        {
            if (state is JumpState) {
                state?.RemoveOnStateEnteredListener(OnStateEntered);
            }
        }

        private void OnStateEntered() {
            SetMaxPitch(maxPitch);
            SetMinPitch(minPitch);

            SetMinVolume(minVolume);
            SetMaxVolume(maxVolume);

            RandomizePitch();
            RandomizeVolume();

            Play(clips);
        }
    }
}