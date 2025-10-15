using UnityEngine;

namespace RedSilver2.Framework.Interactions.Collectibles
{ 
    public class Note : PressCollectible
    {
        [SerializeField] private NoteData noteData;

        protected override void OnInteract()
        {
            base.OnInteract();
            gameObject.SetActive(false);
        }

        public override CollectibleData GetData()
        {
            return noteData;
        }
    }
}