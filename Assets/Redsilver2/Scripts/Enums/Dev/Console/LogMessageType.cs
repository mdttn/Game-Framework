using UnityEngine;

namespace RedSilver2.Framework.Dev
{
    public abstract partial class DevConsole : MonoBehaviour
    {
        public enum LogMessageType 
        {
            All,
            Log,
            Warning,
            Error
        }
    }
}
