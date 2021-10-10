using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StomachRoomTrigger : MonoBehaviour
{

    int enemiesSpawned = 0;
    [SerializeField] List<GameObject> RoomDoors;
    [SerializeField] List<ParticleSystem> Particles;

    [SerializeField] List<GameObject> Enemies;
    [SerializeField] List<Transform> SpawnPoints;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartStomachRoomFight();
        }
    }

    private void StartStomachRoomFight()
    {
        ToggleDoors(true);
        PlayParticles();
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        enemiesSpawned++;
        int x = UnityEngine.Random.Range(0, 2);
        int y = UnityEngine.Random.Range(0, SpawnPoints.Count - 1);
        GameObject enemy = Instantiate(Enemies[x], SpawnPoints[y].position, Quaternion.identity, gameObject.transform.parent.parent);

        if (enemiesSpawned == 10)
        {
            StopCoroutine(SpawnEnemies());
            ToggleDoors(false);
            StopParticles();
            gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(1.5f);
    }

    private void PlayParticles()
    {
        foreach(ParticleSystem particle in Particles)
        {
            particle.Play();
        }
    }

    private void StopParticles()
    {
        foreach (ParticleSystem particle in Particles)
        {
            particle.Stop();
        }
    }

    private void ToggleDoors(bool bol)
    {
       foreach(GameObject door in RoomDoors)
        {
            door.SetActive(bol);
        }
    }
}
