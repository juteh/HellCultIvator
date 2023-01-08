using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI seedsText;
    [SerializeField] TextMeshProUGUI treesText;
    [SerializeField] Image staminaBar;
    [SerializeField] GameObject pauseMenu;

    public void SetSeedPoints(int seeds)
    {
        seedsText.text = string.Format("Seeds: {0}", seeds);
    }

    public void SetTreePoints(int trees)
    {
        treesText.text = string.Format("Trees: {0}", trees);
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        pauseMenu.SetActive(true);
    }

    public void ResumeToGame()
    {
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<InputController>().cursorLocked = true;
        player.GetComponent<InputController>().cursorInputForLook = true;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1.0f;
        SceneController.Instance.LoadSceneByName("SampleScene");
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
