using UnityEngine;

namespace RedSilver2.Framework.Inputs
{
    public sealed partial class KeyboardVector2Input : Vector2Input
    {
        public struct Vector2Keyboard
        {
            public KeyboardKey Up { get; private set; }
            public KeyboardKey Down { get; private set; }
            public KeyboardKey Left { get; private set; }
            public KeyboardKey Right { get; private set; }

            public Vector2Keyboard(KeyboardKey up, KeyboardKey down, KeyboardKey left, KeyboardKey right) 
            {
                this.Up = up;
                this.Down = down;
                this.Left = left;
                this.Right = right;
            }

            public void OverrideUp   (KeyboardKey key) { Up = key; }
            public void OverrideDown (KeyboardKey key) { Down = key; }
            public void OverrideLeft (KeyboardKey key) { Left = key; }
            public void OverrideRight(KeyboardKey key) { Right = key; }
        }
    }
}
