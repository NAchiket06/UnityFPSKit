using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GermSpike : Enemy
{
    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            print("Spike Collided with player");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            print("Spike Collided with player");
        }
    }

    public void DropPickups()
    {
        EnemyDropPickups();
    }
}
