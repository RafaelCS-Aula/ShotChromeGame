using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VerticalMovementBase : MonoBehaviour, IMovementComponent
{

    [SerializeField]
    protected Vector3 feetPosition;

    [SerializeField]
    protected FloatVariable feetRadius;

    public Vector3 MovementVector {get; set;}
    public Vector3 FactorVector {get; set;}

    [SerializeField]
    protected LayerMask groundLayer;
    protected float input;
    protected Vector3 _mov = Vector3.zero;
    protected Vector3 _fact = Vector3.one;

    [SerializeField]
    protected FloatVariable  sideAirControl;

    [SerializeField]
    protected FloatVariable frontAirControl;

    

    private bool TouchingGround()
    {
    
        Vector3 pos = feetPosition + transform.localPosition;
        Collider[] collisions = 
            Physics.OverlapSphere(pos, feetRadius.Value, groundLayer);
        return collisions.Length > 0 ;
        
    }

    private void OnDrawGizmos() 
    {
        float gRadius = feetRadius == null ? 0.0f : feetRadius.Value;
        Gizmos.color = Color.cyan;

        Gizmos.DrawSphere(feetPosition + transform.localPosition, gRadius);
        
        
    }

}
