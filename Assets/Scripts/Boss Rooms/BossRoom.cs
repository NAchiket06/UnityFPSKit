using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{

    // is the bossRoom Currently Activated
    [SerializeField] private bool IsActivated;

    // Health of the Boss
    [SerializeField] private float BossHealth;

    //List of the enemy Spawners Present in the room
    [SerializeField] List<SpawnerController> RoomSpawners;

    // Particles effects that start when boss room is activated
    [SerializeField] List<ParticleSystem> RoomParticles;

    // Barries that prevent player from moving outside the room
    [SerializeField] List<GameObject> RoomBarriers;

    // The spawner door linked to the boss room which deactivated when the boss room is completed
    [SerializeField] GameObject LinkedDoor;




    public void ActivateBossRoom()
    {
        IsActivated = true;

        foreach(SpawnerController spawners in RoomSpawners)
        {
            spawners.EnableSpawner();
        }

        EnableParticles();

        ActivateBarriers();
    }

    private void ActivateBarriers()
    {
        foreach(GameObject barrier in RoomBarriers)
        {
            barrier.SetActive(true);
        }
    }

    private void EnableParticles()
    {
        foreach(ParticleSystem particle in RoomParticles)
        {
            particle.Play();
        }
    }


    void DisbleBossRoom()
    {
        DeactivateBarriers();
        DisbleParticles();
    }

    private void DeactivateBarriers()
    {
        foreach (GameObject barrier in RoomBarriers)
        {
            barrier.SetActive(false);
        }
    }

    private void DisbleParticles()
    {
        foreach (ParticleSystem particle in RoomParticles)
        {
            particle.Stop();
        }
    }

    public void BossDefeated()
    {
        LinkedDoor.SetActive(false);
    }
}
