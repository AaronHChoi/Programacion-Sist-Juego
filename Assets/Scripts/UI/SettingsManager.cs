using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    public float MusicVolume { get; private set; } = 0.6f;
    public float SfxVolume { get; private set; } = 1f;
    public bool Fullscreen { get; private set; } = true;

    private const string KeyMusic = "Settings_MusicVolume";
    private const string KeySfx = "Settings_SfxVolume";
    private const string KeyFullscreen = "Settings_Fullscreen";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Load();
        Apply();
    }

    public void SetMusicVolume(float v)
    {
        MusicVolume = Mathf.Clamp01(v);
        PlayerPrefs.SetFloat(KeyMusic, MusicVolume);
        PlayerPrefs.Save();
        if (SoundManager.Instance != null) SoundManager.Instance.SetMusicVolume(MusicVolume);
    }

    public void SetSfxVolume(float v)
    {
        SfxVolume = Mathf.Clamp01(v);
        PlayerPrefs.SetFloat(KeySfx, SfxVolume);
        PlayerPrefs.Save();
        if (SoundManager.Instance != null) SoundManager.Instance.SetSfxVolume(SfxVolume);
    }

    public void SetFullscreen(bool full)
    {
        Fullscreen = full;
        Screen.fullScreen = Fullscreen;
        PlayerPrefs.SetInt(KeyFullscreen, Fullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Resolution support removed

    public void Apply()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetMusicVolume(MusicVolume);
            SoundManager.Instance.SetSfxVolume(SfxVolume);
        }

        Screen.fullScreen = Fullscreen;
        // Resolution left to system default; do not change screen resolution here
    }

    public void Load()
    {
        MusicVolume = PlayerPrefs.GetFloat(KeyMusic, MusicVolume);
        SfxVolume = PlayerPrefs.GetFloat(KeySfx, SfxVolume);
        Fullscreen = PlayerPrefs.GetInt(KeyFullscreen, Fullscreen ? 1 : 0) == 1;
        // Resolution persistence removed; keep defaults
    }
}
