using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Win Condition Settings")]
    [SerializeField] private int enemiesToKill = 10;
    [SerializeField] private string nextLevelName = "Nivel 2";
    [SerializeField] private bool isLastLevel = false; //Marca si es el �ltimo nivel

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI killCountText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject exitButton; //Bot�n de salir

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

        // Ocultar bot�n de salir al inicio
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

        // Detener spawn de enemigos
        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.StopSpawning();
        }

        // Mostrar panel de victoria
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        // Si es el �ltimo nivel, mostrar bot�n de salir
        if (isLastLevel)
        {
            ShowExitButton();
        }
        else
        {
            // Si no es el �ltimo nivel, cargar el siguiente
            Invoke(nameof(LoadNextLevel), 2f);
        }
    }

    // Mostrar bot�n de salir
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

    // M�todo p�blico para cerrar el juego (llamar desde el bot�n)
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