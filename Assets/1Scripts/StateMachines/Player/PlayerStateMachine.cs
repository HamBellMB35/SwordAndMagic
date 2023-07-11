using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReceiver InputReceiver {get; private set;}
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public UI_Manager UI_ManagerMana { get; private set; }
    [field: SerializeField] public UI_Manager UI_ManagerStamina { get; private set; }
    [field: SerializeField] public GameObject Shield { get; private set; }
    [field: SerializeField] public GameObject Sowrd { get; private set; }
    [field: SerializeField] public GameObject LeftHandParticles { get; private set; }
    [field: SerializeField] public GameObject RightHandParticles { get; private set; }
    [field: SerializeField] public GameObject Slot1 { get; private set; }
    [field: SerializeField] public GameObject Slot2 { get; private set; }
    [field: SerializeField] public GameObject Slot6 { get; private set; }
    [field: SerializeField] public GameObject Slot7 { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; }
    [field: SerializeField] public LedgeDetector LedgeDetector { get; private set; } 
    [field: SerializeField] public Attack[] Attacks { get; private set; }
    [field: SerializeField] public Attack[] StrikeAttacks { get; private set; } 
    [field: SerializeField] public Attack[] MagicAttacks1 { get; private set; } 
    [field: SerializeField] public Attack[] MagicAttacks2 { get; private set; } 
    public AudioSource audioSource;
    [field: SerializeField] public float PlayerAttackVolume { get; private set; }
    [field: SerializeField] public float EnemyAttackVolume { get; private set; }
    [field: SerializeField] public float BlockAttackVolume { get; private set; }
    [field: SerializeField] public float HurtSoundVolume { get; private set; }
    [field: SerializeField] public AudioClip[] SwordAttackSounds { get; private set; } 
    [field: SerializeField] public AudioClip[] StrikeAttackSounds { get; private set; } 
     [field: SerializeField] public AudioClip[] PlayerHurtSounds { get; private set; } 
   




    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
    [field: SerializeField] public float RotationSmoothingValue { get; private set; }
    [field: SerializeField] public float DodgeDuration { get; private set; }
    [field: SerializeField] public float DodgeDistance { get; private set; }
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public int index { get; private set; }
    public float PreviousDodgeTime { get; private set; } = Mathf.NegativeInfinity;
    public bool MagicMode = false;
     


    public Transform MainCameraTransform { get; private set; }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        MainCameraTransform = Camera.main.transform;

        SwitchState(new PlayerFreelookState(this));
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
        SwitchState(new PlayerImpactState(this));
    }

    private void HandleDying()
    {
        SwitchState(new PlayerDeadState(this));
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


