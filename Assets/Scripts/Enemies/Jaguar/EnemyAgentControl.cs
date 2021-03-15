using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

public class EnemyAgentControl : MonoBehaviour
{
    public Transform target;

    NavMeshAgent agent;

    public bool hasPath = false;

    private Herd herd;

    public float chaseDist;
    [HideInInspector] public bool isHerdChasing;
    [HideInInspector] public bool inChaseDist;

    public LayerMask wallLayer;
    private NavMeshPath path;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        herd = GetComponentInParent<Herd>();
        path = new NavMeshPath();
        agent.SetDestination(transform.parent.transform.position);
    }

    void Update()
    {
        if (herd.showDebugGizmos) DrawDebugRays();
        hasPath = CheckForPath(target.position);

        Chase();
    }

    void LateUpdate()
    {
        Vector3 dir = new Vector3(target.position.x, transform.position.y, target.position.z);

        transform.LookAt(dir);
    }
    private void Chase()
    {
        //print("CHASING");
        Vector3 wantedPos = new Vector3(RoundFloat1D(target.position.x), RoundFloat1D(target.position.y), RoundFloat1D(target.position.z));

        if (agent.destination != wantedPos)
        {
            //transform.LookAt(target.position);
            if (CheckForPath(wantedPos)) agent.SetDestination(wantedPos);
        }
    }

    private float RoundFloat1D(float f)
    {
        return Mathf.Round(f * 100) * 0.01f;
    }

    public bool CheckForPath(Vector3 destination)
    {
        agent.CalculatePath(destination, path);

        if (path.status == NavMeshPathStatus.PathComplete) return true;

        else return false;
    }

    private void DrawDebugRays()
    {
        #region SeekDistanceRays
        Vector3 cDir1 = transform.forward.normalized;
        Vector3 cDir2 = -transform.forward.normalized;
        Vector3 cDir3 = transform.right.normalized;
        Vector3 cDir4 = -transform.right.normalized;
        Debug.DrawRay(transform.position, cDir1 * chaseDist, Color.red);
        Debug.DrawRay(transform.position, cDir2 * chaseDist, Color.red);
        Debug.DrawRay(transform.position, cDir3 * chaseDist, Color.red);
        Debug.DrawRay(transform.position, cDir4 * chaseDist, Color.red);
        #endregion
    }
}
