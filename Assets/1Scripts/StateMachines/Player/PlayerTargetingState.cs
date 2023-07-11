using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForWardHash = Animator.StringToHash("TargetingForwardSpeed");
    private readonly int TargetingRightHash = Animator.StringToHash("TargettingRightSpeed");
    private const float CrossFadeDuration = 1.0f;
   
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.InputReceiver.TargetEvent += OnTarget;
        stateMachine.InputReceiver.DodgeEvent += OnDodge;
        stateMachine.InputReceiver.JumpEvent += OnJump;

        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReceiver.IsAttacking)
        {
            if(!stateMachine.MagicMode)
            {
                stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
                return;
            }
        }

        if (stateMachine.InputReceiver.IsAttacking2)
        {
            if(!stateMachine.MagicMode)
            {
                stateMachine.SwitchState(new PlayerStrikeAttackingState(stateMachine, 0));
                return;
            }
        }

         if (stateMachine.InputReceiver.IsAttacking)
        {
            if(stateMachine.MagicMode) 
            {
                stateMachine.SwitchState( new PlayerLightningAttack(stateMachine, 0));;
                return;
            }
        }

        if (stateMachine.InputReceiver.IsAttacking2)
        {
            if(stateMachine.MagicMode) 
            {
                stateMachine.SwitchState(new PlayerMagicAttackState(stateMachine, 0));
                return;
            }
        }

        if(stateMachine.InputReceiver.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }

       if(stateMachine.Targeter.CurrentTarget == null)
       {
            stateMachine.SwitchState(new PlayerFreelookState(stateMachine));
            return;
       }

        Vector3 movement = CalculateMovement(deltaTime);

        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget();
    }

    public override void Exit()
    {
                stateMachine.InputReceiver.TargetEvent -= OnTarget;

        stateMachine.InputReceiver.DodgeEvent -= OnDodge;
        stateMachine.InputReceiver.JumpEvent -= OnJump;

        //stateMachine.Animator.Play(TargetingBlendTreeHash);
        //stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }

    private void OnDodge()
    {
        if(stateMachine.InputReceiver.MovementValue == Vector2.zero) { return;}
        stateMachine.SwitchState(new PlayerDodgingState(stateMachine,stateMachine.InputReceiver.MovementValue));

    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }

    private void OnTarget()
    {
        stateMachine.Targeter.Cancel();
        stateMachine.SwitchState(new PlayerFreelookState(stateMachine));
    }

    private Vector3 CalculateMovement(float deltaTime)
    {
        Vector3 movement = new Vector3();

            movement += stateMachine.transform.right * stateMachine.InputReceiver.MovementValue.x;
            movement += stateMachine.transform.forward * stateMachine.InputReceiver.MovementValue.y;
        
        return movement;
    }


    private void UpdateAnimator(float deltaTime)
    {
        if(stateMachine.InputReceiver.MovementValue.y == 0)
        {
            stateMachine.Animator.SetFloat(TargetingForWardHash, 0, 0.1f, deltaTime);
        }

        else
        {
            float value = stateMachine.InputReceiver.MovementValue.y >0? 1f: -1f;
            stateMachine.Animator.SetFloat(TargetingForWardHash, value);
        }

        if (stateMachine.InputReceiver.MovementValue.x == 0)
        {
            stateMachine.Animator.SetFloat(TargetingRightHash, 0, 0.1f, deltaTime);
        }

        else
        {
            float value = stateMachine.InputReceiver.MovementValue.y > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingRightHash, value, 0.1f, deltaTime);
        }
    }


}
