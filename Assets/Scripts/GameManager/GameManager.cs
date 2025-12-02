using System;
using System.Linq;
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

    // keep original default so we can restore when changing scenes
    private int _defaultEnemiesToKill;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _defaultEnemiesToKill = enemiesToKill;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset per-level state
        enemiesKilled = 0;
        levelCompleted = false;

        // Restore defaults then override for specific levels
        enemiesToKill = _defaultEnemiesToKill;
        isLastLevel = false;

        if (scene.name == "Nivel 2")
        {
            // Level 2 is the last level and requires 15 kills
            enemiesToKill = 15;
            isLastLevel = true;
        }

        // Try to auto-bind UI references in the newly loaded scene
        TryAutoBindUIReferences();

        // Update UI (references may change per-scene; keep null checks)
        UpdateKillCountUI();
        UpdateLivesUI();

        if (winPanel != null) winPanel.SetActive(false);
        if (exitButton != null) exitButton.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        // Play scene-appropriate music
        var sceneName = scene.name;
        if (sceneName.Equals("MainMenu", StringComparison.OrdinalIgnoreCase))
        {
            SoundManager.Instance?.PlayMusicMainMenu();
        }
        else
        {
            // For gameplay levels (Nivel1, Nivel2, etc.) play gameplay music
            SoundManager.Instance?.PlayMusicGameplay();
        }
    }

    void Start()
    {
        // Ensure any inspector-assigned references remain valid, otherwise attempt auto-bind
        TryAutoBindUIReferences();

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

        // Ensure music plays for the current starting scene
        var active = SceneManager.GetActiveScene();
        if (active.IsValid())
        {
            if (active.name.Equals("MainMenu", StringComparison.OrdinalIgnoreCase))
                SoundManager.Instance?.PlayMusicMainMenu();
            else
                SoundManager.Instance?.PlayMusicGameplay();
        }
    }

    void Update()
    {
        // Debug / cheat: press 0 to skip to next level immediately
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("Cheat: Skipping to next level (key 0 pressed)");
            // Force load next level regardless of state
            LoadNextLevel();
        }
    }

    private void TryAutoBindUIReferences()
    {
        // Bind TMP texts
        if (killCountText == null)
        {
            killCountText = FindTMPByNameKeywords("kill", "kills", "killcount", "killscount");
        }

        if (livesText == null)
        {
            livesText = FindTMPByNameKeywords("life", "lives", "health", "vidas");
        }

        // Bind panels/buttons by searching scene GameObjects names
        if (winPanel == null)
        {
            winPanel = FindGameObjectByNameKeywords("win", "victory", "ganaste", "ganador");
        }

        if (losePanel == null)
        {
            losePanel = FindGameObjectByNameKeywords("lose", "lost", "defeat", "derrota");
        }

        if (exitButton == null)
        {
            exitButton = FindGameObjectByNameKeywords("exit", "quit", "salir");
        }
    }

    private TextMeshProUGUI FindTMPByNameKeywords(params string[] keywords)
    {
        var all = FindObjectsOfType<TextMeshProUGUI>(true);
        foreach (var key in keywords)
        {
            var found = all.FirstOrDefault(t => t != null && t.name.IndexOf(key, StringComparison.OrdinalIgnoreCase) >= 0);
            if (found != null) return found;
        }
        // fallback: try to find any TMP that contains 'kills' text
        var heuristic = all.FirstOrDefault(t => t != null && !string.IsNullOrEmpty(t.text) && t.text.ToLower().Contains("kill"));
        if (heuristic != null) return heuristic;
        return all.FirstOrDefault();
    }

    private GameObject FindGameObjectByNameKeywords(params string[] keywords)
    {
        Scene active = SceneManager.GetActiveScene();
        if (!active.IsValid()) return null;

        foreach (GameObject root in active.GetRootGameObjects())
        {
            var found = FindInChildrenByKeywords(root.transform, keywords);
            if (found != null) return found.gameObject;
        }
        return null;
    }

    private Transform FindInChildrenByKeywords(Transform parent, string[] keywords)
    {
        if (parent == null) return null;
        string name = parent.gameObject.name;
        foreach (var key in keywords)
        {
            if (name.IndexOf(key, StringComparison.OrdinalIgnoreCase) >= 0) return parent;
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            var found = FindInChildrenByKeywords(child, keywords);
            if (found != null) return found;
        }
        return null;
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
            else
            {
                // reset color if below threshold
                killCountText.color = Color.white;
            }
        }
    }

    private void WinLevel()
    {
        levelCompleted = true;
        SoundManager.Instance?.PlayVictory();
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