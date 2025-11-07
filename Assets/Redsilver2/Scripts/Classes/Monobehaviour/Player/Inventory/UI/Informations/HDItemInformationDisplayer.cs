using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public abstract class HDItemInformationDisplayer : ItemInformationDisplayer {

        private TextMeshProUGUI displayer;
        
        protected sealed override void Start() 
        {
            displayer = GetComponent<TextMeshProUGUI>();
            if (displayer != null) displayer.text = string.Empty;
            base.Start();
        }

        protected sealed override void DisplayItemInformation(string message)
        {
            if (displayer != null) {
                displayer.text = message;
            }
        }

        protected override void OnTransitionsFinished()
        {
            if (displayer != null) displayer.enabled = true;
        }

        protected sealed override void OnTransitionsStarted()
        {
            if (displayer != null) displayer.enabled = false;
        }
    }
}
