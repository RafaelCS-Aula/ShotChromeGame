using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VerticalMovementBase : MonoBehaviour, IMovementComponent
{

    public Vector3 MovementVector {get; set;}
    public Vector3 FactorVector {get; set;}
    protected Vector3 _mov = Vector3.zero;
    protected Vector3 _fact = Vector3.one;




}
