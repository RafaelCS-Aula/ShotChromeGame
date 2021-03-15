using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine.Events;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private bool hasGroup;

    [SerializeField] private Transform target;

    #region Seek
    [Header("-Seek Settings-")]
    [SerializeField] private FloatVariable seekSpeed;
    [SerializeField] private FloatVariable seekDist;

    private Vector3 vel;
    #endregion

    #region Wander
    [Header("-Wander Settings-")]

    [Tooltip("Wanders if it doesn't find the target. If unchecked, enemy will stay still.")]
    [SerializeField] private bool hasWander;

    [ShowIf("hasWander"), Tooltip("Speed at which the enemy moves when wandering")]
    [SerializeField] private FloatVariable wanderSpeed;

    [ShowIf("hasWander"), Tooltip("Time bewteen each change of direction when wandering")]
    [SerializeField] private FloatVariable dirChangeInterval;

    [ShowIf("hasWander"), Tooltip("Maximum angle change in direction when wandering")]
    [SerializeField] private FloatVariable maxDirChange;

    [ShowIf("hasWander"), Tooltip("")]
    [SerializeField] private FloatVariable wanderRadius;

    private Vector3 wanderOrigin;

    private bool isWandering = false;

    private float dirChangeTimer;

    private Quaternion rotation = Quaternion.identity;
    #endregion
    
    [Header("-Attack Settings-")]
    [SerializeField] FloatVariable attackRange;

    private CharacterController cC;



    private void Start()
    {
        vel = Vector3.zero;
        cC = GetComponent<CharacterController>();

        dirChangeTimer = 0;
    }

    private void Update()
    {
        float distToTarget = Vector3.Distance(target.position, transform.position);

        #region SeekDistanceRays
        Vector3 sDir1 = transform.forward.normalized;
        Vector3 sDir2 = -transform.forward.normalized;
        Vector3 sDir3 = transform.right.normalized;
        Vector3 sDir4 = -transform.right.normalized;
        Debug.DrawRay(transform.position, sDir1 * seekDist, Color.red);
        Debug.DrawRay(transform.position, sDir2 * seekDist, Color.red);
        Debug.DrawRay(transform.position, sDir3 * seekDist, Color.red);
        Debug.DrawRay(transform.position, sDir4 * seekDist, Color.red);
        #endregion
        #region MeleeDistanceRays
        Vector3 mDir1 = transform.forward.normalized;
        Vector3 mDir2 = -transform.forward.normalized;
        Vector3 mDir3 = transform.right.normalized;
        Vector3 mDir4 = -transform.right.normalized;
        Debug.DrawRay(transform.position, mDir1 * attackRange, Color.blue);
        Debug.DrawRay(transform.position, mDir2 * attackRange, Color.blue);
        Debug.DrawRay(transform.position, mDir3 * attackRange, Color.blue);
        Debug.DrawRay(transform.position, mDir4 * attackRange, Color.blue);
        #endregion

        if (distToTarget <= seekDist)
        {
            if (isWandering != false) isWandering = false;

            if (distToTarget > attackRange)
            {
                Seek();
            }
        }

        else if(hasWander)
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
        wantedVel = wantedVel.normalized * seekSpeed;

        //wantedVel = new Vector3(wantedVel.x, 0, wantedVel.z);

        Vector3 steering = wantedVel - vel;

        vel = Vector3.ClampMagnitude(vel + steering, seekSpeed);
        //transform.position += new Vector3(vel.x, 0, vel.z) * Time.deltaTime;
        cC.Move(vel * Time.deltaTime);

        if (vel != Vector3.zero) transform.forward = vel.normalized;

        Debug.DrawRay(transform.position, vel.normalized * 2, Color.green);
        Debug.DrawRay(transform.position, wantedVel.normalized * 2, Color.magenta);
    }

    private void Wander()
    {
        float distToOrigin = Vector3.Distance(transform.position, wanderOrigin); // For Later
        bool rotate = false;
        if (dirChangeTimer <= 0)
        {
            float min = transform.eulerAngles.y - maxDirChange;
            float max = transform.eulerAngles.y + maxDirChange;
            float dir = Random.Range(min, max);
            rotation = Quaternion.Euler(0, dir, 0);
            dirChangeTimer = dirChangeInterval;

            if (rotation.y < 0) rotation = Quaternion.identity;
            //transform.Rotate(rotation.eulerAngles);
            Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dirChangeInterval);

        }

        Vector3 move = transform.forward * wanderSpeed * Time.deltaTime;
        //transform.position += move * Time.deltaTime;
        cC.Move(move);


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

    private void CheckGroup()
    {

    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(wanderOrigin, 1);
    }
}
