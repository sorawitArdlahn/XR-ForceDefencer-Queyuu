using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public List<ParticleSystem> particleList = new List<ParticleSystem>();

    public void PlayParticles()
    {
        foreach (ParticleSystem particle in particleList)
        {
            particle.Play();
        }
    }
}
