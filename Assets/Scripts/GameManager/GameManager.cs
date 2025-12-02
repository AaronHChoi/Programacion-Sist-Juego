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
    [SerializeField] private GameObject losePanel; // panel derrota
    [SerializeField] private TextMeshProUGUI livesText; // muestra vidas del jugador
    [SerializeField] private int playerLives = 3; // vidas iniciales

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

        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }

        UpdateLivesUI();
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

    public void LoseLevel()
    {
        if (levelCompleted) return;
        levelCompleted = true;
        Debug.Log("LEVEL FAILED!");

        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.StopSpawning();
        }

        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = $"Lives: {playerLives}";
        }
    }

    public int DecreaseLife()
    {
        if (levelCompleted) return playerLives;

        playerLives = Mathf.Max(0, playerLives - 1);
        Debug.Log($"Player died. Remaining lives: {playerLives}");
        UpdateLivesUI();

        if (playerLives <= 0)
        {
            LoseLevel();
        }

        return playerLives;
    }

    public int GetPlayerLives() => playerLives;

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

    public void RestartLevel()
    {
        // Opcional: Time.timeScale = 1f;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    public void LoadMainMenu()
    {
        // Opcional: Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
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