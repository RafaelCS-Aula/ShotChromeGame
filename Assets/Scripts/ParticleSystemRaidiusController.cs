using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemRaidiusController : MonoBehaviour
{
    [SerializeField] private FloatData radius;
    
    private ParticleSystem _ps;
    // Start is called before the first frame update
    void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
        ApplyRadius();
    }


    public void ApplyRadius()
    {
        if(_ps == null)
            return;

        var shape = _ps.shape;
        shape.radius = radius;
    }

}
