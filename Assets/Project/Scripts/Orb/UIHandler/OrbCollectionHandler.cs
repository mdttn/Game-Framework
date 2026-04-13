using System.Collections;
using TMPro;
using UnityEngine;

public class OrbCollectionHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI orbCountDisplayer;
    [SerializeField] private AudioClip[] collectedClips;

    [Space]
    [SerializeField] private float maxPositionOffsetX;
    [SerializeField] private float shakeUpdateSpeed;

    [Space]
    [SerializeField] private float shakeUpdateTime;


    private Vector3 originalPosition;

    private IEnumerator updateOrbCountDisplayer;
    
    private AudioSource source;
    private CustomGameManager gameManager;

#if UNITY_EDITOR
    private void OnValidate()
    {
        shakeUpdateTime    = Mathf.Abs(shakeUpdateTime);
        shakeUpdateSpeed   = Mathf.Abs(shakeUpdateSpeed);
        maxPositionOffsetX = Mathf.Abs(maxPositionOffsetX);
    }

#endif

    private void Awake() {
        gameManager = CustomGameManager.GetInstance();
        SetOrbCountDisplayerText(string.Empty);
       
        if (orbCountDisplayer != null) originalPosition = orbCountDisplayer.transform.localPosition;
        source = GetComponent<AudioSource>();
        
        source.playOnAwake = false;
        source.loop = false;
    }

    private void Start()
    {
        gameManager?.AddOnOrbCollectedListener(OnOrbCollected);    
    }

    private void OnEnable()
    {
        gameManager?.AddOnOrbCollectedListener(OnOrbCollected);
    }

    private void OnDisable()
    {
        gameManager?.RemoveOnOrbCollectedListener(OnOrbCollected);
        if(updateOrbCountDisplayer != null) StopCoroutine(updateOrbCountDisplayer);
        SetOrbCountDisplayerVisibility(false);
    }

    private void SetOrbCountDisplayerText(string text)
    {
       if(orbCountDisplayer != null)
           orbCountDisplayer.text = text;
    }

    private void OnOrbCollected(int orbsLeft)
    {
        if (orbsLeft <= 0) return;
        PlayCollectedSound();

        if(updateOrbCountDisplayer != null) 
            StopCoroutine(updateOrbCountDisplayer);

        updateOrbCountDisplayer = UpdateDisplayerCoroutine(orbsLeft);
        StartCoroutine(updateOrbCountDisplayer);
    }

    private void SetOrbCountDisplayerVisibility(bool isVisible)
    {
        if (orbCountDisplayer != null)
            orbCountDisplayer.enabled = isVisible;
    }

    private void UpdateDisplayerLocalPosition(Vector3 position)
    {
        if(orbCountDisplayer == null) return;
        Transform transform     = orbCountDisplayer.transform;
        transform.localPosition = position;
    }

    private IEnumerator UpdateDisplayerCoroutine(int count)
    {
        string message = GetDisplayMessage(count);
       
        SetOrbCountDisplayerText(message);
        SetOrbCountDisplayerVisibility(true);

        yield return StartCoroutine(UpdateDisplayerCoroutine());

        SetOrbCountDisplayerText(string.Empty);
        SetOrbCountDisplayerVisibility(false);
    }

    private IEnumerator UpdateDisplayerCoroutine()
    {
        float t = 0f;

        while (t < shakeUpdateTime) {
            UpdateDisplayerLocalPosition(GetUpdateDisplayPosition());
            t += Time.deltaTime;
            yield return null;
        }
    }

    private void PlayCollectedSound()
    {
        if(source == null || collectedClips == null || collectedClips.Length == 0) return;
        var clip = collectedClips[Random.Range(0, collectedClips.Length)];
       
        if(clip == null) return;
        
        source.clip = clip;
        source.volume = Random.Range(0.8f, 1f);
        
        source.pitch = Random.Range(0.8f, 1f);
        source?.Play();
    }

    private Vector3 GetUpdateDisplayPosition()
    {
        float sin = Mathf.Abs(Mathf.Sin(Time.time * shakeUpdateSpeed));
        float minPositionX = originalPosition.x - maxPositionOffsetX;
        float maxPositionX = originalPosition.x + maxPositionOffsetX;

        return Vector3.left  * Mathf.Lerp(minPositionX, maxPositionX, sin) +
               Vector3.up    * originalPosition.y + 
               Vector3.right * originalPosition.z;
    }

    protected virtual string GetDisplayMessage(int count)
    {
        if(count < 0) { return string.Empty;  }
        return count.ToString();
    }
}
