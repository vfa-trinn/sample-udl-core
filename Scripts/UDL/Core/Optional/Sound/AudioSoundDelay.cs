using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDL.Core.Sound
{
    public class AudioSoundDelay : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private float delayTime = 0.5f;

        private void Awake()
        {
            if (audioSource == null) return;
            audioSource.playOnAwake = false;
        }

        private void OnEnable()
        {
            if (audioSource == null) return;
            StopAllCoroutines();
            StartCoroutine(PlayAudioDelay());
        }

        private IEnumerator PlayAudioDelay()
        {
            if (audioSource == null) yield break;
            yield return new WaitForSeconds(delayTime);
            audioSource.Play();
        }
    }
}
