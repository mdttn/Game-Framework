using UnityEngine;

namespace RedSilver2.Framework.Interactions.Items
{
    public abstract class LightSourceItem : UpdateableItem
    {
        private Light _light;
        public Light Light => _light;

        protected override void Awake()
        {
            base.Awake();
            _light = GetComponentInChildren<Light>(true);

            if (_light == null) {

            }
        }
    }
}
