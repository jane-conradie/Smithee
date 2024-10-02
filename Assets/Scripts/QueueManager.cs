using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    // trigger customers moving when previous customer has been checked out

    public static QueueManager instance;

    List<Customer> customersInQueue = new List<Customer>();

    ScoreKeeper scoreKeeper;

    void Awake()
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

    void Start()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public void AddCustomerToQueue(Customer customer)
    {
        customersInQueue.Add(customer);
    }

    public int GetTotalCustomersInQueue()
    {
        return customersInQueue.Count;
    }

    public void CheckoutCustomer()
    {
        if (customersInQueue.Count > 0)
        {
            // add money
            scoreKeeper.AddMoney();

            // get first customer to leave, and queue to move up
            Customer customer = customersInQueue[0];
            customer.isWaiting = false;

            // remove first customer from queue
            customersInQueue.RemoveAt(0);
        }
    }
}
