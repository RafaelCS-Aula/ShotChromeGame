using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private Waypoint currentWaypoint;

    [SerializeField] private TargetHolder targetH;

    [SerializeField] private Transform shotOrigin;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;

    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private BoolVariable shootWhileMoving;


    private Vector3 movDest;
    private Vector3 movStart;
    private bool isMoving;
    private bool isRotating;
    private float moveLerpStartTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.position = currentWaypoint.transform.position;
        targetH = GetComponent<TargetHolder>();
    }

    private void Update()
    {
        if (!isMoving && Input.GetKeyDown(KeyCode.T))
        {
            ChangeWaypoint();
        }

        if (isMoving) Move(movDest);

        //if (shootWhileMoving) RotateToPoint(target.position);
        //else if (!isMoving) RotateToPoint(target.position);
        //print(CheckForVisual(transform.position));
        //print("RUNNING");

    }

    private void ChangeWaypoint()
    {
        bool validWaypoint = false;
        int waypointsReviewed = 0;
        Waypoint nextWaypoint;

        while (!validWaypoint)
        {
            int next = Random.Range(0, currentWaypoint.outgoingConnections.Count - 1);
            nextWaypoint = currentWaypoint.outgoingConnections[next];

            //if (CheckForVisual(nextWaypoint.transform.position));

            waypointsReviewed++;
            if (waypointsReviewed == currentWaypoint.outgoingConnections.Count && !validWaypoint) return;
        }


        //movDest = waypoints[nextWaypoint].transform.position;
        //movStart = waypoints[currentWaypoint].transform.position;

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

            if (percentageComplete >= 1.0f) isMoving = false;
        }

    }

    private bool CheckForVisual(Vector3 pos)
    {
        Vector3 originOffset = shotOrigin.position - pos;

        Vector3 originP = pos + originOffset;

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

    private void OnDrawGizmos()
    {

    }
}