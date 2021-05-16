using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(TargetHolder))]
public class WaypointMovement : MonoBehaviour
{
    protected Animator anim;
    protected TargetHolder targetHolder;

    [SerializeField] protected Waypoint currentWayPoint;

    [SerializeField] protected Transform lineOfSightSource;

    [SerializeField] protected FloatVariable moveSpeed;
    [SerializeField] protected FloatVariable turnSpeed;

    [SerializeField] protected BoolVariable turnToTargetWhileMoving;

    [SerializeField] protected FloatVariable timeToMove;

    [SerializeField] protected FloatVariable attackDelayOnArrival;


    protected Vector3 movDest;
    protected Vector3 movStart;
    protected bool isMoving;
    protected bool isRotating;
    protected float moveLerpStartTime;

    protected float visualTimer;

    [SerializeField] protected GameObject CloneFrebab;
    protected GameObject clone;

    [Button]
    public void ShowDebugs()
    {
        print(targetHolder.HasLineOfSightToTarget(lineOfSightSource.transform));
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        targetHolder = GetComponent<TargetHolder>();

        transform.position = currentWayPoint.transform.position;

        clone = Instantiate(CloneFrebab, transform.position, transform.rotation, transform) ;

        visualTimer = timeToMove;
        ChangeWaypoint(true);
    }

    private void Update()
    {
        if(anim != null)
            anim.SetBool("isMoving", isMoving);

        if (!isMoving)
        {
            if (!targetHolder.HasLineOfSightToTarget(lineOfSightSource.transform)) 
            {
                visualTimer -= Time.deltaTime;
            }
            else if (visualTimer != timeToMove) visualTimer = timeToMove;

            if (visualTimer <= 0)
            {
                ChangeWaypoint(false);
            }
        }

        if (isMoving) Move(movDest);

        if (turnToTargetWhileMoving) RotateToPoint(targetHolder.Target.position);
        else if (!isMoving) RotateToPoint(targetHolder.Target.position);
    }

    protected void ChangeWaypoint(bool isStart)
    {
        Waypoint nextWaypoint;
        List<Waypoint> canGo = new List<Waypoint>();

        for (int i = 0; i < currentWayPoint.outgoingConnections.Count; i++)
        {
            if(currentWayPoint.outgoingConnections[i] == null)
                continue;

            if ((isStart && !currentWayPoint.outgoingConnections[i].isOccupied) ||
                (!currentWayPoint.outgoingConnections[i].isOccupied))
            {
                Transform originP = SendCloneToPoint(currentWayPoint.outgoingConnections[i].transform.position);        
                if(targetHolder.HasLineOfSightToTarget(originP))
                {
                    
                    canGo.Add(currentWayPoint.outgoingConnections[i]);
                }
                
            }
        }

        if (canGo.Count == 0)
        {
            for (int i = 0; i < currentWayPoint.outgoingConnections.Count; i++)
            {
                if(currentWayPoint.outgoingConnections[i] == null)
                    continue;
                if (!currentWayPoint.outgoingConnections[i].isOccupied)
                {
                    for (int j = 0; j < currentWayPoint.outgoingConnections[i].outgoingConnections.Count; j++)
                    {
                        if (!currentWayPoint.outgoingConnections[i].outgoingConnections[j].isOccupied)
                        {

                            Transform originP = SendCloneToPoint(currentWayPoint.outgoingConnections[i].outgoingConnections[j].transform.position);

                            // Test if clone has line of sight
                            if(targetHolder.HasLineOfSightToTarget(originP))
                            {
                                canGo.Add(currentWayPoint.outgoingConnections[i]);
                            }
                            
                        }
                    }
                }
            }
        }

        if (canGo.Count == 0) return;

        visualTimer = timeToMove;

        nextWaypoint = canGo[Random.Range(0, canGo.Count)];

        movDest = nextWaypoint.transform.position;
        movStart = currentWayPoint.transform.position;

        currentWayPoint.ToggleOccupation();
        currentWayPoint = nextWaypoint;
        currentWayPoint.ToggleOccupation();

        isMoving = true;
        if (turnToTargetWhileMoving) moveLerpStartTime = Time.time;
        else isRotating = true;
    }
    protected virtual void Move(Vector3 end)
    {
        if (isRotating) RotateToPoint(movDest);

        else if (isMoving)
        {
            float secsToMove = Vector3.Distance(movStart, end) / moveSpeed;
            float timeSinceStarted = Time.time - moveLerpStartTime;
            float percentageComplete = timeSinceStarted / secsToMove;

            transform.position = Vector3.Lerp(movStart, end, percentageComplete);

            if (percentageComplete >= 1.0f)
            {
                isMoving = false;

                StartCoroutine(GetComponent<LineOfSightAttack>().LockAttackForSeconds(attackDelayOnArrival));
            }
        }

    }

    /// <summary>
    /// Sends clone to a point and returns the transform of the first child 
    /// object
    /// </summary>
    /// <param name="destiny"></param>
    /// <returns>Transform of the first child object of the clone</returns>
    private Transform SendCloneToPoint(Vector3 destiny)
    {
        // Send clone
        clone.transform.position = destiny;
        clone.transform.LookAt(targetHolder.Target);
        return clone.transform.GetChild(0).transform;
    }

    private void RotateToPoint(Vector3 point)
    {
        Vector3 dir = point - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (isMoving && Mathf.Abs(lookRotation.eulerAngles.y - transform.rotation.eulerAngles.y) <= 0.01f)
        {
            isRotating = false;
            moveLerpStartTime = Time.time;
        }
    }

    public void SetCurrentWaypoint(Waypoint wp) { currentWayPoint = wp; }
    public Waypoint GetCurrentWaypoint() { return currentWayPoint; }

    public void SetMoving(bool value) { isMoving = value; }
}
