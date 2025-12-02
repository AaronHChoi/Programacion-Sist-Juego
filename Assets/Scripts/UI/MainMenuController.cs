using System.Collections;
using UnityEngine;

// Simple controller to open/close panels from main menu buttons
public class MainMenuController : MonoBehaviour
{
    public GameObject settingsPanel;

    void Start()
    {
        // Attempt immediate play; if SoundManager isn't ready yet, wait a bit.
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayMusicMainMenu();
        }
        else
        {
            StartCoroutine(WaitAndPlayMainMenuMusic());
        }
    }

    private IEnumerator WaitAndPlayMainMenuMusic()
    {
        // Wait up to 2 seconds for SoundManager to initialize.
        float timeout = 2f;
        float t = 0f;
        while (SoundManager.Instance == null && t < timeout)
        {
            t += Time.deltaTime;
            yield return null;
        }

        SoundManager.Instance?.PlayMusicMainMenu();
    }

    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }
}
