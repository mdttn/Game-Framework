using UnityEngine;

namespace RedSilver2.Framework.Interactions.Collectibles
{

    [CreateAssetMenu(fileName = "New Note Data", menuName = "Note/Data")]
    public class NoteData : CollectibleData
    {
        [Space]
        [SerializeField] private string[] pages;
        public string[] Pages => pages;
    }
}
