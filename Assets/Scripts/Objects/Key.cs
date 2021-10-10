using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] GameObject SpawnerDoor;


    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, 50 * Time.deltaTime); 
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            SpawnerDoor.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
