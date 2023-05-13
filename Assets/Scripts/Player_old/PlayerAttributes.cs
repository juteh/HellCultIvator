using System.Collections;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour {
    [SerializeField] private int maxStamina = 100;
    [SerializeField] private float reduceIntervalTime = 0.1f;
    [SerializeField] private float recoverIntervalTime = 0.1f;
    [SerializeField] private float exhaustionTime = 3.0f;

    private IEnumerator reduceStaminaRoutine;
    private IEnumerator recoverStaminaRoutine;
    private float _currentStamina;
    private bool _isExhausted;
    private IEnumerator playerExhaustedRoutine;

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Deathzone") {
            GameSystem.Instance.PlayerDie();
            GameSystem.Instance.PlayerRespawn();
        }
    }

    private void Awake() {
        _currentStamina = maxStamina;
    }

    private void Update() {
        // player is exhausted
        if (_currentStamina <= 0 && playerExhaustedRoutine == null) {
            if (reduceStaminaRoutine != null) {
                StopCoroutine(reduceStaminaRoutine);
                reduceStaminaRoutine = null;
            }
            if (recoverStaminaRoutine != null) {
                StopCoroutine(recoverStaminaRoutine);
                reduceStaminaRoutine = null;
            }
            if (playerExhaustedRoutine == null) {
                playerExhaustedRoutine = PlayerExhausted();
                StartCoroutine(playerExhaustedRoutine);
            }
        }
    }

    public void StartReduceStamina() {
        if (playerExhaustedRoutine != null)
            return;

        if (recoverStaminaRoutine != null) {
            StopCoroutine(recoverStaminaRoutine);
            recoverStaminaRoutine = null;
        }
        if (reduceStaminaRoutine == null && _currentStamina >= 0) {
            reduceStaminaRoutine = ReduceStamina();
            StartCoroutine(reduceStaminaRoutine);
        }
    }

    public void StartRecoverStamina() {
        if (playerExhaustedRoutine != null)
            return;

        if (reduceStaminaRoutine != null) {
            StopCoroutine(reduceStaminaRoutine);
            reduceStaminaRoutine = null;
        }
        if (recoverStaminaRoutine == null && _currentStamina <= maxStamina) {
            recoverStaminaRoutine = RecoverStamina();
            StartCoroutine(recoverStaminaRoutine);
        }

    }

    IEnumerator ReduceStamina() {
        for (; _currentStamina >= 0; _currentStamina--) {
            _currentStamina = Mathf.Clamp(_currentStamina, 0, maxStamina);
            float staminaRation = _currentStamina / maxStamina;
            Singletons.Instance.UIManager.UpdateStaminaBar(staminaRation);
            yield return new WaitForSeconds(reduceIntervalTime);
        }
        reduceStaminaRoutine = null;
    }

    IEnumerator RecoverStamina() {
        for (; _currentStamina <= maxStamina; _currentStamina++) {
            _currentStamina = Mathf.Clamp(_currentStamina, 0, maxStamina);
            float staminaRation = _currentStamina / maxStamina;

            Singletons.Instance.UIManager.UpdateStaminaBar(staminaRation);
            yield return new WaitForSeconds(recoverIntervalTime);
        }
        recoverStaminaRoutine = null;
    }

    IEnumerator PlayerExhausted() {
        _isExhausted = true;
        yield return new WaitForSeconds(exhaustionTime);
        playerExhaustedRoutine = null;
        _currentStamina = maxStamina / 4;
        _isExhausted = false;
    }

    public bool IsExhausted() {
        return _isExhausted;
    }
}
