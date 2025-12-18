using UnityEngine;
using UnityEngine.UI;


namespace RedSilver2.Framework.Settings
{
    public abstract class FramerateSelector : FramerateUI
    {
        [SerializeField] private Button next, previous;

        protected override void OnDisable()
        {
            if (Application.isPlaying) {
                next.onClick.RemoveAllListeners();
                previous.onClick.RemoveAllListeners();
            }
        }

        protected override void OnEnable()
        {
            if (Application.isPlaying && next != null && previous != null) {
                SetUI(next, previous);
            }
        }

        protected abstract void SetUI(Button next, Button previous);
    }
}
