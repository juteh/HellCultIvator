using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [SerializeField] GameObject _pauseMenu;

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        _pauseMenu.SetActive(true);
    }

    public void ResumeToGame() {
        Time.timeScale = 1.0f;
        _pauseMenu.SetActive(false);
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
