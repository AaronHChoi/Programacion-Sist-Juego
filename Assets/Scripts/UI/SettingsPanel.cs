using UnityEngine;
using UnityEngine.UI;

// Connect UI controls to SettingsManager. Assign the UI elements in the inspector.
public class SettingsPanel : MonoBehaviour
{
    [Header("Audio")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Display")]
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;

    [Header("Controls")]
    public Button closeButton;

    private Resolution[] resolutions;

    void OnEnable()
    {
        if (SettingsManager.Instance == null)
        {
            Debug.LogWarning("SettingsManager not found in scene. Create a GameObject with SettingsManager.");
            return;
        }

        // Init resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown?.ClearOptions();
        var options = new System.Collections.Generic.List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            var r = resolutions[i];
            options.Add($"{r.width} x {r.height} @ {r.refreshRate}Hz");
        }
        if (options.Count == 0) options.Add($"{Screen.width} x {Screen.height}");
        resolutionDropdown?.AddOptions(options);

        // Populate UI with current settings
        musicSlider?.SetValueWithoutNotify(SettingsManager.Instance.MusicVolume);
        sfxSlider?.SetValueWithoutNotify(SettingsManager.Instance.SfxVolume);
        fullscreenToggle?.SetIsOnWithoutNotify(SettingsManager.Instance.Fullscreen);
        resolutionDropdown?.SetValueWithoutNotify(SettingsManager.Instance.ResolutionIndex);

        // Listeners
        if (musicSlider != null) musicSlider.onValueChanged.AddListener(OnMusicChanged);
        if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(OnSfxChanged);
        if (fullscreenToggle != null) fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
        if (resolutionDropdown != null) resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        if (closeButton != null) closeButton.onClick.AddListener(ClosePanel);
    }

    void OnDisable()
    {
        // Remove listeners to avoid duplicate subscriptions when toggling the panel
        if (musicSlider != null) musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
        if (sfxSlider != null) sfxSlider.onValueChanged.RemoveListener(OnSfxChanged);
        if (fullscreenToggle != null) fullscreenToggle.onValueChanged.RemoveListener(OnFullscreenChanged);
        if (resolutionDropdown != null) resolutionDropdown.onValueChanged.RemoveListener(OnResolutionChanged);
        if (closeButton != null) closeButton.onClick.RemoveListener(ClosePanel);
    }

    public void OnMusicChanged(float v)
    {
        // Persist/apply through SettingsManager (already present)
        SettingsManager.Instance?.SetMusicVolume(v);

        // Also update SoundManager directly for immediate feedback if available
        if (SoundManager.Instance != null) SoundManager.Instance.SetMusicVolume(v);
    }

    public void OnSfxChanged(float v)
    {
        SettingsManager.Instance?.SetSfxVolume(v);
        if (SoundManager.Instance != null) SoundManager.Instance.SetSfxVolume(v);
    }

    public void OnFullscreenChanged(bool on)
    {
        SettingsManager.Instance.SetFullscreen(on);
    }

    public void OnResolutionChanged(int index)
    {
        SettingsManager.Instance.SetResolution(index);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        // refresh values in case changed elsewhere
        musicSlider?.SetValueWithoutNotify(SettingsManager.Instance.MusicVolume);
        sfxSlider?.SetValueWithoutNotify(SettingsManager.Instance.SfxVolume);
        fullscreenToggle?.SetIsOnWithoutNotify(SettingsManager.Instance.Fullscreen);
        resolutionDropdown?.SetValueWithoutNotify(SettingsManager.Instance.ResolutionIndex);
        gameObject.SetActive(true);
    }
}
