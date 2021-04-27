using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerMovement : MonoBehaviour
{
    private Animator anim;
    private TargetHolder targetH;

    [SerializeField] private Waypoint currWP;

    [SerializeField] private Transform shotOrigin;

    [SerializeField] private FloatVariable moveSpeed;
    [SerializeField] private FloatVariable turnSpeed;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask visualCheckLayers;

    [SerializeField] private BoolVariable shootWhileMoving;


    [SerializeField] private FloatVariable timeToMove;

    [SerializeField] private FloatVariable attackDelayOnArrival;


    private Vector3 movDest;
    private Vector3 movStart;
    private bool isMoving;
    private bool isRotating;
    private float moveLerpStartTime;

    private float visualTimer;

    [SerializeField] private GameObject CloneFrebab;
    private GameObject clone;



    private void Start()
    {
        anim = GetComponent<Animator>();
        targetH = GetComponent<TargetHolder>();

        transform.position = currWP.transform.position;

        clone = Instantiate(CloneFrebab, transform.position, transform.rotation, transform) ;

        visualTimer = timeToMove;
        ChangeWaypoint(true);
    }

    private void Update()
    {
        anim.SetBool("isMoving", isMoving);

        if (!isMoving)
        {
            if (!CheckForVisual(true, transform.position)) 
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

        if (shootWhileMoving) RotateToPoint(targetH.Target.position);
        else if (!isMoving) RotateToPoint(targetH.Target.position);
    }

    private void ChangeWaypoint(bool isStart)
    {
        Waypoint nextWaypoint;
        List<Waypoint> canGo = new List<Waypoint>();

        for (int i = 0; i < currWP.outgoingConnections.Count; i++)
        {
            if ((isStart && !currWP.outgoingConnections[i].isOccupied) ||
                (!currWP.outgoingConnections[i].isOccupied && CheckForVisual(false, currWP.outgoingConnections[i].transform.position)))
            {
                canGo.Add(currWP.outgoingConnections[i]);
            }
        }

        if (canGo.Count == 0)
        {
            for (int i = 0; i < currWP.outgoingConnections.Count; i++)
            {
                if (!currWP.outgoingConnections[i].isOccupied)
                {
                    for (int j = 0; j < currWP.outgoingConnections[i].outgoingConnections.Count; j++)
                    {
                        if (!currWP.outgoingConnections[i].outgoingConnections[j].isOccupied &&
                            CheckForVisual(false, currWP.outgoingConnections[i].outgoingConnections[j].transform.position))
                        {
                            canGo.Add(currWP.outgoingConnections[i]);
                        }
                    }
                }
            }
        }

        if (canGo.Count == 0) return;

        visualTimer = timeToMove;

        nextWaypoint = canGo[Random.Range(0, canGo.Count)];

        movDest = nextWaypoint.transform.position;
        movStart = currWP.transform.position;

        currWP.ToggleOccupation();
        currWP = nextWaypoint;
        currWP.ToggleOccupation();

        isMoving = true;
        if (shootWhileMoving) moveLerpStartTime = Time.time;
        else isRotating = true;
    }
    private void Move(Vector3 end)
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

                StartCoroutine(GetComponent<FlyerAttack>().LockAttackForSeconds(attackDelayOnArrival));
            }
        }

    }

    private bool CheckForVisual(bool thiWP, Vector3 pos)
    {
        Vector3 originP;

        if (!thiWP)
        {
            clone.transform.position = pos;
            clone.transform.LookAt(targetH.Target);
            originP = clone.transform.GetChild(0).transform.position;
        }
        else originP = shotOrigin.position;


        //Vector3 originP = pos + Vector3.forward + originOffset;

        Ray rayshow = new Ray(originP, targetH.Target.position - originP);
        RaycastHit hitinfo;

        Debug.DrawRay(originP, (targetH.Target.position - originP) * 3000, Color.red);

        if (Physics.Raycast(rayshow, out hitinfo, 3000, visualCheckLayers))
        {
            if (hitinfo.collider != null)
            {
                if (playerLayer == (playerLayer | (1 << hitinfo.collider.gameObject.layer))) return true;
                else return false;
            }
        }
        return false;
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

    public void SetCurrentWaypoint(Waypoint wp) { currWP = wp; }
    public Waypoint GetCurrentWaypoint() { return currWP; }

    public void SetMoving(bool value) { isMoving = value; }

}