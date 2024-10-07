using System.Collections;
using TMPro;
using UnityEngine;

public class Payment : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI basePaymentText;
   [SerializeField] private TextMeshProUGUI tipText;

   [SerializeField] private Transform textContainer;

   [SerializeField] private float fadeDuration = 2f;
   [SerializeField] private float fadeSpeed = 100f;

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

        while (elapsedTime < fadeDuration)
        {
            // move the object upwards
            textContainer.transform.position = initialPosition + Vector3.up * (elapsedTime * fadeSpeed);
            // increase elapsed time
            elapsedTime += Time.deltaTime; 
            yield return null;
        }

        // // set final position
        transform.position = initialPosition + Vector3.up * (fadeDuration * fadeSpeed);

        // destroy
        Destroy(gameObject);
   }
}
