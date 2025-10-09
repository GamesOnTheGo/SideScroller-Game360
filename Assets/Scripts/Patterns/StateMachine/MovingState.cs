using UnityEngine;

public class MovingState : PlayerState
{
    public override void EnterState(PlayerController player)
    {
        TryPlayAnimation(player, "Run");
    }

    public override void UpdateState(PlayerController player)
    {
        float horizontal = Input.GetAxis("Horizontal");

        Vector2 velocity = player.rb.linearVelocity;
        velocity.x = horizontal * player.moveSpeed;
        player.rb.linearVelocity = velocity;

        if (horizontal < 0)
            player.spriteRenderer.flipX = true;
        else if (horizontal > 0)
            player.spriteRenderer.flipX = false;

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGrounded())
        {
            player.ChangeState(new JumpingState());
        }
        else if (Mathf.Abs(horizontal) < 0.1f)
        {
            player.ChangeState(new IdleState());
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            player.Fire();
        }
    }

    public override void ExitState(PlayerController player) { }

    public override string GetStateName() => "Moving";

    private void TryPlayAnimation(PlayerController player, string animName)
    {
        if (player.animator != null &&
            player.animator.runtimeAnimatorController != null &&
            player.animator.isActiveAndEnabled)
        {
            try
            {
                player.animator.Play(animName);
            }
            catch
            {
                // Animation doesn't exist - continue without it
            }
        }
    }
}
