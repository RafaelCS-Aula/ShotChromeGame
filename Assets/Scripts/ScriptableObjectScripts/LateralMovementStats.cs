using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MovementData/HorizontalMovement", fileName = "HorizontalMovementStats")]
public class LateralMovementStats : ScriptableObject
{
    public float forwardAcceleration;
    public float backwardAcceleration;
    public float sideAcceleration;
    public float decerelation;
    public float maxVelocity;

    public Vector3 currentVelocity;

}
