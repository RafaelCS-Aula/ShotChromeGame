using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class DestroyObjectAfterTime : MonoBehaviour
{
    [SerializeField] private FloatVariable LifeTime;
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
        yield return new WaitForSeconds(LifeTime.Value);
        if(_particleSystem)
        {
            var emiss = _particleSystem.emission;
            emiss.enabled = false;
            yield return new WaitForSeconds(_particleSystem.main.startLifetime.constant);


        }

        Destroy(gameObject);
        
    }
}
