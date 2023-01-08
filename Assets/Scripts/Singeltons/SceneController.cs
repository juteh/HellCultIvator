using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) { return; }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName: sceneName);
    }
}
