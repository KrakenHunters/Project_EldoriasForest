using UnityEngine;

public class pMoveState : BaseState
{
    PlayerController player;
    Vector3 _direction;
    Vector3 _rotation;
    LayerMask groundLayer = LayerMask.GetMask("Ground");

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
        if(player.isInCombat)
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                Vector3 target = hit.point;
                Vector3 direction = target - player.transform.position;
                direction.y = 0;
                player.gameObject.transform.rotation = Quaternion.Slerp(player.gameObject.transform.rotation, Quaternion.LookRotation(direction), player.RotationSpeed );
            }


        }
        else
        {
            if (_direction != Vector3.zero)
                player.gameObject.transform.rotation = Quaternion.Slerp(player.gameObject.transform.rotation, Quaternion.LookRotation(_direction), player.RotationSpeed);
        }
    }


   
    public override void HandleAttack()
    {
        player.ChangeState(new pAttackState());
    }

}
