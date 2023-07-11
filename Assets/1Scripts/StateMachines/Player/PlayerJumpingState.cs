using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private readonly int JumpingHash = Animator.StringToHash("Jump2");

    private Vector3 momentum;
    float enterTime;
    float elapsedTime;

    private const float CrossFadeDuration = 0.1f;

    public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine){}
    

    public override void Enter()
    {
        enterTime = Time.time;
      //  Debug.Log("*** Enter TIME: " + enterTime);
        stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);

        momentum = stateMachine.CharacterController.velocity;
        momentum.y = 0f;

        stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
        stateMachine.Animator.CrossFadeInFixedTime(JumpingHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        elapsedTime = Time.time - enterTime;

        if(stateMachine.CharacterController.velocity.y <= 0 || elapsedTime >= 0.45f)
        {
           // float fallTime = Time.time;
           // Debug.Log("FALL TIME: " + fallTime);
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
            return;
        }

        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;

    }

    private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        stateMachine.SwitchState(new PlayerHangingState(stateMachine, ledgeForward, closestPoint));
    }

}
