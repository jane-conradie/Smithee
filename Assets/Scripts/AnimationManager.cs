using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    // control and switch animations for characters

    public void TriggerMovementAnimation(Vector2 movement, Animator animator, Transform target)
    {
        // only if there was a change
        if (movement != Vector2.zero)
        {
            // trigger walk animation
            animator.SetBool("isMoving", true);

            if (movement.x != 0)
            {
                animator.SetBool("isFacingUp", false);
                animator.SetBool("isFacingDown", false);

                animator.SetBool("isFacingSide", true);

                // check direction
                if (movement.x > 0)
                {
                    target.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    target.localScale = new Vector3(1, 1, 1);
                }
            }
            else if (movement.y != 0)
            {
                animator.SetBool("isFacingSide", false);

                // check direction
                if (movement.y > 0)
                {
                    animator.SetBool("isFacingDown", false);
                    // trigger up animation
                    animator.SetBool("isFacingUp", true);
                }
                else
                {
                    animator.SetBool("isFacingUp", false);
                    // trigger down animation
                    animator.SetBool("isFacingDown", true);
                }
            }
        }
        else
        {
            // trigger idle animation
            animator.SetBool("isMoving", false);
        }
    }
}
