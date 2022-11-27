using Cinemachine;
using JetBrains.Annotations;
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
    [SerializeField] GameObject _pauseMenu;
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
        PlayerRespawn();
    }

    private void Update()
    {
        // pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void IncrementSeeds()
    {
        collectedSeeds++;
    }

    public void DecrementSeeds()
    {
        if (collectedSeeds > 0)
        {
            collectedSeeds--;
        } else
        {
            Debug.Log("No CollectSeeds are 0. Can't decrement");
        }
    }
    public void PlantTree()
    {
        plantedTrees++;
        CheckWinCondition();
    }

    public void PlayerDie()
    {
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

    public void PauseGame()
    {
        player.GetComponent<InputController>().cursorLocked = false;
        player.GetComponent<InputController>().cursorInputForLook = false;
        Time.timeScale = 0.0f;
        _pauseMenu.SetActive(true);
    }
}