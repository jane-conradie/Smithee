using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] List<PathsSO> paths;
    [SerializeField] GameObject customerPrefab;
    [SerializeField] float moveSpeed = 2f;

    // spawn control
    [SerializeField] float minSpawnTime = 3f;
    [SerializeField] float maxSpawnTime = 6f;

    int customerLimit = 6;
    int customersInStore = 0;
    bool isSpawning = false;

    void Update()
    {
        // spawn a customer if the store limit has not been hit 
        // and if another customer spawn is not in progress
        if ((customersInStore < customerLimit) && !isSpawning)
        {
            StartCoroutine(SpawnCustomer());
        }
    }

    // spawns a customer prefab every random number of seconds
    // assigns a path and move speed to the instance of a customer
    IEnumerator SpawnCustomer()
    {
        isSpawning = true;
        customersInStore++;

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
    PathsSO GetRandomPath()
    {
        return paths[Random.Range(0, paths.Count)];
    }

    void DespawnCustomer()
    {
        // TO DO decrease customers in store when done
        // destory customer
    }
}
