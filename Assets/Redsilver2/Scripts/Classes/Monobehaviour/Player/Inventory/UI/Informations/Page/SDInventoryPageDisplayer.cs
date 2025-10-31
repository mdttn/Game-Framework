using UnityEngine;
using UnityEngine.UI;


namespace RedSilver2.Framework.Player.Inventories.UI
{
    [RequireComponent(typeof(Text))]
    public class SDInventoryPageDisplayer : InventoryPageDisplayer
    {
        private Text displayer;

        protected sealed override void Awake()
        {
            base.Awake();
            displayer = GetComponent<Text>();
            displayer.text = string.Empty;
        }

        protected sealed override void DisplayMessage(string message)
        {
            if (displayer != null)
            {
                displayer.text = message;
            }
        }
    }
}