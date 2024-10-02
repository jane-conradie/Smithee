using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    // trigger customers moving when previous customer has been checked out

    public static QueueManager instance;

    List<Customer> customersInQueue = new List<Customer>();

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
        Debug.Log("checkout customer");

        // generate money score

        // add money

        // get first customer to leave, and queue to move up
        Customer customer = customersInQueue[0];
        customer.isWaiting = false;

        // remove first customer from queue
        customersInQueue.RemoveAt(0);

    }
}
