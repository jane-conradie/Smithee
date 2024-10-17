using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public static QueueManager instance;

    List<Customer> customersInQueue = new List<Customer>();

    ScoreKeeper scoreKeeper;

    GameManager gameManager;

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
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        gameManager = GameManager.instance;
    }

    public void AddCustomerToQueue(Customer customer)
    {
        customersInQueue.Add(customer);
    }

    public void RemoveCustomerFromQueue(Customer customer)
    {
        // if specific customer is in the list
        // remove them
        if (customersInQueue.Contains(customer))
        {
            customersInQueue.Remove(customer);
        }
    }

    public int GetTotalCustomersInQueue()
    {
        return customersInQueue.Count;
    }

    public void CheckoutCustomer()
    {
        if (customersInQueue.Count > 0)
        {
            // only if customer is at waypoint

            // let first customer leave
            Customer customer = customersInQueue[0];

            if (customer.reachedFront)
            {
                // remove path tied to customer
                customer.RemovePath();

                // trigger moving
                customer.isWaiting = false;
                customer.waypointIndex++;

                // remove first customer from queue
                customersInQueue.RemoveAt(0);

                // calculate customer payment
                float payment = customer.GetBasePayment();
                float tip = customer.CalculateCustomerPayment();

                // add money
                scoreKeeper.AddMoney(payment, tip);

                // track front customer position
                Vector3 targetPosition = customer.transform.position;

                // trigger moving all customers forward in the queue
                foreach (Customer cust in customersInQueue)
                {
                    Vector3 tempPosition = cust.transform.position;

                    StartCoroutine(cust.MoveForwardInQueue(targetPosition));

                    targetPosition = tempPosition;
                }

                // update customers served
                gameManager.customersServed++;
            }
        }
    }

    public void ClearCustomersInQueue()
    {
        // remove customers in queue list
        customersInQueue.Clear();
    }
}
