using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class VerticalMovementBase : MonoBehaviour, IMovementComponent
{


    [Header("General Vertical Movement Settings")]
    
    [SerializeField]
    [Expandable] protected Vector3Variable feetPosition;

    [SerializeField]
    [Expandable] protected FloatVariable feetRadius;

    [SerializeField]
    [Expandable] protected FloatVariable  sideAirControl;

    [SerializeField]
    [Expandable] protected FloatVariable frontAirControl;

    public Vector3 MovementVector {get; set;}
    public Vector3 FactorVector {get; set;}

    [SerializeField]
    protected LayerMask groundLayer;
    [SerializeField]
    protected bool showGizmos = false;

    protected float input;
    protected Vector3 _mov = Vector3.zero;
    protected Vector3 _fact = Vector3.one;



    

    protected bool TouchingGround()
    {
    
        Vector3 pos = feetPosition + transform.localPosition;
        Collider[] collisions = 
            Physics.OverlapSphere(pos, feetRadius.Value, groundLayer);
        return collisions.Length > 0 ;
        
    }

    protected void OnDrawGizmos() 
    {
        if(!showGizmos)
            return;
        float gRadius = feetRadius == null ? 0.0f : feetRadius.Value;
        Gizmos.color = Color.cyan;

        Gizmos.DrawSphere(feetPosition + transform.localPosition, gRadius);
        
        
    }

}
