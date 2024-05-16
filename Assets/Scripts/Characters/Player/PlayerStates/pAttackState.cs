using UnityEngine;

public class pAttackState : BaseState
{
    PlayerController player;
    Vector3 _direction;

    private SpellBook activeSpell;

    LayerMask groundLayer = LayerMask.GetMask("Ground");
    public override void EnterState()
    {
        Debug.Log("Enter Move");
        player = character.GetComponent<PlayerController>();
        CheckAttackType();
        player.isInCombat = true;
        //Animate

    }
    public override void ExitState()
    {

    }

    public override void StateFixedUpdate()
    {
        player.c.Move(_direction * player.Speed * player.SpeedModifier * Time.fixedDeltaTime);
        Rotate();
    }

    public override void StateUpdate()
    {
     
    }

    public override void HandleMovement(Vector2 dir)
    {
        _direction = new Vector3(dir.x, 0, dir.y);
    }
    private void Rotate()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,Mathf.Infinity,groundLayer))
        {
            Vector3 target = hit.point;
            Vector3 direction = target - player.transform.position;
            direction.y = 0;
            player.gameObject.transform.rotation = Quaternion.Slerp(player.gameObject.transform.rotation, Quaternion.LookRotation(direction), player.RotationSpeed * Time.fixedDeltaTime);
        }
    }



    private void CheckAttackType()
    {
        switch (player.attackType)
        {
            case PlayerController.AttackType.Base:
                Debug.Log("Base Attack");
                break;
            case PlayerController.AttackType.Special:
                Debug.Log("Special Attack");
                break;
            case PlayerController.AttackType.Ultimate:
                Debug.Log("Ultimate Attack");
                break;
        }
    }
}
