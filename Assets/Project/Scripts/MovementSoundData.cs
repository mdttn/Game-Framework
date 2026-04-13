using UnityEngine;

namespace RedSilver2.Framework.StateMachines.Extensions
{
    [CreateAssetMenu(fileName = "New Movement Sound Datas", menuName = "Movement Sound Datas")]
    public class MovementSoundData : ScriptableObject
    {
        public string groundTag;
        public AudioClip[] moveAudioClips;
        public AudioClip[] landAudioClips;
    }
}
