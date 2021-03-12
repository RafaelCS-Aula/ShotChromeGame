using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Waypoint[] possibleDestinations;

    public bool isOccupied { get; private set; }
    public Waypoint GetWaypointAt(int i) { return possibleDestinations[i]; }
    public void ToggleOccupation() { isOccupied = !isOccupied; }
}
