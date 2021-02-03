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

    [SerializeField] private Vector3 wanderLimits = Vector3.zero;

    [SerializeField] private Color boundingBoxColor;

    private Vector3 wanderGoalPos;

    [HideInInspector] public bool isHerdWandering;
    [HideInInspector] public Transform herdWanderBounds;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        herd = GetComponentInParent<Herd>();
        agent.SetDestination(transform.parent.transform.position);
    }

    void Update()
    {
        if (herd.showDebugGizmos) DrawDebugRays();
        hasPath = CheckForPath(target.position);

        float distToTarget = Vector3.Distance(transform.position, target.position);

        if (distToTarget < chaseDist) inChaseDist = true;
        else inChaseDist = false;

        if (isHerdChasing) Chase();

        if (isHerdWandering) Wander();
    }

    void LateUpdate()
    {
        if (isHerdWandering)
        {
            if (agent.updateRotation) agent.updateRotation = false;
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }
        else transform.LookAt(target.position);


    }
    private void Chase()
    {
        //print("CHASING");
        Vector3 wantedPos = new Vector3(RoundFloat1D(target.position.x), agent.destination.y, RoundFloat1D(target.position.z));

        if (agent.destination != wantedPos)
        {
            //transform.LookAt(target.position);
            if (CheckForPath(wantedPos)) agent.SetDestination(wantedPos);
        }
    }

    private void Wander()
    {
        //print("WANDERING");
        bool turn;
        Bounds b = new Bounds(herdWanderBounds.position, herdWanderBounds.localScale);

        if (!b.Contains(transform.position)) turn = true;
        else turn = false;

        if (turn)
        {
            Vector3 direction = herd.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(direction),
                                                  agent.angularSpeed * Time.deltaTime);
        }

        wanderGoalPos = herd.goalPos;

        //transform.LookAt(wanderGoalPos);

        agent.SetDestination(wanderGoalPos);
    }

    private float RoundFloat1D(float f)
    {
        return Mathf.Round(f * 100) * 0.01f;
    }

    public bool CheckForPath(Vector3 destination)
    {
        NavMeshPath path = new NavMeshPath();

        agent.CalculatePath(destination, path);

        if (path.status == NavMeshPathStatus.PathComplete) return true;

        else return false;

        //if (HasWallInBetween()) hasPath = false;
    }

    private bool HasWallInBetween()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, target.position, out hitInfo, Mathf.Infinity, wallLayer))
        {
            if ((wallLayer | (1 << hitInfo.transform.gameObject.layer)) == wallLayer) return true;
        }

        return false;
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
