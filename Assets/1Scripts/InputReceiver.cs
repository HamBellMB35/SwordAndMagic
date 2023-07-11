using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReceiver : MonoBehaviour, Controls.IPlayerActions
{
    public bool IsAttacking { get; private set; }
    public bool IsAttacking2 { get; private set; }
    public bool IsBlocking { get; private set; }
    public Vector2 MovementValue { get; private set; }

    public event Action DodgeEvent;
    public event Action JumpEvent;
    public event Action TargetEvent;
    public PlayerStateMachine PlayerStateMachine;
    public AudioClip SheetingSwordSound;
    public AudioClip UnsheetingSwordSound;
    public GameObject MeleeModeIndicator;
    public GameObject MagicModeIndicator;

    private Controls controls;
     private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);

        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if(!context.performed) {return;}                // Do nothing if spacebar has not been pressed
        JumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if(!context.performed) {return;}                // Do nothing if Shift has not been pressed
        DodgeEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
       
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        TargetEvent?.Invoke();
    }

    

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsAttacking = true;
        }
        else if(context.canceled)
        {
            IsAttacking= false;
        }
    }

        public void OnAttack2(InputAction.CallbackContext context)
    {
        Debug.Log("** USING SECONDARY ATTACK***");
        if (context.performed)
        {
            IsAttacking2 = true;
        }
        else if(context.canceled)
        {
            IsAttacking2= false;
        }
    }

    public void OnBlock(InputAction.CallbackContext context)
    {
         if (context.performed)
        {
            IsBlocking = true;
        }
        else if(context.canceled)
        {
            IsBlocking= false;
        }
    }

    public void OnModeSwitch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {// Add ding/swing sound here
            if(PlayerStateMachine.MagicMode == false)
            {
                PlayerStateMachine.audioSource.PlayOneShot(SheetingSwordSound, PlayerStateMachine.BlockAttackVolume);
            }

            else
            {
                PlayerStateMachine.audioSource.PlayOneShot(UnsheetingSwordSound, PlayerStateMachine.BlockAttackVolume);
            }
            PlayerStateMachine.audioSource.PlayOneShot(SheetingSwordSound, PlayerStateMachine.BlockAttackVolume);
            PlayerStateMachine.Shield.SetActive(PlayerStateMachine.MagicMode);
            PlayerStateMachine.Sowrd.SetActive(PlayerStateMachine.MagicMode);
            MeleeModeIndicator.SetActive(PlayerStateMachine.MagicMode);
            PlayerStateMachine.MagicMode =  !PlayerStateMachine.MagicMode;
            PlayerStateMachine.RightHandParticles.SetActive(PlayerStateMachine.MagicMode);
            PlayerStateMachine.LeftHandParticles.SetActive(PlayerStateMachine.MagicMode);
         
            MagicModeIndicator.SetActive( PlayerStateMachine.MagicMode);
            
        }
        
    }
}
