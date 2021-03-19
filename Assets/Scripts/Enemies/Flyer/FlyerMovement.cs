using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private Waypoint currWP;

    [SerializeField] private TargetHolder targetH;

    [SerializeField] private Transform shotOrigin;

    [SerializeField] private FloatVariable moveSpeed;
    [SerializeField] private FloatVariable turnSpeed;

    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private BoolVariable shootWhileMoving;


    [SerializeField] private FloatVariable timeToMove;

    [SerializeField] private IntVariable attackDelayOnArrival;


    private Vector3 movDest;
    private Vector3 movStart;
    private bool isMoving;
    private bool isRotating;
    private float moveLerpStartTime;

    private float visualTimer;

    [SerializeField] private GameObject FlyerFrebab;
    private GameObject clone;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.position = currWP.transform.position;

        clone = Instantiate(FlyerFrebab, transform);
        visualTimer = timeToMove;
        ChangeWaypoint();
    }

    private void Update()
    {

        if (!isMoving)
        {
            if (!CheckForVisual(true, transform.position))
            {
                visualTimer -= Time.deltaTime;
            }
            else if (visualTimer != timeToMove) visualTimer = timeToMove;

            if (visualTimer <= 0) ChangeWaypoint();
        }

        if (isMoving) Move(movDest);

        if (shootWhileMoving) RotateToPoint(targetH.Target.position);
        else if (!isMoving) RotateToPoint(targetH.Target.position);
    }

    private void ChangeWaypoint()
    {
        Waypoint nextWaypoint;
        List<Waypoint> canGo = new List<Waypoint>();

        for (int i = 0; i < currWP.outgoingConnections.Count; i++)
        {
            if (!currWP.outgoingConnections[i].isOccupied &&
                CheckForVisual(false, currWP.outgoingConnections[i].transform.position))
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

        else
        {
            float secsToMove = Vector3.Distance(movStart, end) / moveSpeed;
            float timeSinceStarted = Time.time - moveLerpStartTime;
            float percentageComplete = timeSinceStarted / secsToMove;

            transform.position = Vector3.Lerp(movStart, end, percentageComplete);

            if (percentageComplete >= 1.0f)
            {
                isMoving = false;
                GetComponent<FlyerAttack>().LockAttackForSeconds(attackDelayOnArrival);
            }
        }

    }

    private bool CheckForVisual(bool thiWP, Vector3 pos)
    {
        Vector3 originP;

        if (!thiWP)
        {
            clone.SetActive(true);
            clone.transform.position = pos;
            clone.transform.LookAt(targetH.Target);
            originP = clone.GetComponent<FlyerAttack>().GetProjOrigin();
            clone.SetActive(false);
        }
        else originP = GetComponent<FlyerAttack>().GetProjOrigin();


        //Vector3 originP = pos + Vector3.forward + originOffset;

        Ray rayshow = new Ray(originP, targetH.Target.position - originP);
        RaycastHit hitinfo;

        Debug.DrawRay(originP, (targetH.Target.position - originP) * 3000, Color.red);

        if (Physics.Raycast(rayshow, out hitinfo, 3000))
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
}