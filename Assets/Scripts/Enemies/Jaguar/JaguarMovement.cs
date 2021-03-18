using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

public class JaguarMovement : MonoBehaviour
{
    private TargetHolder targetH;

    private NavMeshAgent agent;

    private bool hasPath = false;

    private NavMeshPath path;

    [SerializeField] private FloatVariable chaseSpeed;
    [SerializeField] private FloatVariable stopDist;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        targetH = GetComponent<TargetHolder>();

        agent.speed = chaseSpeed;

        agent.stoppingDistance = stopDist;
    }

    void Update()
    {
        hasPath = CheckForPath(targetH.Target.position);

        Chase();
    }

    void LateUpdate()
    {
        Vector3 dir = new Vector3(targetH.Target.position.x, transform.position.y, targetH.Target.position.z);

        transform.LookAt(dir);
    }
    private void Chase()
    {
        Vector3 wantedPos = RoundVecTo1D(targetH.Target.position);

        if (agent.destination != wantedPos)
        {
            if (CheckForPath(wantedPos)) agent.SetDestination(wantedPos);
        }
    }

    private Vector3 RoundVecTo1D(Vector3 vec)
    {
        float f1 = Mathf.Round(vec.x * 100) * 0.01f;
        float f2 = Mathf.Round(vec.y * 100) * 0.01f;
        float f3 = Mathf.Round(vec.z * 100) * 0.01f;

        return new Vector3(f1, f2, f3);
    }

    public bool CheckForPath(Vector3 destination)
    {
        agent.CalculatePath(destination, path);

        if (path.status == NavMeshPathStatus.PathComplete) return true;

        else return false;
    }

    private void DrawDebugRays()
    {

    }
}
