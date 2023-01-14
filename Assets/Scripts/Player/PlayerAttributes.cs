using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [SerializeField] private int maxStamina = 100;
    [SerializeField] private float reduceIntervalTime = 0.1f;
    [SerializeField] private float recoverIntervalTime = 0.1f;
    [SerializeField] private float exhaustionTime = 3.0f;
    public float currentStamina;

    private IEnumerator reduceStaminaRoutine;
    private IEnumerator recoverStaminaRoutine;
    private IEnumerator playerExhaustedRoutine;

    private void Awake()
    {
        currentStamina = maxStamina;
    }

    private void Update()
    {
        // player is exhausted
        if (currentStamina <= 0 && playerExhaustedRoutine == null)
        {
            if (reduceStaminaRoutine != null)
            {
                StopCoroutine(reduceStaminaRoutine);
                reduceStaminaRoutine = null;
            }
            if (recoverStaminaRoutine != null)
            {
                StopCoroutine(recoverStaminaRoutine);
                reduceStaminaRoutine = null;
            } if (playerExhaustedRoutine == null)
            {
                playerExhaustedRoutine = PlayerExhausted();
                StartCoroutine(playerExhaustedRoutine);
            }
        }
    }

    public void StartReduceStamina()
    {
        if (playerExhaustedRoutine != null) return;

        if (recoverStaminaRoutine != null) {
            StopCoroutine(recoverStaminaRoutine);
            recoverStaminaRoutine = null;
        }
        if (reduceStaminaRoutine == null && currentStamina >= 0)
        {
            reduceStaminaRoutine = ReduceStamina();
            StartCoroutine(reduceStaminaRoutine);
        }
    }

    public void StartRecoverStamina()
    {
        if (playerExhaustedRoutine != null) return;

        if (reduceStaminaRoutine != null)
        {
            StopCoroutine(reduceStaminaRoutine);
            reduceStaminaRoutine = null;
        }
        if (recoverStaminaRoutine == null && currentStamina <= maxStamina)
        {
            recoverStaminaRoutine = RecoverStamina();
            StartCoroutine(recoverStaminaRoutine);
        }

    }

    IEnumerator ReduceStamina()
    {
        for (; currentStamina >= 0; currentStamina--)
        {
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            float staminaRation = currentStamina / maxStamina;
            Singletons.Instance.UIManager.UpdateStaminaBar(staminaRation);
            yield return new WaitForSeconds(reduceIntervalTime);
        }
        reduceStaminaRoutine = null;
    }

    IEnumerator RecoverStamina()
    {
        for (; currentStamina <= maxStamina; currentStamina++)
        {
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            float staminaRation = currentStamina / maxStamina;

            Singletons.Instance.UIManager.UpdateStaminaBar(staminaRation);
            yield return new WaitForSeconds(recoverIntervalTime);
        }
        recoverStaminaRoutine = null;
    }

    IEnumerator PlayerExhausted()
    {
        yield return new WaitForSeconds(exhaustionTime);
        playerExhaustedRoutine = null;
        currentStamina = maxStamina / 4;
    }
}
