using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Orb : MonoBehaviour
{
    [SerializeField] private UnityEvent onCollected;
  
    private Collider _collider;
    private MeshRenderer _renderer;
    private CustomGameManager gameManager;

    private AudioSource audioSource;

    private void Awake() {
        gameManager = CustomGameManager.GetInstance();

        _collider   = GetComponent<Collider>();   
        _renderer   = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();

       gameManager?.AddOrb(this);

        onCollected?.AddListener(() => {
            if (_collider != null) _collider.enabled = false;
            if (_renderer != null) _renderer.enabled = false;
        });
    }

    public void AddOnCollectedListener(UnityAction action)
    {
        if (action != null) {
            onCollected?.AddListener(action);
        }
    }

    public void RemoveOnCollectedListener(UnityAction action)
    {
        if(action != null) { 
           onCollected?.RemoveListener(action); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null || gameManager == null) return;

        if (other.tag.ToLower().Equals("player")) {
            gameManager.CollectOrb(this, out bool isCollected);    
            if (isCollected) onCollected?.Invoke();
        }
    }
}
