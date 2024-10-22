using UnityEngine;

public class Sales : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    [SerializeField] private float indicatorMoveSpeed;
    // grab indicator

    // move back and forth

    // track when user clicks

    private void Update()
    {
        // move indicator

    }

    private void MoveIndicator()
    {
        Transform objectTransform = indicator.transform;

        objectTransform.Translate(Vector3.right * indicatorMoveSpeed * Time.deltaTime);
    }


}
