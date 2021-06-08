using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

public class JaguarMovement : MonoBehaviour
{
    private TargetHolder targetH;
    private EnemyHealth eHealth;

    private NavMeshAgent agent;
    private NavMeshPath path;
    private bool hasPath = false;

    [SerializeField] private FloatVariable chaseSpeed;
    [SerializeField] private FloatVariable stopDist;

    public bool globMoving = true;
    private bool moving = true;
    private bool hit = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        targetH = GetComponent<TargetHolder>();
        eHealth = GetComponent<EnemyHealth>();

        path = new NavMeshPath();

        agent.stoppingDistance = stopDist;
        agent.acceleration = float.MaxValue;
        agent.speed = chaseSpeed;
        agent.SetDestination(targetH.Target.position);
        StartCoroutine(GetDestinationWithDelay(true, targetH.Target.position, 0.1f));
    }

    void Update()
    {
        if (!globMoving)
        {
            moving = false;
            if (!hit) StopAllCoroutines();
            if ((!eHealth.Died())) agent.ResetPath();
        }

        if (globMoving && !moving && !hit)
        {
            moving = true;
            StartCoroutine(GetDestinationWithDelay(true, targetH.Target.position, 0.1f));
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

    public IEnumerator GetDestinationWithDelay(bool playerTarget, Vector3 destination, float delay)
    {
        yield return new WaitForSeconds(delay);
        if ((!eHealth.Died()))
        {
            print(destination);
            if (playerTarget) agent.SetDestination(targetH.Target.position);
            else agent.SetDestination(destination);
            StartCoroutine(GetDestinationWithDelay(playerTarget, destination, delay));
        }
    }

    public NavMeshAgent GetAgent() => agent;
    public float GetDefaultSpeed() => chaseSpeed;

    public void StopCR() { StopAllCoroutines(); }
    public IEnumerator HitKnockback(float secs)
    {
        print("HITKNOCKBACK");
        hit = true;
        Vector3 direction = -(targetH.Target.position - transform.position);


        agent.SetDestination(direction);
        agent.speed = chaseSpeed *5;

        yield return new WaitForSeconds(secs);
        print("CONTINUE");
        agent.speed = chaseSpeed;
        hit = false;

    }
    private void DrawDebugRays()
    {

    }
}
