using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    // control and switch animations for characters
    public void TriggerMovementAnimation(Vector2 position, Animator animator, Transform target, Vector2 previousPosition)
    {
        // if movement is changed, set to idle state
        if (previousPosition != position)
        {
            StopMovementAnimation(animator);
        }

        animator.SetBool("isMoving", position != Vector2.zero);
        animator.SetFloat("Horizontal", position.x);
        animator.SetFloat("Vertical", position.y);
    }

    public void StopMovementAnimation(Animator animator)
    {
        // reset values
        animator.SetBool("isMoving", false);
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 0);

        // this will trigger idle animation state
        animator.SetTrigger("Reset");
    }
}
