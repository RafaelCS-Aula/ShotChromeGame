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

    private void Awake()
    {
        _myCol = GetComponent<MeshCollider>();    

    }
    public void EvaluateVelocity(float velocity)
    {
        if(velocity < velocityThreshold)
            _myCol.enabled = false;
        else
            _myCol.enabled = true;
    }
}
