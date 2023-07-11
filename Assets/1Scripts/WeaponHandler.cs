using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] public GameObject weaponLogic;
    [SerializeField] public GameObject magicWeaponLogic;
    [SerializeField] public GameObject FootWeaponLogic;
    [SerializeField] public GameObject MagicAttack2Prefab;
    [SerializeField] public GameObject MagicAttackPrefab;
    [SerializeField] public GameObject EnemyTargetPoint;
    [SerializeField] public Transform magicSpawnPoint;
    [SerializeField] public Transform magicSpawnPoint2;
    [SerializeField] public float launchForce ;
    private Vector3 direction;
    [SerializeField] public PlayerStateMachine playerStateMachine;      // Move this code to MagicattackState
   // [SerializeField] public float destroyDelay = 0.2f;

    private GameObject instantiatedMagicAttack; // Store the reference to the instantiated magic attack
    
    private void Start()
    {
        playerStateMachine = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
        EnemyTargetPoint = GameObject.FindGameObjectWithTag("EnemyTargetPoint");
    }
    public void EnableWeapon()
    {
        weaponLogic.SetActive(true);
    }

    public void EnableMagicWeapon()
    {
        magicWeaponLogic.SetActive(true);
    }

    public void DisAbleWeapon()
    {
        weaponLogic.SetActive(false);
    }

    public void DisAbleMagicWeapon()
    {
        magicWeaponLogic.SetActive(false);
    }

    public void MagicAttack2()                                           // Move this code to MagicattackState
    {
        #region oldMagicattack 
        // GameObject magicAttack = Instantiate(MagicAttack2Prefab, magicSpawnPoint.position, magicSpawnPoint.rotation);
        // Rigidbody ballRigidbody = magicAttack.GetComponent<Rigidbody>();

        //  if(EnemyTargetPoint!= null)
        // {
        //     direction = - EnemyTargetPoint.transform.position;
        //     Debug.Log("Direction is " + direction.ToString());

        //     Debug.Log("CURRENT TARGET FOUND");
        // }

        // // Calculate the direction towards the center of the screen ** use raycast //
        // else
        // {
        //     Debug.Log("CURRENT TARGET NOT FOUND");

        //      direction = magicSpawnPoint.transform.forward;

        // }
        // // Apply the launch force in the calculated direction
        // ballRigidbody.AddForce(direction * launchForce , ForceMode.Force);
        #endregion

         if (instantiatedMagicAttack != null)
        {
            // Magic attack already instantiated, follow the enemy target point
            if (EnemyTargetPoint != null)
            {
                direction = EnemyTargetPoint.transform.position - instantiatedMagicAttack.transform.position;
                instantiatedMagicAttack.GetComponent<Rigidbody>().AddForce(direction * launchForce, ForceMode.Force);
            }
            else
            {
                // Enemy target point not found, stop following
                Destroy(instantiatedMagicAttack);
            }
        }
        else
        {
            // Instantiate the magic attack prefab and start following the enemy target point
            if (EnemyTargetPoint != null)
            {
                instantiatedMagicAttack = Instantiate(MagicAttack2Prefab, magicSpawnPoint2.position, magicSpawnPoint.rotation);
                direction = EnemyTargetPoint.transform.position - instantiatedMagicAttack.transform.position;
                instantiatedMagicAttack.GetComponent<Rigidbody>().AddForce(direction * launchForce, ForceMode.Force);
            }
        }
    }

    public void MagicAttack1()
    {
        if (playerStateMachine.Targeter.CurrentTarget!= null)
        {
            //Instantiate(MagicAttackPrefab, magicWeaponLogic.transform.position, Quaternion.Euler(new Vector3(90,180,0)));
            Instantiate(MagicAttackPrefab, magicWeaponLogic.transform.position,magicSpawnPoint.rotation);
        }
    }
}
