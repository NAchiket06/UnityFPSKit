using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{

    [SerializeField] private float MouseSensitivity = 100;
    private float xRotation;

    [SerializeField] Transform Player;

    void Start()
    {
        transform.SetPositionAndRotation(transform.position, Quaternion.identity);
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        xRotation -= mouseY * MouseSensitivity * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation, -130f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        Player.Rotate(Vector3.up * mouseX * MouseSensitivity * Time.deltaTime);
    }


}
