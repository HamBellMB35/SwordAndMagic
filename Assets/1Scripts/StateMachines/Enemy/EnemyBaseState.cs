using UnityEngine;

public abstract class EnemyBaseState : State
{
      protected EnemyStateMachine stateMachine;
   

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;

    }

     protected void Move(float deltaTIme)
    {
        Move(Vector3.zero, deltaTIme);
    }

    protected void FacePlayer()
    {
         if(stateMachine.Player == null) { return; }

        Vector3 lookPos = stateMachine.Player.transform.position - 
            stateMachine.transform.position;
        lookPos.y = 0f;
        
        stateMachine.transform.rotation= Quaternion.LookRotation(lookPos);
    }


    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected bool IsInChanseRange()
    {
        if(stateMachine.Player.isDead) { return false; }

        float distanceToPlayerSqr = (stateMachine.Player.transform.position - stateMachine.transform.position).sqrMagnitude;

        return distanceToPlayerSqr <= stateMachine.PlayerChasingRange * stateMachine.PlayerChasingRange;
    }


}
