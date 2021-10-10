using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GermSlime_Ranged : Enemy
{
    [SerializeField] NavMeshAgent agent;

    [SerializeField] float timeSinceLastAttack;

    [SerializeField] GameObject enemyProjectile;
    [SerializeField] Transform ShootPosition;

    [SerializeField] Transform playerHitPoint = null;

    private void Start()
    {
        Player = FindObjectOfType<PlayerController>().gameObject;
        playerHealth = Player.GetComponent<HealthController>();
        playerHitPoint = Player.GetComponent<PlayerFighter>().getPlayerHitPoint();

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

        else if (IsPlayerInView(Player, enemyFOV) && DistanceToPlayer < ChaseRange)
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
            GameObject projectile = Instantiate(enemyProjectile, ShootPosition.transform.position, Quaternion.identity);
            projectile.GetComponent<ProjectileController>().SetTarget(playerHitPoint);
            projectile.GetComponent<ProjectileController>().SetDamage(Damage);
            timeSinceLastAttack = 0;
        }
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
