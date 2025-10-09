using UnityEngine;

public class IdleState : PlayerState
{
    public override void EnterState(PlayerController player)
    {
        if (player.animator != null)
        {
            player.animator.Play("Idle");
        }
    }

    public override void UpdateState(PlayerController player)
    {
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGrounded())
        {
            player.ChangeState(new JumpingState());
        }
        else if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
        {
            player.ChangeState(new MovingState());
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            player.Fire();
        }
    }

    public override void ExitState(PlayerController player) { }

    public override string GetStateName() => "Idle";
}