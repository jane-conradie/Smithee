using System;
using Unity.VisualScripting;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] private float minMoveSpeed = 1f;
    [SerializeField] private float maxMoveSpeed = 5f;
    [SerializeField] private float speedDeclineRatePerFrame = 1.5f;

    [SerializeField] private float xPositionDeviation = 5f;

    private float moveSpeed = 1f;
    private float moveDirection = 1;
    public bool shouldMove = true;
    public string zone;

    private void Start()
    {
        // start move speed on max
        moveSpeed = maxMoveSpeed;

        // randomise position 
        MoveToRandomPosition();
    }

    private void Update()
    {
        // move indicator
        if (shouldMove)
        {
            MoveIndicator();

            // calculate move speed
            CalculateMoveSpeed();
        }
    }

    private void MoveToRandomPosition()
    {
        // generate new position
        float positionDeviation = UnityEngine.Random.Range(-xPositionDeviation, xPositionDeviation);
        Vector3 newPosition = new Vector3(positionDeviation, transform.position.y, transform.position.z);

        transform.position = newPosition;
    }

    private void MoveIndicator()
    {
        // move indicator
        transform.Translate(new Vector3(moveDirection, 0, 0) * moveSpeed * Time.deltaTime);
    }

    private void CalculateMoveSpeed()
    {
        // each frame, slow down speed
        moveSpeed = Math.Clamp(moveSpeed -= speedDeclineRatePerFrame * Time.deltaTime, minMoveSpeed, maxMoveSpeed);
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
