using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using NaughtyAttributes;




public class Waypoint : MonoBehaviour
{
    private enum connectionWays{outgoing, incoming, twoway}
    private static int createdPoints = 0;
    private int myCreatedPointNR = 0;

    public Waypoint[] customWaypointList;
    public List<Waypoint> outgoingConnections = new List<Waypoint>();

    [ReadOnly]
    public List<Waypoint> incomingConnections = new List<Waypoint>();

    public bool isOccupied { get; private set; }
    //public Waypoint GetWaypointAt(int i) { return possibleDestinations[i]; }
    public void ToggleOccupation() { isOccupied = !isOccupied; }


    [Button]
    public void AddOneWayOutgoingConnection() => 
        MakeWaypoint(connectionWays.outgoing);

    [Button]
    public void AddOneWayIncomingConnection() =>
        MakeWaypoint(connectionWays.incoming);

    [Button]
    public void AddTwoWayConnection() => 
        MakeWaypoint(connectionWays.twoway);

    private void MakeWaypoint(connectionWays direction)
    {
        myCreatedPointNR = ++createdPoints;
        GameObject newObj = new GameObject($"Waypoint {myCreatedPointNR}");
        Waypoint newComp = newObj.AddComponent<Waypoint>();

        if(direction == connectionWays.outgoing)
        {
            outgoingConnections.Add(newComp);
            newComp.incomingConnections.Add(this);
        }
        else if(direction == connectionWays.incoming)
        {
            incomingConnections.Add(newComp);
            newComp.outgoingConnections.Add(this);
        }
        else if(direction == connectionWays.twoway)
        {
            outgoingConnections.Add(newComp);
            newComp.incomingConnections.Add(this);
            incomingConnections.Add(newComp);
            newComp.outgoingConnections.Add(this);
        }

        newObj.transform.position = transform.position;

    }

    private void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position,$"enemies/waypoint_icon.png", true);
        foreach(Waypoint wp in outgoingConnections)
        {
            Handles.color = Color.cyan;
            if(wp == null)
                continue;
            Handles.DrawDottedLine(transform.position, wp.transform.position,2f);

            Vector3 direction = (wp.transform.position - transform.position).normalized;
            if(wp.transform.position == transform.position)
                continue;

            Quaternion coneRotation = Quaternion.LookRotation(direction,transform.up);
            
            Handles.color = Color.cyan;
            Handles.ConeHandleCap(wp.GetInstanceID(),transform.position + direction * 1.2f, coneRotation,0.5f,EventType.Repaint);

            

        }
    }
}
