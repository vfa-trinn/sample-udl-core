using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDL.Core.Sound
{
    public class ParticleSoundSystem : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem particle;
        [SerializeField]
        private AudioSource bornSound;
        [SerializeField]
        private bool customInterval;
        [SerializeField]
        private float customIntervalValue;

        private float emitRate;
        private float timeCount;

        private void Awake()
        {
            if (bornSound != null)
            {
                bornSound.playOnAwake = false;
                bornSound.loop = false;
            }

            emitRate = 1 / particle.emissionRate;
            if(customInterval)
            {
                emitRate = customIntervalValue;
            }

            timeCount = emitRate;
        }

        private void Start()
        {
            bornSound.Play();
        }

        private void Update()
        {
            timeCount -= Time.deltaTime;
            if(timeCount <= 0)
            {
                timeCount = emitRate;
                bornSound.Play();
            }
            //var amount = Mathf.Abs(currentNumberOfParticles - particle.particleCount);

            //if (particle.particleCount < currentNumberOfParticles)
            //{
            //    if (dieSound != null) dieSound.Play();
            //}
            //else if (particle. > currentNumberOfParticles)
            //{
            //    if (bornSound != null) bornSound.Play();
            //}
            //Debug.Log("compare " + particle.particleCount + " == " + currentNumberOfParticles);
            //currentNumberOfParticles = particle.particleCount;
        }
    }
}
