using UnityEngine;
public class PlayerIdleState : BaseState
{
    PlayerController player;
    
    public override void EnterState()
    {
        player = character.GetComponent<PlayerController>();

        //Animate

    }
    public override void ExitState()
    {

    }

    public override void StateFixedUpdate()
    {
        player.c.Move(_direction.normalized * player.Speed*Time.fixedDeltaTime);
        RotateToTarget();
    }

    public override void StateUpdate()
    {

    }

    public override void HandleMovement(Vector2 dir)
    {
        _direction = new Vector3(dir.x,0,dir.y);
       // character.ChangeState(new PlayerMoveState());
    }
    
    public override void HandleAttack()
    {
        player.ChangeState(new PlayerAttackState());
    }
    public override void HandleInteract()
    {
        player.ChangeState(new PlayerInteractState());
    }






}
