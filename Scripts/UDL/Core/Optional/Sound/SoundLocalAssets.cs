using System.Collections;
using System.Collections.Generic;
using UDL.Core.Sound;
using UnityEngine;

namespace UDL.Core.Sound
{
    [CreateAssetMenu(fileName = "SoundLocalAssets", menuName = "SoundLocalAssets", order = 51)]
    public class SoundLocalAssets : ScriptableObject
    {
        [SerializeField]
        private AudioFile[] musicLocalFiles = default;

        public AudioFile[] MusicLocalFiles
        {
            get { return musicLocalFiles; }
        }

        [SerializeField]
        private AudioFile[] soundLocalFiles = default;

        public AudioFile[] SoundLocalFiles
        {
            get { return soundLocalFiles; }
        }       
    }
}