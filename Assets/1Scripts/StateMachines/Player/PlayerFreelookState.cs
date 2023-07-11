using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreelookState : PlayerBaseState
{
    private bool shouldFade;
    private readonly int FreelookSpeedHash= Animator.StringToHash("FreeLookSpeed");
    private readonly int FreelookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 1.0f;

    public PlayerFreelookState(PlayerStateMachine stateMachine, bool shouldFade = true) : base(stateMachine)
     {
        this.shouldFade = shouldFade;
     }

    public override void Enter()
    {
        stateMachine.InputReceiver.TargetEvent += OnTarget;
        stateMachine.InputReceiver.JumpEvent += OnJump;
        stateMachine.Animator.SetFloat(FreelookSpeedHash, 0f);

        if(shouldFade)
        {
            stateMachine.Animator.CrossFadeInFixedTime(FreelookBlendTreeHash, CrossFadeDuration);
        }

        else
        {
            stateMachine.Animator.Play(FreelookBlendTreeHash);
        }
        
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

        if (stateMachine.InputReceiver.IsAttacking2)
        {
             if(stateMachine.MagicMode && stateMachine.Targeter.CurrentTarget!= null) 
            {
                stateMachine.SwitchState(new PlayerMagicAttackState(stateMachine, 0));
            return;
            }
        }

        if (stateMachine.InputReceiver.IsAttacking)
        {
             if(stateMachine.MagicMode && stateMachine.Targeter.CurrentTarget!= null) 
            {
                stateMachine.SwitchState(new PlayerLightningAttack(stateMachine, 0));
            return;
            }
        }

        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);

        if (stateMachine.InputReceiver.MovementValue == Vector2.zero)
        {

            stateMachine.Animator.SetFloat(FreelookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(FreelookSpeedHash, 1, AnimatorDampTime, deltaTime);

        FaceMovementDirection(movement, deltaTime);

    }

    public override void Exit()
    {
        stateMachine.InputReceiver.TargetEvent -= OnTarget;
        stateMachine.InputReceiver.JumpEvent -= OnJump;
    }

    private void OnTarget()
    {
        if (!stateMachine.Targeter.SelectTarget()) { return; }

        stateMachine.SwitchState(new PlayerTargetingState(stateMachine));

    }

    private Vector3 CalculateMovement()
    {
       Vector3 cameraForward =  stateMachine.MainCameraTransform.forward;
       Vector3 cameraRight = stateMachine.MainCameraTransform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        return cameraForward * stateMachine.InputReceiver.MovementValue.y +
            cameraRight * stateMachine.InputReceiver.MovementValue.x;

    }

    private void FaceMovementDirection(Vector3 movement, float dealtaTime)
    {
        stateMachine.transform.root.rotation = 
            Quaternion.Lerp(stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            dealtaTime * stateMachine.RotationSmoothingValue );
    }
     private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }

}
