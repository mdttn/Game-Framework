using TMPro;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public abstract class HDItemInformationDisplayer : ItemInformationDisplayer {

        protected TextMeshProUGUI displayer;
        
        protected override void Awake() 
        {
            displayer = GetComponent<TextMeshProUGUI>();
            base.Awake();
        }
    }
}
