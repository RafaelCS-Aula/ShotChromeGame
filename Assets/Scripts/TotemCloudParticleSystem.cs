using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TotemCloudParticleSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem cloudParticles;
    
    private SphereCollider _colliderToMatchRadius;

    // Start is called before the first frame update
    private void Start() {
        _colliderToMatchRadius = GetComponent<SphereCollider>();
        ParticleSystem.ShapeModule shape = cloudParticles.shape;
        
        shape.radius = _colliderToMatchRadius.radius;
        ParticleSystem.VelocityOverLifetimeModule vel = cloudParticles.velocityOverLifetime;
        vel.radial = -(shape.radius - 2);
        cloudParticles.Play(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
