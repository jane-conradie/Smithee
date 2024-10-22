using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create New Path", fileName = "Path")]
public class PathsSO : ScriptableObject
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] public bool isInUse;

    public List<Transform> GetWaypoints()
    {
        return waypoints;
    }

    public Transform GetSpawnPoint()
    {
        return waypoints[0];
    }

    public void SetIsInUse(bool isPathInUse)
    {
        isInUse = isPathInUse;
    }
}
