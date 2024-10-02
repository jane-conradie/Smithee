using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create New Path", fileName = "Path")]
public class PathsSO : ScriptableObject
{
    [SerializeField] List<Transform> waypoints;
    [SerializeField] string destination;

    public List<Transform> GetWaypoints()
    {
        return waypoints;
    }

    public Transform GetSpawnPoint()
    {
        return waypoints[0];
    }
}
