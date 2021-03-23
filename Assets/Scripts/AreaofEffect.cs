﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class AreaofEffect : MonoBehaviour
{
    //[SerializeField] private bool isImmediate;

    [Foldout("Events")]
    [SerializeField] private UnityEvent OnActivateAoE;
    [Foldout("Events")]
    [SerializeField] private UnityEvent<Vector3> OnActivateAoEPosition;

    [Foldout("Events")]
    [SerializeField] private UnityEvent<Vector3, Vector3[]> OnFindAffected;

    [SerializeField] private ResourceTypes AffectedResource;
    [SerializeField] private bool useEffectFallOff = true;

    [SerializeField] private FloatVariable effectAmount;
    [SerializeField] private FloatVariable maxRadius;
    [SerializeField] private LayerMask affectedLayers;
    [SerializeField] private LayerMask fullyBlockedByLayers;
    [SerializeField] private CurveVariable effectFalloff;

    [SerializeField] private bool showGizmos;

    [ShowIf("showGizmos")]
    [SerializeField] private Color gizmoHitColor;
    [ShowIf("showGizmos")]
    [SerializeField] private Color gizmoBlockedColor;

    [ShowIf("showGizmos")]
    [Range(0,1)]
    [SerializeField] private float gizmoTransparency;

    private Collider[] _insideArea;

    /// <summary>
    /// Store the last colliders hit be the AoE and how affect they were
    /// </summary>
    /// <typeparam name="Collider"></typeparam>
    /// <typeparam name="float"></typeparam>
    /// <returns></returns>
    public Dictionary<Collider, float> HitsAndAffect = 
        new Dictionary<Collider, float>();
    
    public void ApplyAoE(Vector3 center)
    {
        List<Vector3> hitPositions = new List<Vector3>();
        OnActivateAoE.Invoke();
        OnActivateAoEPosition.Invoke(center);
        HitsAndAffect =
             new Dictionary<Collider, float>();

             //print("boom");
        // Debug
        debugPoints = new List<(Vector3, bool, float)>();
        debugPoints.Add((center, true, effectFalloff.Value.Evaluate(0)));
        //////////
        //print("From AoE: Radius - " + maxRadius.Value );
        _insideArea = Physics.OverlapSphere(center, maxRadius, affectedLayers);
        //print(_insideArea.Length);
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
                if(showGizmos)
                    Debug.DrawLine(center, wallHit.point, gizmoBlockedColor,4);
                        //print("hit wall");
                continue;
            }
                

            
            float effectModifier = 1; 
            if(useEffectFallOff)
            {
                float hitEffect = distToHit / maxRadius;
                effectModifier = effectFalloff.Value.Evaluate(hitEffect);

            }
                

            float finalPower = effectModifier * effectAmount;

            HitsAndAffect.Add(c, finalPower);
            //print(HitsAndAffect.Count);

            //Debug
            debugPoints.Add((c.ClosestPoint(center), true, effectModifier));

            if(showGizmos)
                Debug.DrawLine(center, c.ClosestPoint(center), gizmoHitColor,4);

             print(c.gameObject.name);
            /////////////////
            hitPositions.Add(c.ClosestPoint(center));
            //Get the Health component of the hit colliders and affect them.

        }
        OnFindAffected.Invoke(center, hitPositions.ToArray() );
        foreach (var hit in HitsAndAffect)
        {
            EnemyHealth enemyHealth = hit.Key.transform.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth == null) enemyHealth = hit.Key.transform.gameObject.GetComponentInParent<EnemyHealth>();

            if(enemyHealth != null)
                enemyHealth.OnDamaged(hit.Value);
        }
    }

    

    private List<(Vector3 pos, bool hit, float eff)> debugPoints = 
    new List<(Vector3 pos, bool hit, float eff)>();
    private void OnDrawGizmosSelected() 
    {
        if(!showGizmos)
            return;
        gizmoHitColor.a = gizmoTransparency;
        
        if(debugPoints.Count > 0)
        {
            //Gizmos.DrawSphere(debugPoints[0].pos, maxRadius);    
            for (int i = 1; i < debugPoints.Count; i++)
            {
                if(debugPoints[i].hit)
                    Gizmos.color = gizmoHitColor;
                else
                    Gizmos.color = gizmoBlockedColor;
                
                //Gizmos.DrawWireSphere(debugPoints[i].pos, debugPoints[i].eff);
            }
        }

            
        

    }


}
