using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicAttackState : PlayerBaseState
{
   // private Attack attack;
    private Attack magicAttack; 
    private float previousFrameTime;
    private bool alreadyAppliedForce = false;

    public PlayerMagicAttackState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine){
        
       // attack = stateMachine.Attacks[attackIndex];
        magicAttack = stateMachine.MagicAttacks1[attackIndex];
    }

    public override void Enter()
    {
        stateMachine.Slot7.SetActive(true);
       // stateMachine.UI_ManagerMana.DecreaseHorizontalFill(stateMachine.UI_ManagerMana.imageBar);
       // stateMachine.PlayRandomSound(stateMachine.StrikeAttackSounds, stateMachine.PlayerAttackVolume);
        stateMachine.Weapon.SetAttack(magicAttack.Damage, magicAttack.KnockBack);
        stateMachine.Animator.CrossFadeInFixedTime(magicAttack.AnimationName, magicAttack.TransitionDuration);
    }

 
    public override void Tick(float deltaTime)
    {
        stateMachine.UI_ManagerMana.StartRefill();
     

        Move(deltaTime);

        FaceTarget();

        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "MagicAttack"); 

        if (normalizedTime >= magicAttack.ForceTime)
        {
            TryApplyForce();
        }
       
        if (normalizedTime < 1f) 
        { 
            if(stateMachine.MagicMode)
            {
                if(stateMachine.InputReceiver.IsAttacking2)
                {
                // if(!stateMachine.MagicMode)
                // {
                    TryComboAttack(normalizedTime, magicAttack);
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
        stateMachine.Slot7.SetActive(false);
        
    }

    private void TryComboAttack(float normalizedTime, Attack attackArray)
    {
       if(attackArray.ComboStateIndex == -1) { return; }

       if(normalizedTime < attackArray.ComboAttackTime) { return; }

        stateMachine.SwitchState
        (
            new  PlayerMagicAttackState
            (stateMachine,
                 attackArray.ComboStateIndex
             )
        );
    }

    private void TryApplyForce()
    {
        if(alreadyAppliedForce) { return; }
        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * magicAttack.Force);
        alreadyAppliedForce = true;
    }

}

