using UnityEngine;

public class pAttackState : BaseState
{
    PlayerController player;
    Vector3 _direction;
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
    }

    public override void StateUpdate()
    {
        Rotate();
    }

    public override void HandleMovement(Vector2 dir)
    {
        _direction = new Vector3(dir.x, 0, dir.y);
    }
    private void Rotate()
    {
        Vector3 mousePos = Input.mousePosition;
       
        Vector3 mousePositionWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y,Camera.main.transform.position.z));
        Debug.Log("MousePos" + mousePositionWorld);
        Vector3 direction = mousePositionWorld - player.transform.position;
        player.gameObject.transform.rotation = Quaternion.Slerp(player.gameObject.transform.rotation, Quaternion.LookRotation(direction), 0.2f);

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
