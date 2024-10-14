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

    public int GetTotalCustomersInQueue()
    {
        return customersInQueue.Count;
    }

    public void CheckoutCustomer()
    {
        if (customersInQueue.Count > 0)
        {
            // update customers served
            gameManager.customersServed++;

            // let first customer leave
            Customer customer = customersInQueue[0];

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
        }
    }

    public void ClearCustomerInQueue()
    {
        // remove customers in queue list
        customersInQueue.Clear();
    }
}
