using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Weapon properties
public class Weapon : MonoBehaviour
{
    [SerializeField] ParticleSystem MuzzleParticles = null;
    [SerializeField] AnimatorOverrideController weponOverrideController;
    [SerializeField] Transform ShootPosition;
    [SerializeField] private float FireRate = -1f;

    [SerializeField] private int MagazineCapacity = -1;
    [SerializeField] private int CurrentMagazine = -1;

    
    public int getMagazineSize()
    {
        return CurrentMagazine;
    }

    [SerializeField] private int WeaponDamage = -1;
    public int GetWeaponDamage()
    {
        return WeaponDamage;
    }

    [SerializeField] GameObject Player;
    [SerializeField] PlayerFighter playerFighter;
    [SerializeField] PlayerAnimatorController playerAnimatorController;
    public float GetFireRate()
    {
        return FireRate;
    }


    [SerializeField] private bool isWeaponReloading = false;

    private void Start()
    {
        
        // Get the player refrence
        // Updates refrence in the PlayerAnimatorControllerScript of animator
        Player = transform.parent.parent.gameObject;


        // get refrence of playerFighter
        playerFighter = Player.GetComponent<PlayerFighter>();

        // get refrence to playerAnimator
        playerAnimatorController = Player.GetComponent<PlayerAnimatorController>();

        

        // Set the player fireRate According to the weapon fire Rate
        PlayerFighter.SetFireRate(FireRate);


        //override the player animator with weapons animator
        GetComponent<Animator>().runtimeAnimatorController = weponOverrideController;

        //update shoot Position
        if (ShootPosition != null)
        {
            playerFighter.SetShootPos(ShootPosition);
        }

        //set current Weapon Magazine to Magazine Capacity
        CurrentMagazine = MagazineCapacity;
    }
    
    private void OnEnable()
    {

        // Play the TakeOut Aniamtion
        playerAnimatorController.TakeOutAnimation();

        // Whenever weapon is switched, the gameobject is set active, 
        // and the current active gameobject is the player weapon


        // gets the player refrence
        Player = transform.parent.parent.gameObject;

        // get refrence of playerFighter
        playerFighter = Player.GetComponent<PlayerFighter>();

        // get refrence to playerAnimator
        playerAnimatorController = Player.GetComponent<PlayerAnimatorController>();


        //updates PlayerFighter Weapon Refrence
        playerFighter.UpdateWeapon(this);

            // player Shoot Event
        PlayerFighter.OnShoot += OnShootEvent;
        print($"{gameObject} subscribed to ShootEvent");


        // player Reload Event
        //PlayerFighter.OnReload += Reload;

        PlayerFighter.SetFireRate(FireRate);

        //override the player animator
        print($"Swapped animator to {gameObject.name}");
        GetComponent<Animator>().runtimeAnimatorController = weponOverrideController;

        //update shoot Posiion
        if (ShootPosition != null)
        {
            playerFighter.SetShootPos(ShootPosition);
        }

        //override the player animator with weapons animator
        playerAnimatorController.OverrideAnimator(GetComponent<Animator>());

    }

    private void OnDisable()
    {
        PlayerFighter.OnShoot -= OnShootEvent;
    }

    private void OnShootEvent()
    {
        // is there are no bullets in magazine, player cannot Shoot
        if(CurrentMagazine<=0)
        {
            print("No Bullets in magazine");

            // Try Reload
            ////////////////////////////////////////////////////////////
            return;
        }

        // player Shoots and decrease bullet count by 1
        CurrentMagazine-=1;

        // play Muzzle Particles
        if (MuzzleParticles != null)
        {
            MuzzleParticles.Play();
            //Invoke("StopParticles", 0.2f);
        }
    }

    public void StopParticles()
    {
        //stop muzzle particles after certain time
        MuzzleParticles.Stop();
    }

    public bool CanShoot()
    {
        // is the player currently reloading weapon?
        if(isWeaponReloading)
        {
            return false;
        }

        //are there bullets in magazine?
        // this is checked twice
        if(CurrentMagazine<=0)
        {
            return false;
        }

        // player can shoot
        return true;
    }

    public bool CanReload()
    {

        // if the current magazine is full, player cannot reload
        if(CurrentMagazine== MagazineCapacity)
        {
            return false;
        }

        return true;
    }

    public void Reload()
    {
        // get current bullet count of player
        int playerBullets = playerFighter.GetBulletCount();

        // if player does not have any bullets
        // cannot reload
        if(playerBullets ==0 )
        {
            return;
        }


        //isWeaponReloading = true;
        // if player bullet count is grater than magazine  capacity ,
        // reload and reduce bullet count
        if (playerBullets >= MagazineCapacity)
        {

            // get the amount of bullets to be added in weapon
            int BulletsToAdd = MagazineCapacity - CurrentMagazine;

            // not enough bullets to fill the magazine
            if (playerBullets < BulletsToAdd)
            {
                print("Not engough bullets to fill magazine");
                // add remaining bullets
                CurrentMagazine += playerBullets;
                playerFighter.ReduceBullets(playerBullets);
            }

            else
            {
                // player has enough bullets to fully reload
                CurrentMagazine = MagazineCapacity;
                CurrentMagazine = Mathf.Clamp(CurrentMagazine, 0, MagazineCapacity);
                print("Fully Reloaded");
                playerFighter.ReduceBullets(BulletsToAdd);
            }

            playerFighter.UpdateBulletUI();
            return; 
        }
        
    }

    public void isReloading()
    {
        isWeaponReloading = true;
    }
    public void ReloadAnimationOver()
    {
        isWeaponReloading = false;
    }
}
