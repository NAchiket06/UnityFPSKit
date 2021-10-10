using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PollutedBloodCell : Enemy
{
    [SerializeField] NavMeshAgent agent;

    private void Start()
    {
        Player = FindObjectOfType<PlayerController>().gameObject;
        playerHealth = Player.GetComponent<HealthController>();
        playerCamera = Player.GetComponentInChildren<Camera>();


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

        if (DistanceToPlayer < AttackRange)
        {

            print("Attack Player");
            // player in range of Germ Slimes attack
            transform.LookAt(Player.transform);
            AttackPlayer();
            return;
        }

        else if (IsPlayerInView(Player, enemyFOV) && DistanceToPlayer < ChaseRange)
        {
            // player is in view of germ Slime, but not in attack range
            ChasePlayer();
        }
    }

    private void AttackPlayer()
    {
        Player.GetComponent<HealthController>().SetTicks(5, Damage);
        GetComponent<HealthController>().TakeDamage(100,null);
    }

    private void ChasePlayer()
    {
        transform.LookAt(Player.transform);
        transform.Translate(Vector3.forward * MovementSpeed * Time.deltaTime);
    }

    public void DropPickups()
    {
        EnemyDropPickups();
    }
}
