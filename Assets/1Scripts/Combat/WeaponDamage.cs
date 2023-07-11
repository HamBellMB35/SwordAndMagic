using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider myCoolider;

    private int damage;
    private float knockback;
    
    private void Start()
    {
        myCoolider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
    }

    private List<Collider> alreadyColllidedWith = new List<Collider>();

    private void OnEnable()
    {
        alreadyColllidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyColllidedWith.Contains(other)) { return; }
        
        alreadyColllidedWith.Add(other);
        
        if(other == myCoolider) { return; }

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealSwordDamage(damage);

        }

        if(other.TryGetComponent<ForceReceiver>(out ForceReceiver forcereceiver))
        {
            Vector3 direction = (other.transform.position - myCoolider.transform.position).normalized ;
            forcereceiver.AddForce(direction * knockback);
        }


    }

    public void SetAttack(int damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
    }
}
