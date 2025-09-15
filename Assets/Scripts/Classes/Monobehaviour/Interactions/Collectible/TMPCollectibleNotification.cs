using System.Collections;
using TMPro;

namespace RedSilver2.Framework.Interactions.Collectibles
{
    public abstract class TMPCollectibleNotification : CollectibleNotification
    {
        private TextMeshProUGUI nameDisplayer;

        protected sealed override void Awake()
        {
            base.Awake();
            nameDisplayer = GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}
