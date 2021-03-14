using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private List<Waypoint> possibleDestinations = new List<Waypoint>();

    public bool isOccupied { get; private set; }
    public Waypoint GetWaypointAt(int i) { return possibleDestinations[i]; }
    public void ToggleOccupation() { isOccupied = !isOccupied; }

    private void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position,$"enemies/waypoint_icon.png", true);
        foreach(Waypoint wp in possibleDestinations)
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
