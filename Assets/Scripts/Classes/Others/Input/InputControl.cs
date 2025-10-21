using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public abstract class InputControl
    {
        private Sprite icon;
        public Sprite Icon => icon;

        private InputControl() { }

        public InputControl(Sprite icon) {
            this.icon = icon;
        }

        public void OverrideIcon(Sprite icon) {
            this.icon = icon;
        }

        public abstract bool GetKey();
        public abstract bool GetKeyDown();
        public abstract bool GetKeyUp();
    }
}
