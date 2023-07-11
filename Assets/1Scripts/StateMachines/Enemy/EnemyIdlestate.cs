using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdlestate : EnemyBaseState
{
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    public EnemyIdlestate(EnemyStateMachine stateMachine) : base(stateMachine){}
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public override void Enter()
    {
        
       stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash,CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        if(IsInChanseRange())
        {
            stateMachine.SwitchState(new EnemyChasingstate(stateMachine));
            return;
        }

        FacePlayer();
        stateMachine.Animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        // Add screaming animation here
    }
  
}
