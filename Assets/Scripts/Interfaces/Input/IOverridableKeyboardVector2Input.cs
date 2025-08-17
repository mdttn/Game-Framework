using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public interface IOverridableKeyboardVector2Input 
    {
        void OverrideUpKey(KeyboardKey key);
        void OverrideDownKey(KeyboardKey key);
        void OverrideLeftKey(KeyboardKey key);
        void OverrideRightKey(KeyboardKey key);

        public string GetUpPath();
        public string GetDownPath();
        public string GetLeftPath();
        public string GetRightPath();
    }
}
