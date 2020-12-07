using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AreaofEffect : MonoBehaviour
{
    //[SerializeField] private bool isImmediate;

    [SerializeField] private ResourceTypes AffectedResource;
    [SerializeField] private bool useEffectFallOff = true;

    [SerializeField] private FloatVariable effectAmount;
    [SerializeField] private FloatVariable maxRadius;
    [SerializeField] private LayerMask affectedLayers;
    [SerializeField] private LayerMask fullyBlockedByLayers;
    [SerializeField] private CurveVariable effectFalloff;

    private Collider[] _insideArea;
    
    
    public void ApplyAoE(Vector3 center)
    {
        // Debug
        debugPoints = new List<(Vector3, bool, float)>();
        debugPoints.Add((center, true, effectFalloff.Value.Evaluate(0)));
        //////////

        _insideArea = Physics.OverlapSphere(center, maxRadius, affectedLayers);
        foreach(Collider c in _insideArea)
        {
            Vector3 dirToHit;
            float distToHit;
            distToHit = (center - c.transform.position).magnitude;
            dirToHit = (c.transform.position - center).normalized;

            
            // Dont affect objects behind walls
            RaycastHit wallHit;
            if(Physics.SphereCast(center,1,dirToHit,out wallHit,distToHit,fullyBlockedByLayers))
            {
                Debug.DrawLine(center, wallHit.point, Color.red,4);
                continue;
            }
                

            
            float effectModifier = 1; 
            if(useEffectFallOff)
            {
                float hitEffect = distToHit / maxRadius;
                effectModifier = effectFalloff.Value.Evaluate(hitEffect);

            }
                

            float finalPower = effectModifier * effectAmount;

            //Debug
            debugPoints.Add((c.ClosestPoint(center), true, effectModifier));

            Debug.DrawLine(center, c.ClosestPoint(center), Color.yellow,4);
            print("bzzz");
            /////////////////
            
            //Get the Health component of the hit colliders and affect them.

        }

    }

    private List<(Vector3 pos, bool hit, float eff)> debugPoints = 
    new List<(Vector3 pos, bool hit, float eff)>();
    private void OnDrawGizmos() {

        if(debugPoints.Count > 0)
        {
            Gizmos.DrawWireSphere(debugPoints[0].pos, maxRadius);
            for (int i = 1; i < debugPoints.Count; i++)
            {
                if(debugPoints[i].hit)
                    Gizmos.color = Color.yellow;
                else
                    Gizmos.color = Color.red;
                
                Gizmos.DrawSphere(debugPoints[i].pos, debugPoints[i].eff);
            }
        }
        

    }


}
