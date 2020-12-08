using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VerticalMovementBase : MonoBehaviour, IMovementComponent
{


    [Header("General Vertical Movement Settings")]
    
    [SerializeField]
    protected Vector3Variable feetPosition;

    [SerializeField]
    protected FloatVariable feetRadius;

    [SerializeField]
    protected FloatVariable  sideAirControl;

    [SerializeField]
    protected FloatVariable frontAirControl;

    public Vector3 MovementVector {get; set;}
    public Vector3 FactorVector {get; set;}

    [SerializeField]
    protected LayerMask groundLayer;
    [SerializeField]
    protected bool showGizmos = false;

    protected float input;
    protected Vector3 _mov = Vector3.zero;
    protected Vector3 _fact = Vector3.one;




}
