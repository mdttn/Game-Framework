using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RedSilver2.Framework.Stats.Extensions
{
    public class PlayerBloodVFX : MonoBehaviour
    {
        [Space]
        [SerializeField] private Image bloodVFXDisplayer;
        [SerializeField] private float bloodVFXUpdateTime = 0.5f;

        private float currentVFXUpdateTime;
        private Color originalVFXColor;

        private float currentVFXAlpha;
        private float desiredVFXAlpha;

        private PlayerHealth health;

        private void Start() {
            health = GetComponent<PlayerHealth>();
            health?.AddOnProgressChangedListener(OnProgressChanged);
           
            StartCoroutine(VFXCoroutine());
            if (health != null) OnProgressChanged(health.Progress);

            originalVFXColor = Color.white;
        }

        private void OnProgressChanged(float progress)
        {
            currentVFXUpdateTime = 0f;
            desiredVFXAlpha  = bloodVFXDisplayer == null ? 0f : Mathf.Lerp(1f, 0f, progress);
            currentVFXAlpha  = bloodVFXDisplayer == null ? 0f : bloodVFXDisplayer.color.a;
        }

        private IEnumerator VFXCoroutine()
        {
            while (true)
            {
                currentVFXUpdateTime = Mathf.Clamp(Time.deltaTime + currentVFXUpdateTime, 0f, bloodVFXUpdateTime);
                float alpha = Mathf.Lerp(currentVFXAlpha, desiredVFXAlpha, Mathf.Clamp01(currentVFXUpdateTime / bloodVFXUpdateTime));

                if (bloodVFXDisplayer != null)
                   bloodVFXDisplayer.color = new Color(originalVFXColor.r, originalVFXColor.g, originalVFXColor.b, alpha);
                yield return null;
            }
        }
    }
}
