using UnityEngine;
using UnityEngine.UI;

// Connect UI controls to SettingsManager. Assign the UI elements in the inspector.
public class SettingsPanel : MonoBehaviour
{
    [Header("Audio")]
    public Slider musicSlider;
    public Slider sfxSlider;

 

    [Header("Controls")]
    public Button closeButton;

    // resolution functionality removed per request

    void OnEnable()
    {
        if (SettingsManager.Instance == null)
        {
            Debug.LogWarning("SettingsManager not found in scene. Create a GameObject with SettingsManager.");
            return;
        }

        // Populate UI with current settings
        musicSlider?.SetValueWithoutNotify(SettingsManager.Instance.MusicVolume);
        sfxSlider?.SetValueWithoutNotify(SettingsManager.Instance.SfxVolume);
        

        // Listeners
        if (musicSlider != null) musicSlider.onValueChanged.AddListener(OnMusicChanged);
        if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(OnSfxChanged);
        
        if (closeButton != null) closeButton.onClick.AddListener(ClosePanel);
    }

    void OnDisable()
    {
        // Remove listeners to avoid duplicate subscriptions when toggling the panel
        if (musicSlider != null) musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
        if (sfxSlider != null) sfxSlider.onValueChanged.RemoveListener(OnSfxChanged);
        
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
        // resolution support removed
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
        
        gameObject.SetActive(true);
    }
}
