using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    // control and switch animations for characters
    public void TriggerMovementAnimation(Vector2 movement, Animator animator, Transform target, Vector2 previousMovement)
    {
        Debug.Log(movement);

        // if movement is changed, set to idle state
        if (previousMovement != movement)
        {
            StopMovementAnimation(animator);
        }

        animator.SetBool("isMoving", movement != Vector2.zero);
        animator.SetBool("isMoving_Horizontal", movement.x != 0);
        animator.SetFloat("Vertical", movement.y);

        if (movement.x > 0)
        {
            target.localScale = new Vector2(-1, 1);
        }
        else
        {
            target.localScale = new Vector2(1, 1);
        }
    }

    public void StopMovementAnimation(Animator animator)
    {
        // this will trigger idle animation state
        animator.SetTrigger("Reset");
    }
}
