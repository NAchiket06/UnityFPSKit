using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{

    public bool isPlayer = false;

    [SerializeField] private int MaxHealth;
    public int GetMaxHealth()
    {
        return MaxHealth;
    }
    [SerializeField] private int currentHealth;
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    // particles when entity is damaged
    [SerializeField] ParticleSystem damageParticles;

    // particles when entity dies
    [SerializeField] ParticleSystem deathParticles;

    // Tick damage is used when player takes poison damage from polluted blood cells
    int TickdamageAmount = 0;
    float tickDamageInterval = 1f;
    float timeSinceLastTick = 0;

    public int ticks;

    //Event raised when player takes damage
    public static Action PlayerDamageEvent;

    public static Action PlayerHealthUpdate;


    private void Start()
    {
        CheckIsPlayer();

        currentHealth = MaxHealth;
    }

    private void CheckIsPlayer()
    {
        if (GetComponent<PlayerController>() != null)
        {
            isPlayer = true;
        }

        else
        {
            isPlayer = false;
        }
    }

    private void Update()
    {

        timeSinceLastTick += Time.deltaTime;
        if(ticks>0 && timeSinceLastTick > tickDamageInterval)
        {
            TickDamage();
            timeSinceLastTick = 0;
        }
    }

    public void TakeDamage(int damage,Transform damagePosition)
    {
        //print($"{gameObject.name} took damage of {damage}");

        //if (isPlayer)
        //{
        //    PlayerDamageEvent?.Invoke();
        //}


        if (damageParticles != null && damagePosition != null)
        {
            PlayDamageParticles(damagePosition);
        }
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, MaxHealth);

        if(isPlayer)
        {
            PlayerDamageEvent?.Invoke();
            PlayerHealthUpdate?.Invoke();
        }
        if(currentHealth == 0)
        {
            Died();
        }
        
    }


    public void TickDamage()
    {
        
        TakeDamage(TickdamageAmount, null);
        ticks--;
        print("player took tick damage");
    }

    public void SetTicks(int tickAmount,int damageAmount)
    {
        ticks += tickAmount;
        TickdamageAmount += damageAmount;
    }

    private void Died()
    {

        // Player Died
        if(isPlayer)
            GetComponent<PlayerController>().onPlayerDead();


        //Enemy Died
        else
        {
            if(TryGetComponent(out GermSlime germSlime))
            {
                germSlime.DropPickups();
            }
            if (TryGetComponent(out PollutedBloodCell ps))
            {
                ps.DropPickups();
                ps.RemoveFromSpawnerList();
            }
            if (TryGetComponent(out GermSlime_Ranged gsr))
            {
                gsr.DropPickups();
                gsr.RemoveFromSpawnerList();
            }
            if (TryGetComponent(out GermSpike germSpike))
            {
                germSpike.DropPickups();
                germSlime.RemoveFromSpawnerList();
            }

            PlayDeathParticles();
            print($"{gameObject.name} died.");
            Destroy(gameObject);
        }
    }

    private void PlayDamageParticles(Transform damagePosition)
    {

        if (deathParticles == null) return;
        Instantiate(deathParticles, damagePosition.position, Quaternion.identity);
    }

    private void PlayDeathParticles()
    {
        if (deathParticles == null) return;
        Instantiate(deathParticles, transform.position + ColliderYOffset(), Quaternion.identity); 
    }

    private Vector3 ColliderYOffset()
    {
        Collider box = GetComponent<BoxCollider>();
        if (box == null) return Vector3.zero; ;
        return new Vector3(0,GetComponent<BoxCollider>().size.y / 2,0);
    }

    public void HealthPickup()
    {
        currentHealth += 10;
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
        PlayerHealthUpdate?.Invoke();

    }

    public int ReturnCurrentHealth()
    {
        return currentHealth;
    }
}
