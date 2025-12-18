using UnityEngine;

namespace RedSilver2.Framework.Settings
{
    public abstract class FramerateUI : MonoBehaviour
    {
         protected readonly FramerateSetting framerateSetting = FramerateSetting.Instance;
         protected abstract void OnEnable();
         protected abstract void OnDisable();
    }
}
