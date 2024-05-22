using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class pAttackState : BaseState
{
    PlayerController player;
    Vector3 _direction;

    private SpellBook activeSpell;

    private float timer;

    LayerMask groundLayer = LayerMask.GetMask("Ground");
    public override void EnterState()
    {
        Debug.Log("Enter Move");
        player = character.GetComponent<PlayerController>();
        CheckAttackType();
        player.ResetCombatTimer();
        //Animate ad change to new state and cast spell after animation is done

    }
    public override void ExitState()
    {

    }

    public override void StateFixedUpdate()
    {
        player.c.Move(_direction.normalized * player.Speed * player.SpeedModifier * Time.fixedDeltaTime);
        Rotate();

        timer += Time.deltaTime;
        float clipLength = 0f;//player.anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        if (timer >= clipLength)
        {
            timer = 0f;

            player.CastSpell(activeSpell);
            player.ChangeState(new pIdleState());
        }

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
            player.gameObject.transform.rotation = Quaternion.Slerp(player.gameObject.transform.rotation, Quaternion.LookRotation(direction), player.RotationSpeed );
        }
    }



    private void CheckAttackType()
    {
        switch (player.attackType)
        {
            case PlayerController.AttackType.Base:
                Debug.Log("Base Attack");
                activeSpell = player.tempData.baseSpell;
                break;
            case PlayerController.AttackType.Special:
                activeSpell = player.tempData.specialSpell;
                Debug.Log("Special Attack");
                break;
            case PlayerController.AttackType.Ultimate:
                activeSpell = player.tempData.ultimateSpell;
                Debug.Log("Ultimate Attack");
                break;
        }
    }

}
