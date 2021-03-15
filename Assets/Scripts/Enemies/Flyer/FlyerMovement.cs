using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private Waypoint[] waypoints;
    [SerializeField] private int startingIndex;

    [SerializeField] private Transform target;

    [SerializeField] private Transform shotOrigin;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask detectionLayers;

    [SerializeField] private BoolVariable shootWhileMoving;


    private int currentWaypoint;
    private Vector3 movDest;
    private Vector3 movStart;
    private bool isMoving;
    private bool isRotating;
    private float moveLerpStartTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentWaypoint = startingIndex;
        transform.position = waypoints[currentWaypoint].transform.position;


        detectionLayers = playerLayer.value | obstacleLayer.value;

    }

    private void Update()
    {
        if (!isMoving && Input.GetKeyDown(KeyCode.T))
        {
            ChangeWaypoint();
        }

        if (isMoving) Move(movDest);

        if (shootWhileMoving) RotateToPoint(target.position);
        else if (!isMoving) RotateToPoint(target.position);
        print(CheckForVisual(transform.position));

    }

    private void ChangeWaypoint()
    {
        int nextWaypoint;

        if (currentWaypoint == waypoints.Length - 1) nextWaypoint = 0;
        else nextWaypoint = currentWaypoint + 1;

        movDest = waypoints[nextWaypoint].transform.position;
        movStart = waypoints[currentWaypoint].transform.position;

        currentWaypoint = nextWaypoint;
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

        Ray rayshow = new Ray(originP, target.position - originP);
        RaycastHit hitinfo;

        Debug.DrawRay(originP,(target.position - originP) * 3000, Color.red);

        if (Physics.Raycast(rayshow, out hitinfo, 3000))
        {
            //print("ENTERED RAYCAST");
            if (hitinfo.collider != null)
            {
                //print(hitinfo.collider.gameObject.name);
                if (playerLayer == (playerLayer | (1 << hitinfo.collider.gameObject.layer)))
                {
                    //print("PLAYER");
                    return true;
                }
                else if (obstacleLayer == (obstacleLayer | (1 << hitinfo.collider.gameObject.layer)))
                {
                    //print("OBSTACLE");
                    return false;
                }
            }
        }
        return false;
        #region obs
        /*
        RaycastHit hitInfo;

        Vector3 originP = pos + shotOrigin.localPosition;

        
        if (Physics.Raycast(originP, Vector3.forward, out hitInfo, Mathf.Infinity))
        {
            Debug.DrawRay(originP, Vector3.forward * 30000, Color.red);
            if (hitInfo.transform.gameObject.layer == obstacleLayer)
            {
                print("GOT HERE - HIT OBSTACLE");
                return false;
            }
            else
            {
                print("GOT HERE - NO OBSTACLE");
                return true;
            }
            
        }
        //print("GOT HERE - NO DETECTION");
        return true;
        */
        #endregion
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