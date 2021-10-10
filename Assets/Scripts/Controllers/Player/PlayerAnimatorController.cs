using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;

    private float moveSpeed;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if(animator == null)
        {
            print("No animator found for playerAnimator");
        }

        playerController = GetComponent<PlayerController>();
        if(playerController == null)
        {
            print("No PlayerController found for playerAnimator");

        }

        PlayerFighter.OnShoot += StartShootingAnimations;
        PlayerFighter.StopShoot += StopShootingAnimation;
        PlayerFighter.OnReload += ReloadAnimations;

    }

    private void OnDestroy()
    {
        PlayerFighter.OnShoot -= StartShootingAnimations;
        PlayerFighter.StopShoot -= StopShootingAnimation;
        PlayerFighter.OnReload -= ReloadAnimations;


    }
    private void Update()
    {
        moveSpeed = playerController.GetPlayerVelocity();

        animator.SetFloat("moveSpeed", moveSpeed);
    }

    void StartShootingAnimations()
    {
        //animator.SetTrigger("Shoot");
        animator.SetBool("Shoot", true);
    }

    void StopShootingAnimation()
    {
        animator.SetBool("Shoot", false);
    }

    void ReloadAnimations()
    {
        animator.SetTrigger("Reload");
    }
    public void ResetReloadTrigger()
    {
        animator.ResetTrigger("Reload");
    }

    public void TakeOutAnimation()
    {
        animator.SetTrigger("TakeOut");
    }
    public void OverrideAnimator(Animator Anim)
    {
        animator = Anim;
    }

    public void DeathAnimations()
    {

    }
}
