using RedSilver2.Framework.Stats;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class HurtSFX : MonoBehaviour {
    [SerializeField] private AudioClip[] clips;

    private Health health;
    private AudioSource source;
    private float currentProgress;

    private void Start() {
       health =  transform.parent != null ? transform.parent.GetComponentInChildren<Health>()
                                          : GetComponentInChildren<Health>();     

        source = GetComponent<AudioSource>();
        currentProgress = health == null ? 0f : health.Progress;

        health?.AddOnProgressChangedListener(OnProgressChanged);
    }

    private void OnDisable()
    {
        health?.RemoveOnProgressChangedListener(OnProgressChanged);
    }

    private void OnEnable()
    {
        health?.AddOnProgressChangedListener(OnProgressChanged);
    }

    private void OnProgressChanged(float progress) {
        if(progress < currentProgress){
            var clip = GetRandomClip();
            if(clip != null) source?.PlayOneShot(clip);
        }
    }

    private AudioClip GetRandomClip()
    {
        if(clips == null || clips.Length == 0) return null;
        return clips[Random.Range(0, clips.Length)];
    }
}
