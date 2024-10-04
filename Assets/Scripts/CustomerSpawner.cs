using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner instance;

    [SerializeField] private List<PathsSO> paths;
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private float moveSpeed = 2f;

    // spawn control
    [SerializeField] private float minSpawnTime = 3f;
    [SerializeField] private float maxSpawnTime = 6f;

    private int customerLimit = 6;
    private int totalCustomersInStore = 0;
    private bool isSpawning = false;

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

        DontDestroyOnLoad(instance);
    }

    private void Update()
    {
        // spawn a customer if the store limit has not been hit 
        // and if another customer spawn is not in progress
        if ((totalCustomersInStore < customerLimit) && !isSpawning)
        {
            StartCoroutine(SpawnCustomer());
        }
    }

    // spawns a customer prefab every random number of seconds
    // assigns a path and move speed to the instance of a customer
    private IEnumerator SpawnCustomer()
    {
        isSpawning = true;
        totalCustomersInStore++;

        // wait before spawning the customer
        yield return new WaitForSecondsRealtime(Random.Range(minSpawnTime, maxSpawnTime));

        // randomise path
        PathsSO path = GetRandomPath();

        // use prefab and spawn a customer at their path spawn point
        GameObject instance = Instantiate(customerPrefab, path.GetSpawnPoint().transform.position, Quaternion.identity);
        Customer customer = instance.GetComponent<Customer>();

        // set that customer object's path
        customer.path = path;
        // set customer move speed
        customer.moveSpeed = moveSpeed;

        isSpawning = false;

        yield return null;
    }

    // returns a random path from the list of paths
    private PathsSO GetRandomPath()
    {
        return paths[Random.Range(0, paths.Count - 1)];
    }

    public void DespawnCustomer()
    {
        totalCustomersInStore--;
    }
}
