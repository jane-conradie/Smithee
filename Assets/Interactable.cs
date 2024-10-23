using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] public float id;
    public Customer customer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(customer);
        if (!customer)
        {
            // grab customer that has been collided with
            if (other.gameObject.tag == "Customer")
            {
                Customer tempCustomer = other.gameObject.GetComponent<Customer>();
                // check if this customer is for this interactable
                if (tempCustomer.path.interactable == id)
                {
                    customer = tempCustomer;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Customer")
        {
            if (customer && customer.path.interactable == id)
            {
                customer = null;
            }

            Debug.Log("customer left, should now be null and is: " + customer);
        }
    }
}
