using TMPro;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [Header("Interactables")]
    [SerializeField] private GameObject anvilCanvas;
    [SerializeField] private GameObject registerCanvas;


    public GameObject GetInteractableCanvas(Collider2D other)
    {
        GameObject canvas = null;

        switch (other.tag)
        {
            case "Anvil":
                canvas = anvilCanvas;
                break;
            case "Cash Register":
                canvas = registerCanvas;
                break;
            default:
                break;
        }

        return canvas;
    }

    public GameObject GetCustomerCanvas(Customer customer)
    {
        return customer.canvas;
    }
}
