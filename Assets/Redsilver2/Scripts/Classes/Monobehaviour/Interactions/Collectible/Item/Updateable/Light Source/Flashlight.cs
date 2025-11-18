using RedSilver2.Framework.Interactions.Collectibles;
using UnityEngine;

namespace RedSilver2.Framework.Interactions.Items
{
    // this is for testing
    public class Flashlight : LightSourceItem
    {
        [Space]
        [SerializeField] private ItemData data;

        [Space]
        [SerializeField] private RuntimeAnimatorController leftArmController, rightArmController;

        public override CollectibleData GetData()
        {
            return data;
        }


        protected sealed override void SetHandType(ref ItemHandType handType) {
            handType = ItemHandType.OneHanded;
        }

        protected sealed override RuntimeAnimatorController GetAnimatorController(ItemHandSide handSide)
        {
            return handSide == ItemHandSide.Left ? leftArmController : rightArmController;
        }
    }
}
