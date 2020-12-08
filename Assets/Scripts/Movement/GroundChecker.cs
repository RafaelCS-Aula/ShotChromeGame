using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

enum Casts{RAYCAST, SPHERE}

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private Casts CastMethod = Casts.SPHERE;
    private bool _useRay => CastMethod == Casts.RAYCAST;
    [SerializeField] private Vector3Variable origin;

    [ShowIf("_useRay")]
    [SerializeField] private FloatVariable rayRange;

    [ShowIf("_useRay")]
    [SerializeField] private Vector3Variable rayDirection;

    [HideIf("_useRay")]
    [SerializeField] private FloatVariable sphereRadius;

    [SerializeField] private LayerMask groundLayer;

    public bool OnGround()
    {

        if(_useRay)
           return CheckRay(origin, rayDirection, rayRange, groundLayer);
        else
            return CheckSphere(origin, sphereRadius, groundLayer);

    }

    public bool CheckSphere(Vector3 origin, float radius, LayerMask layer)
    {
        return (Physics.OverlapSphere(origin, radius, layer).Length > 0);
    }
    public bool CheckRay(Vector3 origin, Vector3 direction, float range, LayerMask layer)
    {
        return Physics.Raycast(origin, direction, range, layer);
    }
}
