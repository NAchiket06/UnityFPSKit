using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] bool CanSpawn;

    [SerializeField] List<GameObject> ListOfEnemiesSpawned;

    [SerializeField] GameObject[] Enemies;

    [SerializeField] Transform SpawnPoint;

    [SerializeField] Transform[] ListOfEnemyIdlePoints;

    [SerializeField] GameObject EnemiesParent;

    float timeBetweenSpawns = 4f;
    float timeSinceLastSpawned;

    [SerializeField] int MaxEnemyLimit = 5;

    
    void Start()
    {
        EnemiesParent = GameObject.FindGameObjectWithTag("Enemies");
    }

    
    void Update()
    {
        if(!CanSpawn || ListOfEnemiesSpawned.Count >= MaxEnemyLimit)
        {
            return;
        }

        checkForNullInList();
        timeSinceLastSpawned += Time.deltaTime;

        if (timeSinceLastSpawned > timeBetweenSpawns)
        {
            timeSinceLastSpawned = 0;
            int EnemyRandom = UnityEngine.Random.Range(0, Enemies.Length);
            int IdlePointRandom = UnityEngine.Random.Range(0, ListOfEnemyIdlePoints.Length-1);
            SpawnEnemy(EnemyRandom, IdlePointRandom);

        }
    }

    private void checkForNullInList()
    {
        foreach(GameObject nulls in ListOfEnemiesSpawned)
        {
            if (nulls == null)
                ListOfEnemiesSpawned.Remove(nulls);
        }
    }

    private void SpawnEnemy(int EnemyRandom, int IdlePointRandom)
    {
        //GameObject SpawnedEnemy = Instantiate(Enemies[EnemyRandom], SpawnPoint.position, Quaternion.identity,EnemiesParent.transform);
        GameObject SpawnedEnemy = Instantiate(Enemies[EnemyRandom], ListOfEnemyIdlePoints[IdlePointRandom].position, ListOfEnemyIdlePoints[IdlePointRandom].rotation, EnemiesParent.transform); 


        if (SpawnedEnemy.TryGetComponent(out GermSlime germSlime))
        {
            //germSlime.SetDefaultSpawnPoint(ListOfEnemyIdlePoints[IdlePointRandom]);
            germSlime.SetSpawner(this);
        }
        if (SpawnedEnemy.TryGetComponent(out PollutedBloodCell ps))
        {
            //ps.SetDefaultSpawnPoint(ListOfEnemyIdlePoints[IdlePointRandom]);
            ps.SetSpawner(this);
        }
        if (SpawnedEnemy.TryGetComponent(out GermSlime_Ranged gsr))
        {
            //gsr.SetDefaultSpawnPoint(ListOfEnemyIdlePoints[IdlePointRandom]);
            gsr.SetSpawner(this);
        }

        
        ListOfEnemiesSpawned.Add(SpawnedEnemy);
    }

    public void RemoveFromSpawnedList(GameObject enemy)
    {
        ListOfEnemiesSpawned.Remove(enemy);
    }

    public void EnableSpawner()
    {
        CanSpawn = true;
    }

    public void DisableSpawner()
    {
        CanSpawn = false;
    }
}
