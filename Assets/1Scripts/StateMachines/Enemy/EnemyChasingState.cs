using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingstate : EnemyBaseState
{
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    public EnemyChasingstate(EnemyStateMachine stateMachine) : base(stateMachine){}
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public override void Enter()
    {
        
       stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash,CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        

        if(!IsInChanseRange())
        {
            stateMachine.SwitchState(new EnemyIdlestate(stateMachine));
            return;
        }

        else if(IsInAttackRange())
        {
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
            return;
        }

        MoveTowardsPlayer(deltaTime);
        FacePlayer();
        stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
        // Add screaming animation here
    }

   private void MoveTowardsPlayer(float deltaTIme)
   {
        if(stateMachine.Agent.isOnNavMesh)
        {
             stateMachine.Agent.destination =   stateMachine.Player.transform.position;
                                                         // Tells the enemy where to go

             Move(stateMachine.Agent.desiredVelocity.normalized
                * stateMachine.MovementSpeed, deltaTIme);
        }

        stateMachine.Agent.velocity = stateMachine.Controller.velocity;

   }

   private bool IsInAttackRange()
   {
        if(stateMachine.Player.isDead) { return false; }
        
        float playerDistanceSqr = (stateMachine.Player.transform.position 
        - stateMachine.transform.position).sqrMagnitude;

        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
   }
  
}

