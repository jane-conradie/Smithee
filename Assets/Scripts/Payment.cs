using System.Collections;
using TMPro;
using UnityEngine;

public class Payment : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI basePaymentText;
   [SerializeField] private TextMeshProUGUI tipText;

   [SerializeField] private Transform textContainer;
   [SerializeField] private CanvasGroup canvas;

   [SerializeField] private float fadeDuration = 2f;
   [SerializeField] private float moveSpeed = 100f;
   [SerializeField] private float fadeSpeed = 5f;

   private void Start()
   {
        StartCoroutine(FadeOut());
   }

   public void UpdateDisplay(float payment, float tip)
   {
        basePaymentText.SetText(payment.ToString());
        tipText.SetText(tip.ToString());
   }

   private IEnumerator FadeOut()
   {
        float elapsedTime = 0f;
        // store the initial position
        Vector3 initialPosition = textContainer.transform.position; 

        float startAlpha = canvas.alpha;
        float targetAlpha = 0f;

        while (elapsedTime < fadeDuration)
        {
            // move the object upwards
            textContainer.transform.position = initialPosition + Vector3.up * (elapsedTime * moveSpeed);

            // fade on alpha
            canvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime * fadeSpeed);

            // increase elapsed time
            elapsedTime += Time.deltaTime; 
            yield return null;
        }

        // set final position
        transform.position = initialPosition + Vector3.up * (fadeDuration * moveSpeed);

        // set final alpha
        canvas.alpha = targetAlpha;

        // destroy
        Destroy(gameObject);
   }
}
