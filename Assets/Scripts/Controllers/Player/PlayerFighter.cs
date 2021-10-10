using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerFighter : MonoBehaviour
{
    #region Weapon Data

    [SerializeField] List<Weapon> PlayerWeapons;
    [Header("Weapon Data")]

    [SerializeField] Weapon CurrentWeapon;

    [SerializeField] private int BulletCount = 25;
    public int GetBulletCount()
    {
        return BulletCount;
    }

    [SerializeField] Transform ShootPos;
    [SerializeField] bool CanFire = true;


    #endregion

    #region Gameobject Refrences
    [Header("GameObjects")]

    // bullet to be shot
    [SerializeField] GameObject Bullet;

    [SerializeField] GameObject rayCastStartPoint;

    // after firing, a raycast determines the hit position of bullet, 
    // RaycastHitPosition is intantiated at hit point of raycast and bullet rotates to face this
    [SerializeField] GameObject RayCastHitPositon;

    [SerializeField] private Camera mainCamera;

    // When raycast hits nothing, RayCastHitPoint is instantiated at this position
    [SerializeField] Transform NullHitPoint;

    [SerializeField] private Transform playerHitPoint;
    public Transform getPlayerHitPoint()
    {
        return playerHitPoint;
    }

    #endregion

    #region Shooting varibles

    [Header("Shooting Time")]
    [SerializeField] static float fireRate;
    [SerializeField] float timeSinceLastShoot;

    [SerializeField] ParticleSystem hitParticles;

    #endregion

    #region Events

    [Header("Events")]
    public static Action OnShoot;
    public static Action StopShoot;
    public static Action OnReload;

    #endregion

    #region UI Funtions

    [Header("UI Functions")]
    [SerializeField] TMP_Text currentBulletCount = null;
    [SerializeField] TMP_Text TotalPlayerBulletCount = null;

    #endregion

    private void Start()
    {
        mainCamera = Camera.main;

        GetCurrentWeapon();

        UpdateBulletUI();
    }
    void GetCurrentWeapon()
    {
        CurrentWeapon = GetComponentInChildren<Weapon>();
        TotalPlayerBulletCount.text = BulletCount.ToString();
    }
    public void UpdateBulletUI()
    {
        currentBulletCount.text = CurrentWeapon.getMagazineSize().ToString();
        TotalPlayerBulletCount.text = BulletCount.ToString();
    }
    private void Update()
    {
        CheckWeaponSwitch();

        CheckFire();

        CheckReload();
    }

    void CheckWeaponSwitch()
    {
       if(Input.GetKeyDown(KeyCode.Alpha1))
       {
            SwapWepaon(0);
       }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwapWepaon(1);
        }
    }

    void SwapWepaon(int weaponIndex)
    {
        if (CurrentWeapon == PlayerWeapons[weaponIndex])
        {
            return;
        }
        else
        {

            CurrentWeapon.gameObject.SetActive(false);
            CurrentWeapon = PlayerWeapons[weaponIndex];
            PlayerWeapons[weaponIndex].gameObject.SetActive(true);

            UpdateBulletUI();

        }
    }



    private void CheckFire()
    {

        // if elapsed time is less than the firerate, player cannot shoot
        timeSinceLastShoot += Time.deltaTime;

        if (Input.GetButton("Fire") && CurrentWeapon.CanShoot() && timeSinceLastShoot > 1 / fireRate)
        {

            // if player can fire

            // reset the LastShootTime
            timeSinceLastShoot = 0;

            // Shoot the raycast, which determines the hit position of bullet


            Physics.Raycast(rayCastStartPoint.transform.position, Camera.main.transform.forward, out RaycastHit hit, Mathf.Infinity);

            GameObject RayCastHitPoint;
            //if raycast does not hit anything, instantiate RayCastHitPoint at NullHitPoint
            if (hit.collider == null)
            {
                // instantiate temporary object for the bullet to face to
                // this gives initial direction to the bullet
                RayCastHitPoint = InstantiateRayCastHitPoint(NullHitPoint.position);
            }

            else
            {
                // instantiate temporary object for the bullet to face to
                // this gives initial direction to the bullet
                RayCastHitPoint = InstantiateRayCastHitPoint(hit.point);
               

                if(hit.collider.GetComponent<HealthController>() != null)
                {
                    hit.collider.GetComponent<HealthController>().TakeDamage(CurrentWeapon.GetWeaponDamage(),hit.transform);
                }

                else
                {
                    // instantiat the hit particles at the point of impact
                    InstantiateHitParticles(hit.point);
                }
                
            }

            // invoke The shoot Events
            /*
             * -> PlayerAnimatorController 
             */
            OnShoot?.Invoke();

            //Update the bullet UI
            UpdateBulletUI();


            // Spawn the bullet
            SpawnBullet(RayCastHitPoint);

        }

        // Pla

        if (Input.GetButtonUp("Fire"))
        {
            GetComponentInChildren<Weapon>().StopParticles();
            StopShoot?.Invoke();
        }
    }

    private void CheckReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && CurrentWeapon.CanReload())
        {
            OnReload?.Invoke();
        }
        else
        {
            GetComponent<PlayerAnimatorController>().ResetReloadTrigger();
        }
    }

    private GameObject InstantiateRayCastHitPoint(Vector3 hit)
    {
        GameObject RayCastHitPoint = Instantiate(RayCastHitPositon, hit, Quaternion.identity);
        Destroy(RayCastHitPoint, 0.3f);
        return RayCastHitPoint;
    }

    private void SpawnBullet(GameObject RayCastHitPoint)
    {
        GameObject bullet = Instantiate(Bullet);
        bullet.transform.position = ShootPos.transform.position;
        bullet.GetComponentInChildren<ProjectileController>().SetTarget(RayCastHitPoint.transform);
    }

    // Updates fire Rate whenever player changes weapon 
    public static void SetFireRate(float FireRate)
    {
        fireRate = FireRate;
    }

    // Updates bullet Shoot position whenever player changes weapon 
    public void SetShootPos(Transform shootPos)
    {
        ShootPos = shootPos;
    }

    public void UpdateWeapon(Weapon weapon)
    {
        CurrentWeapon = weapon;
    }

    public void ReduceBullets(int amount)
    {
        BulletCount -= amount;
    }

    public void InstantiateHitParticles(Vector3 position)
    {
        Instantiate(hitParticles, position, Quaternion.identity);
    }

    public void BulletPickupPicked()
    {
        BulletCount += 10;
    }

}
