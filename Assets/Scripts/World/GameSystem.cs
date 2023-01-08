using Cinemachine;
using JetBrains.Annotations;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    [Header("WinCondition")]
    [Tooltip("Maximum of needed Trees to win the Game")]
    [SerializeField] int maxTrees;
    [Space(10)]
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] CinemachineVirtualCamera playerFollowCamera;
    public static GameSystem Instance { get; private set; }
    public int collectedSeeds = 0;
    public int plantedTrees = 0;
    
    private GameObject player;

    void Awake()
    {
        if (Instance != null)
        {
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Singletons.Instance.AudioManager.PlayMusic();
        Singletons.Instance.UIManager.SetSeedPoints(collectedSeeds);
        Singletons.Instance.UIManager.SetTreePoints(plantedTrees);
        PlayerRespawn();
    }

    private void Update()
    {
        // pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // game is already paused
            if (Time.timeScale == 0.0f)
            {
                player.GetComponent<InputController>().cursorLocked = true;
                player.GetComponent<InputController>().cursorInputForLook = true;
                Singletons.Instance.UIManager.ResumeToGame();
            } else
            {
                player.GetComponent<InputController>().cursorLocked = false;
                player.GetComponent<InputController>().cursorInputForLook = false;
                Singletons.Instance.UIManager.PauseGame();
            }
        }
    }

    public void IncrementSeeds()
    {
        Singletons.Instance.AudioManager.PlayCollectSeed();
        collectedSeeds++;
        Singletons.Instance.UIManager.SetSeedPoints(collectedSeeds);
    }

    public bool DecrementSeeds()
    {
        if (collectedSeeds >= 3)
        {
            collectedSeeds -= 3;
            Singletons.Instance.UIManager.SetSeedPoints(collectedSeeds);
            return true;
        }

        return false;
    }
    public void PlantTree()
    {
        Singletons.Instance.AudioManager.PlayPlantTree();
        plantedTrees++;
        Singletons.Instance.UIManager.SetTreePoints(plantedTrees);
        CheckWinCondition();
    }

    public void PlayerDie()
    {
        Singletons.Instance.AudioManager.PlayPlayerDie();
        playerFollowCamera.Follow = null;
        Destroy(player);
    }

    public void PlayerRespawn()
    {
        player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        playerFollowCamera.Follow = player.transform.transform.Find("CameraRoot");
    }

    private void CheckWinCondition()
    {
        if (plantedTrees >= maxTrees)
        {
            SceneController.Instance.LoadSceneByName("WinScreen");
        }
    }
}