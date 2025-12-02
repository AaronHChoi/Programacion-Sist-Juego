using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    public float MusicVolume { get; private set; } = 0.6f;
    public float SfxVolume { get; private set; } = 1f;
    public bool Fullscreen { get; private set; } = true;
    public int ResolutionIndex { get; private set; } = 0;

    private Resolution[] resolutions;

    private const string KeyMusic = "Settings_MusicVolume";
    private const string KeySfx = "Settings_SfxVolume";
    private const string KeyFullscreen = "Settings_Fullscreen";
    private const string KeyResolution = "Settings_ResolutionIndex";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        resolutions = Screen.resolutions;
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

    public void SetResolution(int index)
    {
        if (resolutions == null || resolutions.Length == 0) return;
        ResolutionIndex = Mathf.Clamp(index, 0, resolutions.Length - 1);
        Resolution r = resolutions[ResolutionIndex];
        Screen.SetResolution(r.width, r.height, Fullscreen, r.refreshRate);
        PlayerPrefs.SetInt(KeyResolution, ResolutionIndex);
        PlayerPrefs.Save();
    }

    public Resolution[] GetResolutions() => resolutions;

    public void Apply()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetMusicVolume(MusicVolume);
            SoundManager.Instance.SetSfxVolume(SfxVolume);
        }

        Screen.fullScreen = Fullscreen;
        if (resolutions != null && resolutions.Length > 0 && ResolutionIndex >= 0 && ResolutionIndex < resolutions.Length)
        {
            Resolution r = resolutions[ResolutionIndex];
            Screen.SetResolution(r.width, r.height, Fullscreen, r.refreshRate);
        }
    }

    public void Load()
    {
        MusicVolume = PlayerPrefs.GetFloat(KeyMusic, MusicVolume);
        SfxVolume = PlayerPrefs.GetFloat(KeySfx, SfxVolume);
        Fullscreen = PlayerPrefs.GetInt(KeyFullscreen, Fullscreen ? 1 : 0) == 1;
        ResolutionIndex = PlayerPrefs.GetInt(KeyResolution, ResolutionIndex);
        if (resolutions == null) resolutions = Screen.resolutions;
        ResolutionIndex = Mathf.Clamp(ResolutionIndex, 0, resolutions.Length > 0 ? resolutions.Length - 1 : 0);
    }
}
