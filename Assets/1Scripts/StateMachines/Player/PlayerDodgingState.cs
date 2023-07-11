using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");
    private readonly int DodgeForWardHash = Animator.StringToHash("DodgeForward");
    private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");
    private float reamainingDodgeTime;
    private Vector3 dodgingDirectionInput;

    private const float CrossFadeDuration = 0.1f;

    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput) 
    : base(stateMachine)
    {
        this.dodgingDirectionInput = dodgingDirectionInput;
    }
   

    public override void Enter()
    {
        reamainingDodgeTime = stateMachine.DodgeDuration;
        stateMachine.Animator.SetFloat(DodgeForWardHash, dodgingDirectionInput.y);
        stateMachine.Animator.SetFloat(DodgeRightHash, dodgingDirectionInput.x);
        stateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash, CrossFadeDuration);

        stateMachine.Health.SetInvulnerable(true);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * dodgingDirectionInput.x *
            stateMachine.DodgeDistance / stateMachine.DodgeDuration;

        movement += stateMachine.transform.forward * dodgingDirectionInput.y *
            stateMachine.DodgeDistance / stateMachine.DodgeDuration; 

        Move(movement, deltaTime);

        FaceTarget();

        reamainingDodgeTime -= deltaTime;

        if(reamainingDodgeTime <= 0)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }

    }
    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
       
    }
}
