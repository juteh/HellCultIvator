using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] TextMeshProUGUI seedsText;
    [SerializeField] TextMeshProUGUI treesText;
    [SerializeField] Image staminaBar;
    [SerializeField] GameObject pauseMenu;

    public void SetSeedPoints(int seeds) {
        seedsText.text = string.Format("Seeds: {0}", seeds);
    }

    public void SetTreePoints(int trees) {
        treesText.text = string.Format("Trees: {0}", trees);
    }

    public void PauseGame() {
        Time.timeScale = 0.0f;
        pauseMenu.SetActive(true);
    }

    public void ResumeToGame() {
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
    }

    public void RestartLevel() {
        Time.timeScale = 1.0f;
        SceneController.Instance.LoadSceneByName("SampleScene");
    }

    public void ExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void UpdateStaminaBar(float value) {
        if (value >= 0.0f && value <= 1.0f) {
            staminaBar.fillAmount = value;
        } else {
            Debug.LogError(string.Format("Wrong value for UpdateStaminaBar() with value: {0}", value));
        }
    }
}
