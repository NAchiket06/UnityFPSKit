using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace FPSKit.UI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] HealthController playerHealthController;
        [SerializeField] TextMeshProUGUI currentHealthTextCompnent;
        void Start()
        {
            HealthController.PlayerHealthUpdate += UpdateHealthUI;
            playerHealthController = FindObjectOfType<PlayerController>().gameObject.GetComponent<HealthController>();

            UpdateHealthUI();
        }

        void UpdateHealthUI()
        {
            currentHealthTextCompnent.text = playerHealthController.GetCurrentHealth().ToString();
        }
    }
}