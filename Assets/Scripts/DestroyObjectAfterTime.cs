using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class DestroyObjectAfterTime : MonoBehaviour
{
    [SerializeField] private FloatVariable LifeTime;
    [SerializeField] private bool stopParticles;
    private ParticleSystem _particleSystem;

    private void OnEnable() 
    {

        _particleSystem = GetComponent<ParticleSystem>() ;
        StartCoroutine(CountDown());
    }

    public IEnumerator CountDown()
    {
        if(LifeTime == null)
            yield return null;

        yield return new WaitForSecondsRealtime(LifeTime.Value);

        if(stopParticles && _particleSystem)
        {
            var emiss = _particleSystem.emission;
            emiss.enabled = false;
            yield return new WaitForSecondsRealtime(_particleSystem.main.startLifetime.constant);


        }

        Destroy(gameObject);
        
    }
}
