using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{

    [field: SerializeField] public Animator Animator {get; private set;}
    [field: SerializeField] public CharacterController Controller {get; private set;}
    [field: SerializeField] public ForceReceiver ForceReceiver {get; private set;}
    [field: SerializeField] public NavMeshAgent Agent {get; private set;}
    [field: SerializeField] public WeaponDamage Weapon {get; private set;}
    [field: SerializeField] public Health Health {get; private set;}
    [field: SerializeField] public Target Target {get; private set;}
    [field: SerializeField] public Ragdoll Ragdoll {get; private set;}
    [field: SerializeField] public float PlayerChasingRange {get; private set;}
    [field: SerializeField] public float AttackRange {get; private set;}
    [field: SerializeField] public int AttackDamage {get; private set;}
    [field: SerializeField] public int AttackKnockBack {get; private set;}
    [field: SerializeField] public float MovementSpeed {get; private set;}
    [field: SerializeField] public int index { get; private set; }
    [field: SerializeField] public AudioSource audioSource;
    [field: SerializeField] public float EnemyAttackVolume {get; private set;}
    [field: SerializeField] public float HurtSoundVolume { get; private set; }
    [field: SerializeField] public AudioClip[] EnemyAttackSounds { get; private set; } 
     [field: SerializeField] public AudioClip[] EnemyHurtSounds { get; private set; } 



    public Health Player {get; private set;}
    
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        Agent.updatePosition = false;
        Agent.updateRotation = false;
        SwitchState(new EnemyIdlestate(this));
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDying;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDying;
        
    }

    private void HandleTakeDamage()
    {
        SwitchState(new EnemyImpactState(this));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
    }

     private void HandleDying()
    {
        SwitchState(new EnemyDeadState(this));
    }
    public int RandomizeArrayElementIndex(AudioClip[] audioClipsArray)
    {
        
        return index = Random.Range(0,audioClipsArray.Length);
    }

    public void PlayRandomSound(AudioClip[] audioClipsArray,float volume)
    {
        RandomizeArrayElementIndex(audioClipsArray);
        audioSource.PlayOneShot(audioClipsArray[index], volume);
    }
}
    

