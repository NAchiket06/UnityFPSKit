using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    enum PickupType
    {
        Health,Ammo
    }
    [SerializeField] PickupType pickupType;

    void Start()
    {
        float x = Random.Range(30,60);
        float y = Random.Range(30, 60);
        float z = Random.Range(30,60);


        float negative = Random.value;
        if(negative>0.5f)
        {
            x *= -1;
            z *= -1;
        }
        GetComponent<Rigidbody>().AddForce(new Vector3(x,y,z),ForceMode.Impulse);       
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {

            if(pickupType == PickupType.Ammo)
            {
                other.GetComponent<PlayerFighter>().BulletPickupPicked();
                other.GetComponent<PlayerFighter>().UpdateBulletUI();
            }
            else
            {
                other.GetComponent<HealthController>().HealthPickup();
            }

            Destroy(gameObject);
        }

        
    }

}
