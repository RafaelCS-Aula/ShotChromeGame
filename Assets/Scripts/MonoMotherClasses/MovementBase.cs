using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public abstract class MovementBase : InputReceiverBase, IMovementComponent
{
    public Vector3 MovementVector {get; set;}
    public Vector3 FactorVector {get; set;}
    protected Vector3 _mov = Vector3.zero;
    protected Vector3 _fact = Vector3.one;
}
