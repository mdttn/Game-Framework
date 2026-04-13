using RedSilver2.Framework.StateMachines.Controllers;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Extensions
{
    [RequireComponent(typeof(AudioSource))]

    public class MovementSoundExtension : MovementStateExtension
    {
        [SerializeField] private MovementSoundData[] movementSoundDatas;
        [SerializeField] private float soundTriggerTime;

        private AudioSource source;
        private MovementSoundData currentData;


        private float currentMoveSoundTriggerTime;

        protected sealed override void Start() {
            base.Start();
            source = GetComponent<AudioSource>();

            if (movementController != null)
            {
                MovementStateMachine stateMachine = movementController.GetStateMachine() as MovementStateMachine;
                stateMachine?.AddOnUpdateListener(OnUpdate);
                stateMachine?.AddOnGroundTagChangedListener(OnGroundTagChanged);
            }
        }

        private void OnUpdate() {
            if (movementController == null) return;

            if (movementController.IsMoving() && movementController.IsGrounded()) {
                currentMoveSoundTriggerTime = Mathf.Clamp(Time.deltaTime + currentMoveSoundTriggerTime, 0f, soundTriggerTime);

                if(currentMoveSoundTriggerTime >= soundTriggerTime) {
                    currentMoveSoundTriggerTime = 0f;
                    TriggerMoveSoundUpdate();
                }
            }
        }

        private void OnGroundTagChanged(string value) {
            currentData = GetMovementSoundData(value);
            currentMoveSoundTriggerTime = soundTriggerTime;
        }

        private void TriggerMoveSoundUpdate()
        {
            if(source == null || currentData == null) return;
            source.pitch  = Random.Range(0.6f, 0.8f);
            source.volume = Random.Range(0.8f, 1f);

            source.clip = GetRandomAudioClip(currentData.moveAudioClips);
            source.Play();
        }

        private AudioClip GetRandomAudioClip(AudioClip[] clips) {
            if(clips == null || clips.Length == 0) return null;
            return clips[Random.Range(0, clips.Length)];
        }

        private MovementSoundData GetMovementSoundData(string groundTag) {
            var results = movementSoundDatas.Where(x => x != null)
                                            .Where(x => x.groundTag.ToLower().Equals(groundTag));

            return results.Count() > 0 ? results.First() : null;    
        }

        protected override void OnDisable() {

            if (movementController != null)
            {
                MovementStateMachine stateMachine = movementController.GetStateMachine() as MovementStateMachine;
                stateMachine?.RemoveOnUpdateListener(OnUpdate);
                stateMachine?.RemoveOnGroundTagChangedListener(OnGroundTagChanged);
            }
        }

        protected override void OnEnable() {
            if (movementController != null)
            {
                MovementStateMachine stateMachine = movementController.GetStateMachine() as MovementStateMachine;
                stateMachine?.AddOnUpdateListener(OnUpdate);
                stateMachine?.AddOnGroundTagChangedListener(OnGroundTagChanged);
            }
        }

        protected override void OnStateMachineAdded(MovementStateMachine stateMachine)
        {
            throw new System.NotImplementedException();
        }
    }
}
