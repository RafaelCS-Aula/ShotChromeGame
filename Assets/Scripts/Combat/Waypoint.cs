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
            Handles.DrawDottedLine(transform.position, wp.transform.position,2f);
            
        }
    }
}
