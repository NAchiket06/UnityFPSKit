
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] bool isDead = false;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera mainCamera;

    [Header("Movement")]
    [SerializeField] private float MovementSpeed = 5f;
    [SerializeField] private float SprintMultiplier = 1.5f;
    [SerializeField] private float JumpForce = 3f;
    private float Gravity = -10f;
    private float defaultGravity = -10f;
    [SerializeField] private Vector3 Velocity;
    private Vector3 moveVector;


    [SerializeField] private bool isGrounded = true;

    float h, v;
    private void Start()
    {

        SubscribeEvents();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        characterController = GetComponent<CharacterController>();
        if(characterController==null)
        {
            print("No Character Controller component found on player.");
        }

        mainCamera = GetComponentInChildren<Camera>();
        if (mainCamera == null)
        {
            print("No Camera found on player.");
        }

    }

    private void SubscribeEvents()
    {
        
    }

    private void Update()
    {
        if(isDead)
        {
            return;
        }
        float h = Input.GetAxis("Horizontal") * MovementSpeed;
        float v = Input.GetAxis("Vertical") * MovementSpeed;

        moveVector = transform.forward * v + transform.right * h;

        isGrounded = CheckGrounded();

        if (isGrounded)
        {
            ResetGravityAndVelocity();

            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveVector *= SprintMultiplier;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Velocity.y = JumpForce;
            }

        }

        else
        {
            Velocity.y += Gravity * Time.deltaTime;
            Gravity -= 0.1f;
            moveVector /= 1;
        }


        characterController.Move((moveVector + Velocity) * Time.deltaTime);

    }

    private void ResetGravityAndVelocity()
    {
        Gravity = defaultGravity;
        Velocity.y = 0;
    }

    public bool CheckGrounded()
    {
        return characterController.isGrounded;

    }

    public float GetPlayerVelocity()
    {
        return moveVector.magnitude;
    }


    public void onPlayerDead()
    {
        print(gameObject.name + " died.");
        isDead = true;
    }

   
}
