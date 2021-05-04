using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TargetHolder : MonoBehaviour
{
    public Transform Target;

    [SerializeField] private LayerMask sightBlockLayers;

    private LayerMask _targetLayer => Target != null ? Target.gameObject.layer : gameObject.layer;

    [ShowNativeProperty]
    private string targetLayerName => LayerMask.LayerToName(_targetLayer);
    public bool HasLineOfSightToTarget(Transform origin = null)
    {
        Vector3 startPoint = 
            origin == null ? transform.position : origin.position;

        Ray rayshow = new Ray(startPoint, (Target.position - startPoint).normalized) ;
        RaycastHit hitinfo;
       // Debug.DrawRay(startPoint,(Target.position - startPoint).normalized * 700);
        //Find the Target
        bool foundTarget = Physics.Raycast(rayshow, out hitinfo, 3000, layerMask: LayerMask.GetMask(LayerMask.LayerToName(_targetLayer)));
//        print($"{hitinfo.transform.name}");
        if (foundTarget)
        {
            
            RaycastHit blockedHitInfo;
            // Find if there are obstacles between it and target
            Ray blockingRay = new Ray(hitinfo.point,(startPoint-hitinfo.point).normalized);
            if(Physics.Raycast(blockingRay,
                out blockedHitInfo,
                Vector3.Distance(hitinfo.point, startPoint),
                layerMask: sightBlockLayers))
            {
                //Debug.DrawLine(startPoint,blockedHitInfo.point, Color.red, 1.0f);

                // Hit something that blocks sight
                return false;
            }
            else
            {
                
                //Debug.DrawLine(startPoint,Target.position, Color.green, 1.0f);
                return true;
            } 
        }
        else
        {
            return false;  
        }   

    }
}
