using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    //default Speed of projectile
    [SerializeField] float projectileSpeed = 30f;

    [SerializeField] int damage;

    // rayCast hit position where the bullet needs to hit 
    // sets the inital rotation of the bullet
    // This is done to ensure that bullet always hits at the crosshair center 
    [SerializeField] Transform target;


    [SerializeField] ParticleSystem HitParticles;


    private void Start()
    {
        Destroy(gameObject, 5f);
    }
    void Update()
    {
        
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
       
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "Player")
        {
            other.GetComponent<HealthController>().TakeDamage(damage,null);
        }

        //print($"Collided with {other.gameObject.name}");

        //Instantiate(HitParticles, transform.position, Quaternion.identity);


        //play hit particles


        //destory projectile
        Destroy(gameObject);

    }

    public void InstantiateHitParticles(Vector3 position)
    {
        Instantiate(HitParticles, position, Quaternion.identity);
    }

    public void SetTarget(Transform Target)
    {

        // after fired, set the target to the hit point of raycast
        target = Target;

        // look at the
        transform.LookAt(target.transform);
    }

    public void SetDamage(int Damage)
    {
        damage = Damage;
    }
}
