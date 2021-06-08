using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

[RequireComponent(typeof(ParticleSystem))]
public class ShockwaveScaler : MonoBehaviour
{

    [SerializeField] private FloatData multiplierData;
    [SerializeField] private float baseForce;
    ParticleSystem ps;
    // Start is called before the first frame update
    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        ApplySpeed();
    }

    public void ApplySpeed(float speed)
    {
        var main = ps.main;
        main.startSpeed = baseForce * speed;
    
    }
    public void ApplySpeed()
    {
        if(multiplierData == null)
            return;
        var main = ps.main;
        main.startSpeed = baseForce * multiplierData;
    
    }


}
