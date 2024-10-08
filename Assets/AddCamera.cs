using UnityEngine;

public class AddCamera : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        // assign camera
        canvas.worldCamera = Camera.main;
    }
}
