using UnityEngine;

namespace RedSilver2.Framework.Dev
{
    public abstract partial class DevConsole : MonoBehaviour
    {

        public enum DevConsoleArgumentType 
        {
            None,
            Boolean,
            String,
            Char,
            Int,
            UInt,
            Float,
            Double,
            Long
        }
    }
}
