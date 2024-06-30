using UnityEngine;

public class PlayerMoveState : BaseState
{
    public override void EnterState()
    {
        base.EnterState();

        player.animator.CrossFade(MovementHash, 0.2f);

    }
    public override void ExitState()
    {

    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
        float t = lerpTimer / lerpDuration;
        currentSpeed = Mathf.Lerp(initialSpeed, player.Speed, t);
        player.c.SimpleMove(_direction.normalized * currentSpeed);

        Rotate();
    }

    public override void StateUpdate()
    {

    }

    public override void HandleMovement(Vector2 dir)
    {
        _direction = new Vector3(dir.x,0,dir.y);
        player.animator.SetFloat("Horizontal", (currentSpeed / player.Speed) * _direction.x);
        player.animator.SetFloat("Vertical", (currentSpeed / player.Speed) * _direction.z);

    }

    private void Rotate()
    {
        if (_direction != Vector3.zero)
            player.gameObject.transform.rotation = Quaternion.RotateTowards(player.gameObject.transform.rotation, Quaternion.LookRotation(_direction), player.RotationSpeed);
    }


}
