using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static PathManager instance;

    [SerializeField] private List<PathsSO> paths;

    float numOfAvailablePaths;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // reset paths
        ResetPaths();

        // set number of available paths
        numOfAvailablePaths = GetNumberOfAvailablePaths();
    }

    public float GetNumberOfAvailablePaths()
    {
        numOfAvailablePaths = paths.Where((x) => x.isInUse == false).Count();

        return numOfAvailablePaths;
    }

    public PathsSO GetRandomAvailablePath()
    {
        // get all paths that are not in use
        PathsSO[] availablePaths = paths.Where((x) => x.isInUse == false).ToArray();

        // get a random one
        PathsSO path = availablePaths[Random.Range(0, availablePaths.Length - 1)];

        // set the path as in use
        path.SetIsInUse(true);

        // return a random one
        return path;
    }

    public void ResetPaths()
    {
        foreach (PathsSO path in paths)
        {
            path.SetIsInUse(false);
        }
    }
}
