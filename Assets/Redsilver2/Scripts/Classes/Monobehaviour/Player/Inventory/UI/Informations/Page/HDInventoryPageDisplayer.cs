using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class HDInventoryPageDisplayer : InventoryPageDisplayer
    {
        private TextMeshProUGUI displayer;

        protected sealed override void Awake()
        {
            base.Awake();
            displayer = GetComponent<TextMeshProUGUI>();
            displayer.text = string.Empty;
        }

        protected sealed override void DisplayMessage(string message)
        {
            if(displayer != null) {
                displayer.text = message;
            }
        }
    }
}
