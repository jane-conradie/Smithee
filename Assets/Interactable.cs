using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Customer customer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // grab customer that has been collided with
        if (other.gameObject.tag == "Customer")
        {
            customer = other.gameObject.GetComponent<Customer>();
        }
    }
}
