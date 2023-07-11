using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private Attack attack;
   // private Attack strikeAttack;
    private float previousFrameTime;
    int randomIndex;
    private bool alreadyAppliedForce = false;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine){
        
        attack = stateMachine.Attacks[attackIndex];
      //  strikeAttack = stateMachine.StrikeAttacks[attackIndex];
    }

    public override void Enter()
    {
      //  randomIndex = stateMachine.RandomizeArrayElementIndex(stateMachine.SwordAttackSounds);
      //  stateMachine.audioSource.PlayOneShot(stateMachine.SwordAttackSounds[randomIndex]);
         stateMachine.Slot1.SetActive(true);
        stateMachine.PlayRandomSound(stateMachine.SwordAttackSounds, stateMachine.PlayerAttackVolume);
        stateMachine.Weapon.SetAttack(attack.Damage, attack.KnockBack);
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
    }

 
    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        FaceTarget();

        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");

        if (normalizedTime >= attack.ForceTime)
        {
            TryApplyForce();
        }
       
        if (normalizedTime < 1f) 
        { 
            if(!stateMachine.MagicMode)
            {
            if(stateMachine.InputReceiver.IsAttacking)
            {
               // if(!stateMachine.MagicMode)
               // {
                    TryComboAttack(normalizedTime, attack);
              //  }
            }
            }

            //  if(stateMachine.InputReceiver.IsAttacking2)
            // {
            //     Debug.Log("** This SHOULD WORK***");
            //     TryComboAttack(normalizedTime, strikeAttack);
            // }
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
         stateMachine.Slot1.SetActive(false);
        
    }


    private void TryComboAttack(float normalizedTime, Attack attackArray)
    {
       if(attackArray.ComboStateIndex == -1) { return; }

       if(normalizedTime < attackArray.ComboAttackTime) { return; }

        stateMachine.SwitchState(new PlayerAttackingState (stateMachine,attackArray.ComboStateIndex));
    }

    private void TryApplyForce()
    {
        if(alreadyAppliedForce) { return; }
        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);
        alreadyAppliedForce = true;
    }

}
