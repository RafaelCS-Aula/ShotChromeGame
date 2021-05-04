using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttractor : MonoBehaviour
{
    [SerializeField] private Transform attractionPoint;
    [SerializeField] private ParticleSystem system;
    [SerializeField] private float attractionForce;
    
    private ParticleSystem.Particle[] _particles;

    // Update is called once per frame
    void LateUpdate()
    {
        if(attractionPoint == null || system == null )
            return;
        
        _particles = new ParticleSystem.Particle[system.particleCount];
        system.GetParticles(_particles);

        for(int i = 0; i < _particles.Length; i++)
        {

            ParticleSystem.Particle particle = _particles[i];
            Vector3 direction =  (attractionPoint.position - particle.position).normalized;

            Vector3 attractionVector = direction * attractionForce * Time.deltaTime;

            particle.velocity += attractionVector;

            _particles[i] = particle;

        }

        system.SetParticles(_particles);

    }
}
