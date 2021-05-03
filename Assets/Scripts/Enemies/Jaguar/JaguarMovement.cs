using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

public class JaguarMovement : MonoBehaviour
{
    private TargetHolder targetH;

    private NavMeshAgent agent;
    private NavMeshPath path;
    private bool hasPath = false;

    [SerializeField] private FloatVariable chaseSpeed;
    [SerializeField] private FloatVariable stopDist;

    public bool globMoving = true;
    private bool moving = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        targetH = GetComponent<TargetHolder>();

        path = new NavMeshPath();

        agent.stoppingDistance = stopDist;
        agent.acceleration = float.MaxValue;
        agent.speed = chaseSpeed;
        agent.SetDestination(targetH.Target.position);
        StartCoroutine(GetDestinationWithDelay(0.1f));
    }

    void Update()
    {
        if (!globMoving && moving)
        {
            moving = false;
            StopAllCoroutines();
        }
        if (globMoving && !moving)
        {
            moving = true;
            StartCoroutine(GetDestinationWithDelay(0.1f));
        }
    }

    void LateUpdate()
    {
        Vector3 dir = new Vector3(targetH.Target.position.x, transform.position.y, targetH.Target.position.z);

        transform.LookAt(dir);
    }

    private Vector3 RoundVecTo1D(Vector3 vec)
    {
        float f1 = Mathf.Round(vec.x * 100) * 0.01f;
        float f2 = Mathf.Round(vec.y * 100) * 0.01f;
        float f3 = Mathf.Round(vec.z * 100) * 0.01f;

        return new Vector3(f1, f2, f3);
    }

    private IEnumerator GetDestinationWithDelay(float time)
    {
        yield return new WaitForSeconds(time);
        agent.SetDestination(targetH.Target.position);
        StartCoroutine(GetDestinationWithDelay(time));
    }

    private void DrawDebugRays()
    {

    }
}
