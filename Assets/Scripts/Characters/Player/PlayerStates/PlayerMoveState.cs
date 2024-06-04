using UnityEngine;

public class PlayerMoveState : BaseState
{
    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("Enter Move");

        //Animate

    }
    public override void ExitState()
    {

    }

    public override void StateFixedUpdate()
    {
        base.EnterState();

        player.c.SimpleMove(_direction.normalized * player.Speed);
        Rotate();
    }

    public override void StateUpdate()
    {

    }

    public override void HandleMovement(Vector2 dir)
    {
        _direction = new Vector3(dir.x,0,dir.y);
    }

    private void Rotate()
    {
        if (_direction != Vector3.zero)
            player.gameObject.transform.rotation = Quaternion.RotateTowards(player.gameObject.transform.rotation, Quaternion.LookRotation(_direction), player.RotationSpeed);
    }


}
