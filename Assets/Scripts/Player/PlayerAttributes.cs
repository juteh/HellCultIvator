using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [SerializeField] private int maxStamina = 100;
    [SerializeField] private float reduceTime = 0.1f;
    [SerializeField] private float recoverTime = 0.1f;
    public float currentStamina;

    private IEnumerator reduceStaminaRoutine;
    private IEnumerator recoverStaminaRoutine;

    private void Awake()
    {
        currentStamina = maxStamina;
    }

    public void StartReduceStamina()
    {
        if (recoverStaminaRoutine != null) {
            Debug.Log("STOP RecoverStamina");
            StopCoroutine(recoverStaminaRoutine);
            recoverStaminaRoutine = null;
        }
        if (reduceStaminaRoutine == null && currentStamina >= 0)
        {
            Debug.Log("START ReduceStamina");
            reduceStaminaRoutine = ReduceStamina();
            StartCoroutine(reduceStaminaRoutine);
        }
    }

    public void StartRecoverStamina()
    {
        if (reduceStaminaRoutine != null)
        {
            Debug.Log("STOP ReduceStamina");
            StopCoroutine(reduceStaminaRoutine);
            reduceStaminaRoutine = null;
        }
        if (recoverStaminaRoutine == null && currentStamina <= maxStamina)
        {
            Debug.Log("START RecoverStamina");
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
            //Debug.Log(staminaRation);
            Singletons.Instance.UIManager.UpdateStaminaBar(staminaRation);
            yield return new WaitForSeconds(reduceTime);
        }
        Debug.Log("FINISH ReduceStamina");
        yield return null;
        StartCoroutine(reduceStaminaRoutine);
        reduceStaminaRoutine = null;
    }

    IEnumerator RecoverStamina()
    {
        for (; currentStamina <= maxStamina; currentStamina++)
        {
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            float staminaRation = currentStamina / maxStamina;

            //Debug.Log(staminaRation);
            Singletons.Instance.UIManager.UpdateStaminaBar(staminaRation);
            yield return new WaitForSeconds(recoverTime);
        }
        Debug.Log("FINISH RecoverStamina");
        yield return null;
        StartCoroutine(recoverStaminaRoutine);
        recoverStaminaRoutine = null;
    }
}
