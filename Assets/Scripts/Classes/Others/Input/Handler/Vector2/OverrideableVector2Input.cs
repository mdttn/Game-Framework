namespace RedSilver2.Framework.Inputs
{
    public sealed class OverrideableVector2Input : Vector2Input
    {

        public OverrideableVector2Input(string name) : base(name)
        {
        }

        public OverrideableVector2Input(string name, bool useLeftGamepadStick) : base(name, useLeftGamepadStick)
        {
        }

        public void OverrideStick(bool useLeftGamepadStick) => this.useLeftGamepadStick = useLeftGamepadStick;
    }
}
