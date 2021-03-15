using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using NaughtyAttributes;




public class Waypoint : MonoBehaviour
{
    private enum connectionWays{outgoing, incoming, twoway}
    private static int createdPoints = 0;
    private int myCreatedPointNR = 0;

    public bool drawPathGizmos = true;
    public Waypoint[] customWaypointList;
    public List<Waypoint> outgoingConnections = new List<Waypoint>();

    [ReadOnly]
    public List<Waypoint> incomingConnections = new List<Waypoint>();

    public bool isOccupied { get; private set; }
    //public Waypoint GetWaypointAt(int i) { return possibleDestinations[i]; }
    public void ToggleOccupation() { isOccupied = !isOccupied; }


    [Button]
    public void CreateOneWayOutgoingConnection() => 
        MakeWaypoint(connectionWays.outgoing);

    [Button]
    public void CreateOneWayIncomingConnection() =>
        MakeWaypoint(connectionWays.incoming);

    [Button]
    public void CreateTwoWayConnection() => 
        MakeWaypoint(connectionWays.twoway);

    [Button]
    public void MakeOneWayOutgoingConnectionWith(Waypoint other = null)
    {
        if(other == null)
        {
            foreach(Waypoint wp in customWaypointList)
                AddWaypoint(wp,connectionWays.outgoing);
        }
        else
            AddWaypoint(other,connectionWays.outgoing);
    }
    [Button]
    public void MakeOneWayIncomingConnectionWith(Waypoint other = null)
    {
        if(other == null)
        {
            foreach(Waypoint wp in customWaypointList)
                AddWaypoint(wp,connectionWays.incoming);
        }
        else
            AddWaypoint(other,connectionWays.incoming);
    }
    
    [Button]
    public void MakeTwoWayConnectionWith(Waypoint other = null)
    {
        if(other == null)
        {
            foreach(Waypoint wp in customWaypointList)
                AddWaypoint(wp,connectionWays.twoway);
        }
        else
            AddWaypoint(other,connectionWays.twoway);
    }

    private void MakeWaypoint(connectionWays direction)
    {
        myCreatedPointNR = ++createdPoints;
        GameObject newObj = new GameObject($"Waypoint {myCreatedPointNR}");
        Waypoint newComp = newObj.AddComponent<Waypoint>();

        AddWaypoint(newComp, direction);

        newObj.transform.position = transform.position;

    }

    private void AddWaypoint(Waypoint other, connectionWays direction)
    {
        if(direction == connectionWays.outgoing)
        {
            outgoingConnections.Add(other);
            other.incomingConnections.Add(this);
        }
        else if(direction == connectionWays.incoming)
        {
            incomingConnections.Add(other);
            other.outgoingConnections.Add(this);
        }
        else if(direction == connectionWays.twoway)
        {
            outgoingConnections.Add(other);
            other.incomingConnections.Add(this);
            incomingConnections.Add(other);
            other.outgoingConnections.Add(this);
        }
    }

    private void OnDrawGizmos() {
        if(!drawPathGizmos)
            return;
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
