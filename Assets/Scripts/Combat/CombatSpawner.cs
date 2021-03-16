﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;

public class CombatSpawner : MonoBehaviour
{
    [HideInInspector]
    public EnemyTypes enemy;

    public Queue<GameObject> spawnQueue = new Queue<GameObject>();

    private string _enemyName => enemy.ToString().ToLower();
    private string pathToIcons => $"enemies/{_enemyName}_icon.png";

    private Waypoint flyerWaypoint = null;

    private bool _isFlyer => enemy == EnemyTypes.FLYER;
    private bool _openForSpawning;
    private CombatWave _callerWave;
    private float _minDistanceToNext;

    private GameObject _lastSpawned;

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

    public GameObject StartSpawning(CombatWave callerWave,float minDistanceToNext)
    {
        _callerWave = callerWave;
        _minDistanceToNext = minDistanceToNext;
        _lastSpawned = Instantiate(spawnQueue.Peek(),transform.position, Quaternion.identity);
        spawnQueue.Dequeue();

        if(spawnQueue.Count > 0)
            _openForSpawning = true;

        return _lastSpawned;
    }

    private void Update() 
    {
        if(_openForSpawning)
        {
            float distance = distance = (_lastSpawned.transform.position - transform.position).sqrMagnitude;

            if(distance >= _minDistanceToNext)
            {
                _lastSpawned = Instantiate(spawnQueue.Peek(),transform.position, Quaternion.identity);
                spawnQueue.Dequeue();
                _callerWave.AddLateSpawnedEnemy(enemy,_lastSpawned);
                
            }   
            if(spawnQueue.Count == 0)
                _openForSpawning = false;
        }
        
        
    }

    private void OnDrawGizmos() 
    {
        
       Gizmos.DrawIcon(transform.position, pathToIcons, true);
        Handles.RadiusHandle(Quaternion.identity,transform.position,_minDistanceToNext,false);
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
