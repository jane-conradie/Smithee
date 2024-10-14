using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner instance;

    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private float moveSpeed = 2f;

    // spawn control
    [SerializeField] private float minSpawnTime = 1f;
    [SerializeField] private float maxSpawnTime = 3f;

    private bool canSpawn = true;
    private bool isSpawning = false;

    private List<Customer> customersInStore = new List<Customer>();

    PathManager pathManager;
    QueueManager queueManager;

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

    private void Start()
    {
        pathManager = PathManager.instance;
        queueManager = QueueManager.instance;
    }

    private void Update()
    {
        canSpawn = pathManager.GetNumberOfAvailablePaths() > 0;

        // spawn customer if there is an available path to take
        if (canSpawn && !isSpawning)
        {
            StartCoroutine(SpawnCustomer());
        }
    }

    // spawns a customer prefab every random number of seconds
    // assigns a path and move speed to the instance of a customer
    private IEnumerator SpawnCustomer()
    {
        isSpawning = true;

        // wait before spawning the customer
        yield return new WaitForSecondsRealtime(Random.Range(minSpawnTime, maxSpawnTime));

        // randomise path
        PathsSO path = pathManager.GetRandomAvailablePath();

        // use prefab and spawn a customer at their path spawn point
        GameObject instance = Instantiate(customerPrefab, path.GetSpawnPoint().transform.position, Quaternion.identity);
        Customer customer = instance.GetComponent<Customer>();

        // set that customer object's path
        customer.path = path;
        // set customer move speed
        customer.moveSpeed = moveSpeed;

        // add instance to list of customers
        customersInStore.Add(customer);

        isSpawning = false;

        yield return null;
    }

    public void ClearCustomers()
    {
        // clear customer queue list
        queueManager.ClearCustomerInQueue();

        // remove each customer in store
        foreach (Customer customer in customersInStore)
        {
            customer.DestroySelf();
        }

        // clear list of customers
        customersInStore.Clear();
    }

      public void RemoveCustomerFromStore(Customer customer)
    {
        customersInStore.Remove(customer);
    }
}
