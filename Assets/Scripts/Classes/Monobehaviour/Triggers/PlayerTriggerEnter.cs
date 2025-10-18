using UnityEngine;
using UnityEngine.Events;


namespace RedSilver2.Framework.Player {
    public class PlayerTriggerEnter : MonoBehaviour {

        [SerializeField] private UnityEvent onEnterTrigger;

        private void Awake() {
            if (gameObject.TryGetComponent(out Collider collider))
                collider.isTrigger = true;
        }

        public void AddOnTriggerEnterListener(UnityAction action) {
            if(onEnterTrigger != null && action != null) {
                onEnterTrigger.AddListener(action);
            }
        }

        public void RemoveOnTriggerEnterListener(UnityAction action)
        {
            if (onEnterTrigger != null && action != null) {
                onEnterTrigger.RemoveListener(action);
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent(out PlayerController controller))
                if (PlayerController.Current == controller)
                    onEnterTrigger.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.TryGetComponent(out PlayerController controller))
                if (PlayerController.Current == controller)
                    onEnterTrigger.Invoke();
        }
    }
}
