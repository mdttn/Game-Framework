

namespace RedSilver2.Framework.Inputs
{
    public enum KeyboardKey
    {
        #region Alphanumeric keys

        #region Letters
        Q, W, E, R, T, Y, U, I, O, P,
        A, S, D, F, G, H, J, K, L,
        Z, X, C, V, B, N, M,
        #endregion

        #region Numbers
        Alpha0, Alpha1, Alpha2, Alpha3, Alpha4, Alpha5,
        Alpha6, Alpha7, Alpha8, Alpha9,
        #endregion

        #region Others
        Backquote, Minus, Equals, LeftBracket, RightBracket,
        Semicolon, Quote, Comma, Period, Slash,
        #endregion

        #endregion

        #region Modifier keys
        LeftShift, RightShift, LeftCtrl, RightCtrl,
        LeftAlt, RightAlt, LeftCommand, RightCommand,
        #endregion

        #region Arrow keys
         UpArrow, DownArrow, LeftArrow, RightArrow,
        #endregion

        #region Numpad keys

        #region Numbers
        Numpad0, Numpad1, Numpad2, Numpad3, Numpad4,
        Numpad5, Numpad6, Numpad7, Numpad8, Numpad9,
        #endregion

        #region Others
        NumpadPeriod, NumpadDivide, NumpadMultiply, NumpadMinus,
        NumpadPlus, NumpadEnter, NumpadEquals,
        #endregion

        #endregion

        #region Special keys
        Space    , Enter , Return, Tab , Escape,
        Backspace, Delete, Insert, Home, End   , PageUp, PageDown,
        #endregion

        #region Function keys
        F1, F2, F3, F4 , F5 , F6 ,
        F7, F8, F9, F10, F11, F12,
        #endregion

        #region Other Keys
         CapsLock, ScrollLock, NumLock, PrintScreen, Pause
        #endregion
    }
}
