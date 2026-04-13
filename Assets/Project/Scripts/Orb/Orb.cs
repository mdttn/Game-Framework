using UnityEngine;
using UnityEngine.Events;

public class Orb : MonoBehaviour
{
    private UnityEvent onCollected;
  
    private Collider _collider;
    private CustomGameManager gameManager;

    private void Awake() {
        gameManager = CustomGameManager.GetInstance();
        _collider = GetComponent<Collider>();

        gameManager?.AddOrb(this);
        onCollected = new UnityEvent();

        if(_collider != null) _collider.isTrigger = true;

       onCollected?.AddListener(() => {
           gameObject.SetActive(false);
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
