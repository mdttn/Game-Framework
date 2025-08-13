namespace RedSilver2.Framework.Inputs
{
    public enum GamepadKey
    {
        #region Action Buttons
        ButtonSouth, ButtonEast, ButtonNorth, ButtonWest,
        #endregion

        #region Shoulder Buttons
        LeftShoulder, RightShoulder,
        #endregion

        #region Triggers
        LeftTrigger, RightTrigger,
        #endregion

        #region Menu / System Buttons
        Select, Start,
        #endregion

        #region Thumbsticks
        LeftStick     , RightStick     , LeftStickAxisY , 
        LeftStickAxisX, RightStickAxisY, RightStickAxisX,
        LeftStickPress, RightStickPress,
        #endregion

        #region D-Pad
        DpadUp, DpadDown, DpadLeft, DpadRight
        #endregion
    }
}
