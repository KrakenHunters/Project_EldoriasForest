using UnityEngine;

public class PlayerAttackState : BaseState
{

    private SpellBook activeSpell;
    private float spellDuration;

    private bool cancelAttack = false;

    private float clipLength;

    private bool mirrorAnim;

    public override void EnterState()
    {
        base.EnterState();
        timer = 10f;
        cancelAttack = false;
        player.animator.CrossFade(MovementHash, 0.2f);
        player.animator.Play(AttackHash);
        clipLength = 0.5f;
        mirrorAnim = true;
        CheckAttackType();
        //Animate ad change to new state and cast spell after animation is done

    }
    public override void ExitState()
    {

    }

    public override void StateFixedUpdate()
    {
        base.StateFixedUpdate();
    }

    public override void StateUpdate()
    {
        timer += Time.deltaTime;

        float t = lerpTimer / lerpDuration;
        currentSpeed = Mathf.Lerp(initialSpeed, player.Speed * player.SpeedModifier, t);
        player.c.SimpleMove(_direction.normalized * currentSpeed);

        player.RotateToTarget();
        if (timer >= clipLength)
        {
            mirrorAnim = !mirrorAnim;

            if (cancelAttack)
            {
                player.ChangeState(new PlayerMoveInCombatState());
            }
            else
            {

                player.CastSpell(activeSpell, out spellDuration);
                player.animator.Play(AttackHash);
                player.animator.SetBool("MirrorAnim", mirrorAnim);
                timer = 0f;

            }

        }

    }

    public override void HandleInteract()
    {

    }

    public override void HandleAttackCancel()
    {
        cancelAttack = true;
    }

    public override void HandleSpecialAttack()
    {

    }
    public override void HandleMovement(Vector2 dir)
    {
        _direction = new Vector3(dir.x, 0, dir.y);
        player.animator.SetFloat("Horizontal", (currentSpeed / player.Speed) * _direction.x);
        player.animator.SetFloat("Vertical", (currentSpeed / player.Speed) * _direction.z);

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
