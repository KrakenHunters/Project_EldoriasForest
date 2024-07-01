using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialAttack : BaseState
{
    private SpellBook activeSpell;
    private float spellDuration;
    private bool spellCast = false;

    public override void EnterState()
    {
        base.EnterState();
        timer = 0f;
        CheckAttackType();
        spellCast = false;
        player.animator.CrossFade(MovementHash, 0.2f);
        player.animator.Play(AttackHash);

        player.spellWeapon.InstantiateIndicator(activeSpell, player);


        //Animate ad change to new state and cast spell after animation is done

    }
    public override void ExitState()
    {

    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
        float t = lerpTimer / lerpDuration;
        currentSpeed = Mathf.Lerp(initialSpeed, player.Speed * player.SpeedModifier, t);
        player.c.SimpleMove(_direction.normalized * currentSpeed);

        player.RotateToTarget();

        timer += Time.deltaTime;

        if (activeSpell.canUseBaseSpell)
        {
            float clipLength = 0.5f;//player.anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            if (timer >= clipLength)
            {
                if (!spellCast)
                {
                    player.CastSpell(activeSpell, out spellDuration);
                    spellCast = true;
                }
                player.ChangeState(new PlayerMoveInCombatState());
            }
        }
        else
        {
            float clipLength = 0.5f;//player.anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            if (timer >= clipLength)
            {
                if (!spellCast)
                {
                    player.CastSpell(activeSpell, out spellDuration);
                    spellCast = true;
                }
                if (timer >= spellDuration)
                {
                    player.ChangeState(new PlayerMoveInCombatState());
                }

            }
        }

    }

    public override void StateUpdate()
    {

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
