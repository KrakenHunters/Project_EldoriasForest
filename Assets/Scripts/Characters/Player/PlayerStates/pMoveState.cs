using UnityEngine;

public class pMoveState : BaseState
{
    PlayerController player;
    Vector3 _direction;
    Vector3 _rotation;
    public override void EnterState()
    {
        Debug.Log("Enter Move");
        player = character.GetComponent<PlayerController>();

        //Animate

    }
    public override void ExitState()
    {

    }

    public override void StateFixedUpdate()
    {
        player.c.Move(_direction*player.Speed*Time.fixedDeltaTime);
        Rotate();
    }

    public override void StateUpdate()
    {

    }

    public override void HandleMovement(Vector2 dir)
    {
        _direction = new Vector3(dir.x,0,dir.y);
        //rotate
    }

    private void Rotate()
    {
        if (_direction != Vector3.zero)
            player.gameObject.transform.rotation = Quaternion.Slerp(player.gameObject.transform.rotation, Quaternion.LookRotation(_direction), 0.2f);

    }


   
    public override void HandleAttack()
    {
        player.ChangeState(new pAttackState());
    }

}
