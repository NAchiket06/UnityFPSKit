using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GermSlime : Enemy
{


    [SerializeField] NavMeshAgent agent;

    [SerializeField] float timeSinceLastAttack;

    private void Start()
    {
        Player = FindObjectOfType<PlayerController>().gameObject;
        playerHealth = Player.GetComponent<HealthController>();


        agent = GetComponent<NavMeshAgent>();

        UpdateNavMeshAgent();
    }


    private void UpdateNavMeshAgent()
    {
        agent.speed = MovementSpeed;
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        float DistanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);

        timeSinceLastAttack += Time.deltaTime;


        if (DistanceToPlayer < AttackRange)
        {
            // player in range of Germ Slimes attack

            transform.LookAt(Player.transform);
            AttackPlayer();
            return;     
        }

        else if (IsPlayerInView(Player, enemyFOV) && DistanceToPlayer < ChaseRange || DistanceToPlayer < SuspicionRange)
        {
            // player is in view of germ Slime, but not in attack range
            ChasePlayer();
        }

    }

    private void AttackPlayer()
    {
        
        if (timeSinceLastAttack > TimeBetweenAttacks)
        {
            print($"{gameObject.name} attacked player");
            playerHealth.TakeDamage(Damage, null);
            timeSinceLastAttack = 0;
        }
    }

    private void ChasePlayer()
    {
        transform.LookAt(Player.transform);
        transform.Translate(Vector3.forward * MovementSpeed * Time.deltaTime);
    }

    public void OnEnemyDeath()
    {
        print(gameObject.name + " died.");

        DropPickups();
        isDead = true;

        Destroy(gameObject);
    }

    public void DropPickups()
    {
        EnemyDropPickups();

    }
}
