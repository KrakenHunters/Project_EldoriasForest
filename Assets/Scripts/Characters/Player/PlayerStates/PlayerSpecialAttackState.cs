using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialAttack : BaseState
{
    private SpellBook activeSpell;
    private float spellDuration;

    public override void EnterState()
    {
        base.EnterState();
        timer = 10f;
        CheckAttackType();
        //Animate ad change to new state and cast spell after animation is done

    }
    public override void ExitState()
    {

    }

    public override void StateFixedUpdate()
    {
        player.c.SimpleMove(_direction.normalized * player.Speed * player.SpeedModifier);
        player.RotateToTarget();

        if (activeSpell.canUseBaseSpell)
        {
            float clipLength = 0.5f;//player.anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            if (timer >= clipLength)
            {
                timer = 0f;

                player.CastSpell(activeSpell, out spellDuration);
                player.ChangeState(new PlayerMoveInCombatState());
            }
        }
        else
        {
            float clipLength = 0.5f;//player.anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            if (timer >= clipLength)
            {
                timer = 0f;

                player.CastSpell(activeSpell, out spellDuration);
                if (timer >= spellDuration)
                {

                }

            }
        }

    }

    public override void StateUpdate()
    {
        timer += Time.deltaTime;
    }

    public override void HandleInteract()
    {

    }
    public override void HandleAttack()
    {

    }


    public override void HandleMovement(Vector2 dir)
    {
        _direction = new Vector3(dir.x, 0, dir.y);
    }
    private void CheckAttackType()
    {
        switch (player.attackType)
        {
            case PlayerController.AttackType.Base:
                activeSpell = player.tempData.baseSpell;
                break;
            case PlayerController.AttackType.Special:
                activeSpell = player.tempData.specialSpell;
                break;
            case PlayerController.AttackType.Ultimate:
                activeSpell = player.tempData.ultimateSpell;
                break;
        }
    }
}
