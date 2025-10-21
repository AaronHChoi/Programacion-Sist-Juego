using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Win Condition Settings")]
    [SerializeField] private int enemiesToKill = 10;
    [SerializeField] private string nextLevelName = "Nivel 2";
    [SerializeField] private bool isLastLevel = false; 
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI killCountText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject exitButton; 

    private int enemiesKilled = 0;
    private bool levelCompleted = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateKillCountUI();

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (exitButton != null)
        {
            exitButton.SetActive(false);
        }
    }

    public void OnEnemyKilled()
    {
        if (levelCompleted) return;

        enemiesKilled++;
        Debug.Log($"Enemy killed! Total: {enemiesKilled}/{enemiesToKill}");

        UpdateKillCountUI();

        if (enemiesKilled >= enemiesToKill)
        {
            WinLevel();
        }
    }

    private void UpdateKillCountUI()
    {
        if (killCountText != null)
        {
            killCountText.text = $"Kills: {enemiesKilled}/{enemiesToKill}";

            if (enemiesKilled >= enemiesToKill * 0.8f)
            {
                killCountText.color = Color.green;
            }
        }
    }

    private void WinLevel()
    {
        levelCompleted = true;
        Debug.Log("LEVEL COMPLETED!");

        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.StopSpawning();
        }

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        if (isLastLevel)
        {
            ShowExitButton();
        }
        else
        {
            Invoke(nameof(LoadNextLevel), 2f);
        }
    }

    private void ShowExitButton()
    {
        if (exitButton != null)
        {
            exitButton.SetActive(true);
        }

        Debug.Log("GAME COMPLETED! Press Exit to quit.");
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public int GetEnemiesKilled() => enemiesKilled;
    public int GetEnemiesToKill() => enemiesToKill;
    public bool IsLevelCompleted() => levelCompleted;
}