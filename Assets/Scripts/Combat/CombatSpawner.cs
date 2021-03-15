using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;

public class CombatSpawner : MonoBehaviour
{
    [HideInInspector]
    public EnemyTypes enemy;

    private string _enemyName => enemy.ToString().ToLower();
    private string pathToIcons => $"enemies/{_enemyName}_icon.png";

    private Waypoint flyerWaypoint = null;

    private bool _isFlyer => enemy == EnemyTypes.FLYER;

    [ShowIf("_isFlyer")]
    public List<Waypoint> flyerEntryWaypoints;

    [Button]
    public void ConnectWithFlyerWaypoints()
    {
        if(!_isFlyer)
        {
            Debug.Log("Only flyer types spawners make use of flyer waypoints");
            return;
        }

        flyerWaypoint = gameObject.GetComponent<Waypoint>() ?? gameObject.AddComponent<Waypoint>();
        flyerWaypoint.drawPathGizmos = false;
        foreach(Waypoint wp in flyerEntryWaypoints)
        {
            flyerWaypoint.MakeOneWayOutgoingConnectionWith(wp);
        }
    }
    private void OnDrawGizmos() 
    {
        
       Gizmos.DrawIcon(transform.position, pathToIcons, true);

       if(flyerWaypoint != null && _isFlyer)
       {
           Handles.color = Color.blue;
           foreach(Waypoint wp in flyerWaypoint.outgoingConnections)
           {
               Handles.DrawDottedLine(transform.position, wp.transform.position,2f);

                Vector3 direction = (wp.transform.position - transform.position).normalized;
                if(wp.transform.position == transform.position)
                    continue;

                Quaternion coneRotation = Quaternion.LookRotation(direction,transform.up);
                
                
                Handles.ConeHandleCap(wp.GetInstanceID(),transform.position + direction * 1.2f, coneRotation,0.5f,EventType.Repaint);
           }
       }
    }
}
