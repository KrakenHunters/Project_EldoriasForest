using UnityEngine;
public class pIdleState : BaseState
{
    PlayerController player;
    LayerMask groundLayer = LayerMask.GetMask("Ground");

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
        Rotate();
    }

    public override void StateUpdate()
    {

    }

    public override void HandleMovement(Vector2 d)
    {
        character.ChangeState(new pMoveState());
    }
    
    public override void HandleAttack()
    {
        player.ChangeState(new pAttackState());
    }
    public override void HandleInteract()
    {
        player.ChangeState(new pInteractState());
    }



    private void Rotate()
    {
        if (player.isInCombat)
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                Vector3 target = hit.point;
                Vector3 direction = target - player.transform.position;
                direction.y = 0;
                player.gameObject.transform.rotation = Quaternion.Slerp(player.gameObject.transform.rotation, Quaternion.LookRotation(direction), player.RotationSpeed);
            }


        }
    }


}
