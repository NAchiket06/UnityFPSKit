using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] protected bool isDead = false;

    [SerializeField] protected float MovementSpeed = -1;

    [SerializeField] protected Transform defaultIdleLocation;

    #region Attack Parameters

    [Header("Attack Parameters")]
    [SerializeField] protected int Damage=-1;

    [SerializeField] protected float TimeBetweenAttacks=-1;

    [SerializeField] protected float AttackRange = -1; // distance from the player the enemy can attack

    [SerializeField] protected float SuspicionRange = -1; // if player is in suspicion range, enemy chases player even if player was not in FOV

    [SerializeField] protected float ChaseRange = -1; // max distance from where the enemy starts chasing the player, if player is in FOV of enemy


    /// <summary>
    // AttackRange < SuspicionRange < ChaseRange
    /// </summary>

    #endregion

    #region Pickups
    [Header("Death Pickups")]

    [SerializeField] protected GameObject HealthPickup;
    [SerializeField] protected GameObject AmmoPickup;

    #endregion

    #region Player Refrences
    [Header("Player Refrences")]

    public GameObject Player;
    protected Camera playerCamera;
    protected HealthController playerHealth;

    #endregion


    [SerializeField] protected float enemyFOV = 50f;

    [SerializeField] protected SpawnerController spawnerController;

    //[SerializeField] protected Animator animator;

    public bool IsPlayerInView(GameObject player,float FOV)
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 20f)
        {
            // player is nearby enemy

            // if enemy is facing the same direction as player, only notice if player is moving

            Vector3 playerDir = player.transform.position - transform.position;

            float angle = Vector3.Angle(transform.forward, playerDir);

            if (angle >= -FOV && angle <= FOV)
            {
                print("Player in view of " + gameObject.name);
                return true;
            }

        }

        return false;
    }

    public void EnemyDropPickups()
    {
        float ammoRandom = UnityEngine.Random.value; 

        if (ammoRandom > 0.5f)
        {
            print($"{gameObject.name} dropped ammo");
            GameObject ammo = Instantiate(AmmoPickup,null,true);
            ammo.transform.position = transform.position;
        }

        float healthRandom = Random.value;
        print(healthRandom);
        if (healthRandom < 0.3f)
        {
            print($"{gameObject.name} dropped health");
            GameObject health = Instantiate(HealthPickup,transform,true);
            health.transform.position = transform.position;
        }
    }

    public void SetDefaultSpawnPoint(Transform Position)
    {
        defaultIdleLocation = Position;
    }

    public void SetSpawner(SpawnerController spawner)
    {
        spawnerController = spawner;
    }

    public void RemoveFromSpawnerList()
    {

        if(spawnerController== null)
        {
            return;
        }
        spawnerController.RemoveFromSpawnedList(gameObject);
    }
}
