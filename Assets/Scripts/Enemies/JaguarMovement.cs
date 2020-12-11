using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaguarMovement : MonoBehaviour
{
    [SerializeField] private Transform target;

    #region Seek
    [SerializeField] private float bodyMass;
    [SerializeField] private float maxSeekVel;
    [SerializeField] private float maxForce;
    [SerializeField] private float seekDist;
    #endregion

    #region Wander

    [SerializeField] private float wanderSpeed;
    [SerializeField] private float dirChangeInterval;
    [SerializeField] private float maxHeadingChange;

    private Vector3 wanderOrigin;
    [SerializeField] private float wanderRadius;

    private bool isWandering = false;

    private float dirChangeTimer;

    private float heading;
    private Vector3 rotation = Vector3.zero;

    #endregion

    private Vector3 vel;


    private void Start()
    {
        vel = Vector3.zero;

        dirChangeTimer = 0;
    }

    private void Update()
    {
        float distToTarget = Vector3.Distance(target.position, transform.position);

        #region SeekDistanceRays
        Vector3 dir1 = transform.forward.normalized;
        Vector3 dir2 = -transform.forward.normalized;
        Vector3 dir3 = transform.right.normalized;
        Vector3 dir4 = -transform.right.normalized;
        Debug.DrawRay(transform.position, dir1 * seekDist, Color.red);
        Debug.DrawRay(transform.position, dir2 * seekDist, Color.red);
        Debug.DrawRay(transform.position, dir3 * seekDist, Color.red);
        Debug.DrawRay(transform.position, dir4 * seekDist, Color.red);
        #endregion

        if (distToTarget <= seekDist)
        {
            Seek();
            if (isWandering != false) isWandering = false;
        }

        else
        {
            Wander();
            if (isWandering != true)
            {
                isWandering = true;
                wanderOrigin = transform.position;
                dirChangeTimer = dirChangeInterval;
            }
            dirChangeTimer -= Time.deltaTime;
        }
    }

    private void Seek()
    {
        Vector3 wantedVel = target.position - transform.position;
        wantedVel = wantedVel.normalized * maxSeekVel;

        Vector3 steering = wantedVel - vel;

        steering = Vector3.ClampMagnitude(steering, maxForce) / bodyMass;

        vel = Vector3.ClampMagnitude(vel + steering, maxSeekVel);
        transform.position += new Vector3(vel.x, 0, vel.z) * Time.deltaTime;
        transform.forward = vel.normalized;

        Debug.DrawRay(transform.position, vel.normalized * 2, Color.green);
        Debug.DrawRay(transform.position, wantedVel.normalized * 2, Color.magenta);
    }

    private void Wander()
    {
        float distToOrigin = Vector3.Distance(transform.position, wanderOrigin); // For Later

        if (dirChangeTimer <= 0)
        {
            float min = transform.eulerAngles.y - maxHeadingChange;
            float max = transform.eulerAngles.y + maxHeadingChange;
            heading = Random.Range(min, max);
            rotation = new Vector3(0, heading, 0);
            dirChangeTimer = dirChangeInterval;
        }

        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, rotation, Time.deltaTime* dirChangeInterval);
        Vector3 move = transform.forward * wanderSpeed;
        transform.position += move * Time.deltaTime;


        #region WanderDistanceRays
        Vector3 dir1 = transform.forward.normalized;
        Vector3 dir2 = -transform.forward.normalized;
        Vector3 dir3 = transform.right.normalized;
        Vector3 dir4 = -transform.right.normalized;
        Debug.DrawRay(transform.position, dir1 * wanderRadius, Color.green);
        Debug.DrawRay(transform.position, dir2 * wanderRadius, Color.green);
        Debug.DrawRay(transform.position, dir3 * wanderRadius, Color.green);
        Debug.DrawRay(transform.position, dir4 * wanderRadius, Color.green);
        #endregion
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(wanderOrigin, 1);
    }
}
