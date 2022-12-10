using System.Collections.Generic;
using UnityEngine;

public class ParticleEmiter : MonoBehaviour
{
    private List<ParticleSystem> particles = new List<ParticleSystem>(10);
    [SerializeField] bool loop = false;
    [SerializeField] bool playOnAwake = false;
    public bool beingPlayed;

    private void OnEnable()
    {
        particles = transform.GetChildElementsTo<ParticleSystem>();

        particles.ForEach(particleSystem =>
        {
            var mainModule = particleSystem.main;
            mainModule.loop = loop;
            mainModule.playOnAwake = playOnAwake;
        });
    }

    public void PlayOnce()
    {
        particles.ForEach(particle =>
        {
            particle.Play();
        });
    }

    public void Play() // I use this version when the particle needs to loop.
    {
        particles.ForEach(particle => 
{        
            if (!particle.isPlaying)
            {
                particle.Play();
            }
        });
    }

    public void Stop() 
    {
        particles.ForEach(particle => 
        {
            if (particle.isEmitting == true) 
            {
                particle.Stop();
            }
        });
    }

    public void SetLoop(bool value)
    {
        particles.ForEach(particles =>
        {
            var mainModule = particles.main;
            mainModule.loop = value;
        });
    }

    public List<ParticleSystem> GetParticles() => particles;
}
