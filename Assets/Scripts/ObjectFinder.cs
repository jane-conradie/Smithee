using UnityEngine;

public class ObjectFinder : MonoBehaviour
{
    public static ObjectFinder instance;

    [SerializeField] private GameObject statusHolder;
    [SerializeField] private GameObject statusPiece;
    [SerializeField] private GameObject canvas;

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

        DontDestroyOnLoad(gameObject);
    }
}
