using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private Collider2D objectCollider;

    private void Update() {
        
    }

    private void Start() {
        Debug.Log("this ishere");
    }

    // [SerializeField] private TextMeshProUGUI minigameText;
    // [SerializeField] private Image image;
    // [SerializeField] Slider timeSlider;

    // private void Start()
    // {
    //     minigameText.text = "Click on the pieces until it matches the image on the left";
    // }

    // public void UpdateDisplay()
    // {
    //     minigameText.text = "Done!";
    // }

    // public void UpdateExampleSprite(Sprite sprite)
    // {
    //     image.sprite = sprite;
    // }

    // public void UpdateTimeSlider(float timeToComplete)
    // {
    //     timeSlider.value = timeToComplete;
    // }

    private void OnCollisionStay2D(Collision2D other) 
    {
        Debug.Log(other.gameObject.name);

        // check if colliding with sale object or anvil
        
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        Debug.Log(other.gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.gameObject.name);
    }

    // get customer colliding with item

    // start either anvil or sales game

    // cancel games

    // handle time passing
}
