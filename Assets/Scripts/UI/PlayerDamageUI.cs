using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FPSKit.UI
{
    public class PlayerDamageUI : MonoBehaviour
    {
        [SerializeField] CanvasGroup DamageCanvas;


        void Start()
        {
            HealthController.PlayerDamageEvent += OnPlayerDamaged;
        }

        public void OnPlayerDamaged()
        {
            DamageCanvas.alpha = 1;
            StartCoroutine(FadeOutDamageEffect(2f));
        }

        public IEnumerator FadeOutDamageEffect(float time)
        {
            while (DamageCanvas.alpha != 0)
            {
                DamageCanvas.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}
