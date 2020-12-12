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

    [SerializeField] private bool showGizmos;

    [ShowIf("showGizmos")]
    [SerializeField] private Color gizmoColor;

    [ShowIf("showGizmos")]
    [Range(0,1)]
    [SerializeField] private float gizmoTransparency;
    public bool OnGround()
    {

        if(_useRay)
           return CheckRay(origin, rayDirection, rayRange, groundLayer);
        else
            return CheckSphere(origin, sphereRadius, groundLayer);

            

    }

    public bool OnGround(LayerMask layer)
    {

        if(_useRay)
           return CheckRay(origin, rayDirection, rayRange, layer);
        else
            return CheckSphere(origin, sphereRadius, layer);

    }

    private bool CheckSphere(Vector3 origin, float radius, LayerMask layer)
    {
        //print(Physics.OverlapSphere(origin, radius, layer).Length);
        return (Physics.OverlapSphere(transform.localPosition + origin, radius, layer).Length > 0);
    }
    private bool CheckRay(Vector3 origin, Vector3 direction, float range, LayerMask layer)
    {
        //RaycastHit hit;
        //print( Physics.Raycast(origin, direction, out hit, range, layer));
        //if(hit.collider != null)print(hit.collider.name);
        return Physics.Raycast(transform.localPosition + origin, direction, range, layer);
    }
    public bool CheckRay(out RaycastHit hitInfo, LayerMask layer)
    {
        return Physics.Raycast(transform.localPosition + origin, rayDirection, out hitInfo, rayRange, layer);
    }

    private void OnDrawGizmos() 
    {
        if(!showGizmos)
            return;
        float gRadius = sphereRadius;
        float gRange = rayRange;
        gizmoColor.a = gizmoTransparency;
        Gizmos.color = gizmoColor;

        if(!_useRay)
            Gizmos.DrawSphere(origin + transform.localPosition, gRadius);
        if(_useRay)
            Gizmos.DrawLine(origin + transform.localPosition, origin + transform.localPosition + rayDirection.Value.normalized * rayRange);
        
        
    }
}
