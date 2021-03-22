using System.Collections;
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

    /// <summary>
    /// Create an Outgoing connection with the Spawners in the flyerEntryPoints
    /// colection.
    /// </summary>
    [Button]
    public void ConnectWithFlyerWaypoints()
    {
        if (!_isFlyer)
        {
            Debug.Log("Only flyer types spawners make use of flyer waypoints");
            return;
        }

        flyerWaypoint = gameObject.GetComponent<Waypoint>() ?? gameObject.AddComponent<Waypoint>();
        flyerWaypoint.drawPathGizmos = false;
        foreach (Waypoint wp in flyerEntryWaypoints)
        {
            flyerWaypoint.MakeOneWayOutgoingConnectionWith(wp);
        }
    }

    /// <summary>
    /// Spawn the first object in its queue and if there are more left, keep 
    /// itself open too keep spawning
    /// </summary>
    /// <param name="callerWave">The wave commanding the spawns</param>
    /// <param name="minDistanceToNext">The minimum distance a newly spawned
    /// enemy must have to this spawner until it can spawn the next monster in 
    /// the queue.</param>
    /// <returns></returns>
    public GameObject StartSpawning(CombatWave callerWave, float minDistanceToNext)
    {

        _callerWave = callerWave;
        _minDistanceToNext = minDistanceToNext;

        _lastSpawned = Instantiate(spawnQueue.Peek(), transform.position, transform.rotation);
        spawnQueue.Dequeue();

        TargetHolder target = _lastSpawned.GetComponent<TargetHolder>();
        if (target != null)
        {
            target.Target = _callerWave.enemyTarget;
        }
        else Debug.LogError($"Enemy from {_lastSpawned.name} doesn't have a TargetHolder Component");

        if (_isFlyer)
        {
            GetComponent<Waypoint>().ToggleOccupation();
            _lastSpawned.GetComponent<FlyerMovement>().SetCurrentWaypoint(GetComponent<Waypoint>());
        }

        if (spawnQueue.Count > 0)
            _openForSpawning = true;

        return _lastSpawned;
    }

    private void Update()
    {

        if (_openForSpawning)
        {
            bool _canSpawn = false;

            if (!_isFlyer)
            {
                if(_lastSpawned == null)
                {
                    _canSpawn = true;
                }
                else
                {
                    // If the newest spawn is far away enough, spawn the next in the queue.
                    float distance = (_lastSpawned.transform.position - transform.position).sqrMagnitude;

                    if (distance >= _minDistanceToNext) _canSpawn = true;
                }
            }

            else
            {
                Waypoint thisWP = GetComponent<Waypoint>();

                if (_lastSpawned == null)
                {
                    print("NULL SPAWNED");
                    if (!thisWP.isOccupied) _canSpawn = true; ;
                }
                else
                {
                    float distance = (_lastSpawned.transform.position - transform.position).sqrMagnitude;

                    if (distance >= _minDistanceToNext)
                    {

                        if (!thisWP.isOccupied) _canSpawn = true; ;
                        
                        /*
                        for (int i = 0; i < thisWP.outgoingConnections.Count; i++)
                        {

                            if (!thisWP.outgoingConnections[i].isOccupied)
                            {
                                _canSpawn = true;
                                break;
                            }
                        }
                        */
                    }
                }
            }

            if (_canSpawn)
            {
                _lastSpawned = Instantiate(spawnQueue.Peek(), transform.position, transform.rotation);
                TargetHolder target =
                    _lastSpawned.GetComponent<TargetHolder>();
                if (target != null)
                {
                    target.Target = _callerWave.enemyTarget;
                }
                else Debug.LogError($"Enemy from {_lastSpawned.name} doesn't have a TargetHolder Component");


                if (_isFlyer)
                {
                    GetComponent<Waypoint>().ToggleOccupation();
                    _lastSpawned.GetComponent<FlyerMovement>().SetCurrentWaypoint(GetComponent<Waypoint>());
                }

                _lastSpawned.layer = 9;
                spawnQueue.Dequeue();
                _callerWave.AddLateSpawnedEnemy(enemy, _lastSpawned);
            }

            if (spawnQueue.Count == 0)
                _openForSpawning = false;
        }
    }

    private void OnDrawGizmos()
    {

        Gizmos.DrawIcon(transform.position, pathToIcons, true);
        Handles.RadiusHandle(Quaternion.identity, transform.position, _minDistanceToNext, false);

        Handles.ConeHandleCap(GetInstanceID(), transform.position + transform.forward, transform.rotation, 0.3f, EventType.Repaint);

        Gizmos.DrawRay(transform.position, transform.forward);
        if (flyerWaypoint != null && _isFlyer)
        {
            Handles.color = Color.blue;
            foreach (Waypoint wp in flyerWaypoint.outgoingConnections)
            {
                Handles.DrawDottedLine(transform.position, wp.transform.position, 2f);

                Vector3 direction = (wp.transform.position - transform.position).normalized;
                if (wp.transform.position == transform.position)
                    continue;

                Quaternion coneRotation = Quaternion.LookRotation(direction, transform.up);


                Handles.ConeHandleCap(wp.GetInstanceID(), transform.position + direction * 1.2f, coneRotation, 0.5f, EventType.Repaint);
            }
        }
    }
}
