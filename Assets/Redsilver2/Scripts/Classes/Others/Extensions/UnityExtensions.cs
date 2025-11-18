using System.Collections;
using UnityEngine;

public static class UnityExtensions
{
    public static float Set(this float current, float value)
    {
        current = value;
        return current;
    }
    public static bool HasReachedTarget(this float current, float target) => current >= target;

    public static void SetAlpha(this CanvasRenderer renderer, float progression, float currentAlpha, float alphaTarget) {
        progression = Mathf.Clamp01(progression);

        if (renderer != null)
        {
            if (progression >= 1f) renderer.SetAlpha(alphaTarget);
            else renderer.SetAlpha(Mathf.Lerp(currentAlpha, alphaTarget, progression));
        }
    }
    public static void SetAlpha(this CanvasRenderer renderer, float progression, float alphaTarget) {
        if (renderer != null) renderer.SetAlpha(progression, renderer.GetAlpha(), alphaTarget);
    }

    public static IEnumerator Fade(this CanvasRenderer renderer, float targetedAlpha, float duration) {
        float timeElpased = 0f, currentAlpha = renderer.GetAlpha();
        targetedAlpha = Mathf.Clamp01(targetedAlpha);

        while (timeElpased < duration) {
            timeElpased += Time.deltaTime;
            renderer.SetAlpha(Mathf.Lerp(currentAlpha, targetedAlpha, Mathf.Clamp01(timeElpased / duration)));
            yield return null;
        }

        renderer.SetAlpha(targetedAlpha);
    }
    public static IEnumerator Fade(this CanvasRenderer renderer, bool isVisible, float duration) {
        yield return renderer.Fade(isVisible ? 1f : 0f, duration);
    }
}
