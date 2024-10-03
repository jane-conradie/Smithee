using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
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
            // let first customer leave
            Customer customer = customersInQueue[0];
            customer.isWaiting = false;
            customer.waypointIndex++;

            // remove first customer from queue
            customersInQueue.RemoveAt(0);

            // add money
            scoreKeeper.AddMoney();

            // track front customer position
            Vector3 targetPosition = customer.transform.position;

            // trigger moving all customers forward in the queue
            foreach (Customer item in customersInQueue)
            {
                Vector3 temp = item.transform.position;

                StartCoroutine(item.MoveForwardInQueue(targetPosition));

                targetPosition = temp;
            }
        }
    }
}
