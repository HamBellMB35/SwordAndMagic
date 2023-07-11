using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] public PlayerStateMachine playerStateMachine;    
    [SerializeField] public EnemyStateMachine enemyStateMachine;    
    [SerializeField] public AudioClip DeathSound;    
    
   
   
    private int health;
  
    public bool isDead => health == 0;

    private bool isInvulnerable;

    public event Action OnTakeDamage;
    public event Action OnDie;

    
    private void Start()
    {
        health = maxHealth;

    }

    public void SetInvulnerable(bool isInvulnerable)
    {
       
        this.isInvulnerable = isInvulnerable;
    }

    public void DealSwordDamage(int swordDamage)
    {
        if(health == 0) { return; }

        if(isInvulnerable)
        {
            return; 
        }
        
       // health -= swordDamage;

       if(playerStateMachine != null && gameObject.tag == "Player")
       {
            playerStateMachine.PlayRandomSound(playerStateMachine.PlayerHurtSounds, playerStateMachine.HurtSoundVolume);
       }
       else if(enemyStateMachine != null && gameObject.tag == "Enemy")
       {
           enemyStateMachine.PlayRandomSound(enemyStateMachine.EnemyHurtSounds, enemyStateMachine.HurtSoundVolume);
       }

        health = Mathf.Max(health - swordDamage, 0);

        OnTakeDamage?.Invoke();
        
        if(health == 0)
        {
            
            playerStateMachine.audioSource.PlayOneShot(DeathSound, playerStateMachine.HurtSoundVolume);

            OnDie?.Invoke();
        }

        Debug.Log("THE ENEMY HEALTH IS " + health); 
    }


}
