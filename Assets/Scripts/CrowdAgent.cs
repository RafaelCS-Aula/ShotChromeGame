using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(MeshCollider))]
public class CrowdAgent : MonoBehaviour
{

    private Collider _myCol;

    [Expandable]
    [SerializeField] FloatVariable velocityThreshold;
    [SerializeField] FloatVariable runnerMaxVelocity;

    [Expandable]
    [SerializeField] FloatVariable timeUntilColiderReset;

    

    private float _factoredThreshold;

    private void Awake()
    {
        _myCol = GetComponent<MeshCollider>();    
        
        
    }
    public void EvaluateVelocity(float velocity)
    {
        _factoredThreshold =
             runnerMaxVelocity * velocityThreshold;

        if(velocity < _factoredThreshold)
            _myCol.enabled = false;
        else
            _myCol.enabled = true;

        StartCoroutine(ResetCollider());
    }

    private IEnumerator ResetCollider()
    {
        yield return new WaitForSeconds(timeUntilColiderReset);
        _myCol.enabled = true;
        print("collider ON");
        yield return null;
    } 
}
