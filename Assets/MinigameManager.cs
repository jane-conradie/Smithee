using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI minigameText;

    [SerializeField] private Image image;

    private void Start()
    {
        minigameText.text = "Click on the pieces until it matches the image on the left";
    }

    public void UpdateDisplay()
    {
        minigameText.text = "Done!";
    }

    public void UpdateExampleSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
