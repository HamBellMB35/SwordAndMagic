using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStrikeAttackingState : PlayerBaseState
{
   // private Attack attack;
    private Attack strikeAttack; 
    private float previousFrameTime;
    private bool alreadyAppliedForce = false;

    public PlayerStrikeAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine){
        
       // attack = stateMachine.Attacks[attackIndex];
        strikeAttack = stateMachine.StrikeAttacks[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.Slot2.SetActive(true);
        stateMachine.PlayRandomSound(stateMachine.StrikeAttackSounds, stateMachine.PlayerAttackVolume);
        stateMachine.Weapon.SetAttack(strikeAttack.Damage, strikeAttack.KnockBack);
        stateMachine.Animator.CrossFadeInFixedTime(strikeAttack.AnimationName, strikeAttack.TransitionDuration);
    }

 
    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        FaceTarget();

        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "StrikeAttack"); 

        if (normalizedTime >= strikeAttack.ForceTime)
        {
            TryApplyForce();
        }
       
        if (normalizedTime < 1f) 
        { 
            if(!stateMachine.MagicMode)
            {
                if(stateMachine.InputReceiver.IsAttacking2)
                {
                // if(!stateMachine.MagicMode)
                // {
                    TryComboAttack(normalizedTime, strikeAttack);
                //  }
                }
            }
        }

        else
        {
            if(stateMachine.Targeter.CurrentTarget!= null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }

            else
            {
                stateMachine.SwitchState(new PlayerFreelookState(stateMachine));    
            }
        }

        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
        stateMachine.Slot2.SetActive(false);
    }

    private void TryComboAttack(float normalizedTime, Attack attackArray)
    {
       if(attackArray.ComboStateIndex == -1) { return; }

       if(normalizedTime < attackArray.ComboAttackTime) { return; }

        stateMachine.SwitchState
        (
            new  PlayerStrikeAttackingState
            (stateMachine,
                 attackArray.ComboStateIndex
             )
        );
    }

    private void TryApplyForce()
    {
        if(alreadyAppliedForce) { return; }
        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * strikeAttack.Force);
        alreadyAppliedForce = true;
    }

}
