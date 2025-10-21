namespace RedSilver2.Framework.Inputs
{
    public interface IOverridableSingleInput
    {
        void OverrideKey(KeyboardKey key);
        void OverrideKey(GamepadButton  key);
    }
}
