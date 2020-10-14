using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementComponent
{
    Vector3 MovementVector {get; set;}
    Vector3 FactorVector {get; set;}

   // T MovementData {get;}

}
