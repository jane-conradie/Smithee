using TMPro;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [Header("Interactables")]
    [SerializeField] private GameObject anvilCanvas;
    [SerializeField] private GameObject registerCanvas;
    [SerializeField] private GameObject sellCanvas;

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
            case "Customer":
                //  grab canvas
                //Customer customer = other.gameObject.GetComponent<Customer>();
                //canvas = customer.canvas;
                canvas = sellCanvas;
                break;
            case "Sellable":
                Canvas canvasObject = other.gameObject.GetComponentInChildren<Canvas>(true);
                canvas = canvasObject.gameObject;
                break;
            default:
                break;
        }

        return canvas;
    }

    public void ToggleVisibility(GameObject objectToToggle)
    {
        objectToToggle.SetActive(!objectToToggle.activeSelf);
    }
}
