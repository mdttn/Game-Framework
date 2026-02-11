using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.StateMachines.States
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class MovementSoundModule : MovementStateModule {
        private AudioSource source;
        private float currentMinPitch , currentMaxPitch; 
        private float currentMinVolume, currentMaxVolume;

        protected override void Awake()
        {
            base.Awake();
            source = GetComponent<AudioSource>();
        }


        public void SetPitch(float pitch) {
            if(source != null) source.pitch = pitch;
        }

        public void SetMinPitch(float pitch) {
            currentMinPitch = pitch;
        }

        public void SetMaxPitch(float pitch) {
            currentMaxPitch = pitch;
        }

        public void SetMinVolume(float volumr)
        {
            currentMinVolume = volumr;
        }

        public void SetMaxVolume(float volume)
        {
            currentMaxVolume = volume;
        }

        

        public void UpdateMinPitch(float minPitch, float updateSpeed) {
            currentMinPitch = Mathf.Lerp(currentMinPitch, minPitch, Time.deltaTime * updateSpeed);
        }

        public void UpdateMaxPitch(float maxPitch, float updateSpeed) {
            currentMaxPitch = Mathf.Lerp(currentMaxPitch, maxPitch, Time.deltaTime * updateSpeed);
        }

        public void UpdateMinVolume(float minVolume, float updateSpeed)
        {
            currentMinVolume = Mathf.Lerp(currentMinVolume, minVolume, Time.deltaTime * updateSpeed);
        }

        public void UpdateMaxVolume(float maxVolume, float updateSpeed)
        {
            currentMaxVolume = Mathf.Lerp(currentMaxVolume, maxVolume, Time.deltaTime * updateSpeed);
        }


        public void RandomizePitch() {
            RandomizePitch(currentMinPitch, currentMaxPitch);
        }

        public void RandomizePitch(float minPitch, float maxPitch) {
            if(source != null) source.pitch = Random.Range(minPitch, maxPitch);
        }

        public void RandomizeVolume() {
            RandomizeVolume(currentMinVolume, currentMaxVolume);
        }

        public void RandomizeVolume(float minVolume, float maxVolume) {
            if (source != null) source.volume = Random.Range(minVolume, maxVolume);
        }

        protected void Play(AudioClip[] clips)
        {
            if (clips == null || clips.Length == 0) return;
            Play(clips[Random.Range(0, clips.Length)]);
        }

        private void Play(AudioClip clip) {
            if(source == null) return;
            source.clip = clip;
            source.Play();
        }

        protected override string GetModuleName()  {
            return "Movement Sound Module";
        }
    }
}