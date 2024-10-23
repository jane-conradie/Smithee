using Unity.VisualScripting;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] private float indicatorMoveSpeed;

    private float moveDirection = 1;
    public bool shouldMove = true;
    public string zone;

    private void Update()
    {
        // move indicator
        if (shouldMove)
        {
            MoveIndicator();
        }
    }

    private void MoveIndicator()
    {
        // move indicator
        transform.Translate(new Vector3(moveDirection, 0, 0) * indicatorMoveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        string tag = other.gameObject.tag;

        if (tag == "Indicator Holder")
        {
            // when hitting edge of holder, switch direction
            moveDirection = -moveDirection;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        zone = other.gameObject.tag;
    }

    public void ToggleMoveIndicator()
    {
        shouldMove = !shouldMove;
    }
}
