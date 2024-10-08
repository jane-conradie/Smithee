using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI minigameText;

    private void Start()
    {
        minigameText.text = "Click on the pieces until it matches the image on the left";
    }

    public void UpdateDisplay()
    {
        minigameText.text = "Done!";
    }
}
