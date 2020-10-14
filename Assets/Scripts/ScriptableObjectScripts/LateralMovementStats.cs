using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MovementData/HorizontalMovement", fileName = "HorizontalMovementStats")]
public class LateralMovementStats : ScriptableObject
{
    public float maxForwardVelocity;
    public float maxBackVelocity;
    public float maxStrafeVelocity;
    public float acelerationTime;
    public float decerelationTime;
    

    public Vector3 currentVelocity;

}
