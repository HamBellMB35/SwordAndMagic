using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("Impact");
    private const float CrossFadeDuration = 0.2f;
    private float duration = .75f;

    public EnemyImpactState(EnemyStateMachine stateMachine) : base(stateMachine){}
    

    public override void Enter()
    {
        //stateMachine.
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;

        if(duration <= 0f)
        {
            stateMachine.SwitchState(new EnemyIdlestate(stateMachine));
        }

    }

    public override void Exit(){}
    
}

