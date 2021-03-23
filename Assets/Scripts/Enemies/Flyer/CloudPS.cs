using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPS : MonoBehaviour
{
    private ParticleSystem cloudPS;

    // Start is called before the first frame update
    void Start()
    {
        cloudPS = GetComponent<ParticleSystem>();
        StartCoroutine(CloudParticleSetup(0.01f));
    }

    private IEnumerator CloudParticleSetup(float time)
    {
        cloudPS.Play();
        yield return new WaitForSeconds(time);
        cloudPS.Pause();
    }
}
