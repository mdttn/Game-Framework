using RedSilver2.Framework.Interactions.Items;
using UnityEngine;

namespace RedSilver2.Framework.Player.Inventories.UI
{
    public sealed class HDItemNameDisplayer : HDItemInformationDisplayer
    {
        protected sealed override void DisplayItemInformation(ItemData data)
        {
            if(data != null) {
                DisplayItemInformation(data.Name);
            }
        }
    }
}