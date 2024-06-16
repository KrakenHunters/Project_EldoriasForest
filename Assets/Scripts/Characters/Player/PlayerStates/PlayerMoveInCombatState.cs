using System.Threading;
using UnityEngine;
public class PlayerMoveInCombatState : BaseState
{
    
    public override void EnterState()
    {
        base.EnterState();
        //Animate
        timer = 0f;
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

        player.RotateToTarget();
    }

    public override void StateUpdate()
    {
        timer += Time.deltaTime;
        //Show UI loader here!

        if (timer >= player.combatCooldown)
        {
            player.ChangeState(new PlayerMoveState());
        }

    }

    public override void HandleMovement(Vector2 dir)
    {
        _direction = new Vector3(dir.x, 0, dir.y);
        //rotate
    }


}
