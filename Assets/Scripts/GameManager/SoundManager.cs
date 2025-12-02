using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [Header("Music Clips")]
    [SerializeField] private AudioClip _mainMenuMusic;
    [SerializeField] private AudioClip _gameplayMusic;
    [SerializeField] private AudioClip _otherMusic;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip _playerDeathClip;
    [SerializeField] private AudioClip _enemyDeathClip;
    [SerializeField] private AudioClip _victoryClip;
    [SerializeField] private AudioClip _uiButtonClip;
    [SerializeField] private AudioClip _playerDamagedClip;
    [SerializeField] private AudioClip _loseClip;
    [SerializeField] private List<AudioClip> _otherSfx = new List<AudioClip>();

    [Header("Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float _musicVolume = 0.6f;
    [Range(0f, 1f)]
    [SerializeField] private float _sfxVolume = 1f;
    [SerializeField] private float _musicFadeDuration = 0.5f;

    private readonly Dictionary<string, AudioClip> _sfxMap = new Dictionary<string, AudioClip>(StringComparer.OrdinalIgnoreCase);
    private Coroutine _musicFadeCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Ensure AudioSources exist
        if (_musicSource == null)
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
        }

        if (_sfxSource == null)
        {
            _sfxSource = gameObject.AddComponent<AudioSource>();
            _sfxSource.loop = false;
        }

        ApplyVolumes();
        BuildSfxMap();
    }

    void OnValidate()
    {
        // Keep inspector changes reflected in play mode where possible
        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        if (_musicSource != null)
            _musicSource.volume = _musicVolume;
        if (_sfxSource != null)
            _sfxSource.volume = _sfxVolume;
    }

    private void BuildSfxMap()
    {
        _sfxMap.Clear();
        if (_playerDeathClip != null) _sfxMap["PlayerDeath"] = _playerDeathClip;
        if (_enemyDeathClip != null) _sfxMap["EnemyDeath"] = _enemyDeathClip;
        if (_victoryClip != null) _sfxMap["Victory"] = _victoryClip;
        if (_uiButtonClip != null) _sfxMap["UIButton"] = _uiButtonClip;
        if (_playerDamagedClip != null) _sfxMap["PlayerDamaged"] = _playerDamagedClip;
        if (_loseClip != null) _sfxMap["Lose"] = _loseClip;

        for (int i = 0; i < _otherSfx.Count; i++)
        {
            var clip = _otherSfx[i];
            if (clip == null) continue;
            var key = $"Other_{i}";
            // Only add if key not already present
            if (!_sfxMap.ContainsKey(key)) _sfxMap[key] = clip;
        }
    }

    // Facade methods for common sounds
    public void PlayPlayerDeath() => PlaySfx(_playerDeathClip);
    public void PlayEnemyDeath() => PlaySfx(_enemyDeathClip);
    public void PlayVictory() => PlaySfx(_victoryClip);
    public void PlayUIButton() => PlaySfx(_uiButtonClip);

    public void PlayPlayerDamaged() => PlaySfx(_playerDamagedClip);
    public void PlayLose() => PlaySfx(_loseClip);

    public void PlayMusicMainMenu() => PlayMusic(_mainMenuMusic, true);
    public void PlayMusicGameplay() => PlayMusic(_gameplayMusic, true);
    public void PlayMusicOther() => PlayMusic(_otherMusic, true);

    // General-purpose API
    public void PlaySfx(AudioClip clip)
    {
        if (clip == null || _sfxSource == null) return;
        _sfxSource.PlayOneShot(clip, _sfxVolume);
    }

    public void PlaySfx(string key)
    {
        if (string.IsNullOrEmpty(key)) return;
        if (_sfxMap.TryGetValue(key, out var clip))
            PlaySfx(clip);
    }

    public void PlayMusic(AudioClip musicClip, bool loop = true, bool crossfade = true)
    {
        if (_musicSource == null) return;

        if (musicClip == null)
        {
            StopMusic();
            return;
        }

        if (_musicSource.clip == musicClip && _musicSource.isPlaying) return;

        if (crossfade)
        {
            if (_musicFadeCoroutine != null) StopCoroutine(_musicFadeCoroutine);
            _musicFadeCoroutine = StartCoroutine(CrossfadeMusic(musicClip, loop, _musicFadeDuration));
        }
        else
        {
            _musicSource.clip = musicClip;
            _musicSource.loop = loop;
            _musicSource.volume = _musicVolume;
            _musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (_musicFadeCoroutine != null)
        {
            StopCoroutine(_musicFadeCoroutine);
            _musicFadeCoroutine = null;
        }

        if (_musicSource != null)
            _musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = Mathf.Clamp01(volume);
        if (_musicSource != null) _musicSource.volume = _musicVolume;
    }

    public void SetSfxVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
        if (_sfxSource != null) _sfxSource.volume = _sfxVolume;
    }

    private IEnumerator CrossfadeMusic(AudioClip newClip, bool loop, float duration)
    {
        var oldSource = _musicSource;
        var startingVolume = oldSource.volume;
        float half = Mathf.Max(0.001f, duration);

        // Fade out
        float t = 0f;
        while (t < half)
        {
            t += Time.unscaledDeltaTime;
            oldSource.volume = Mathf.Lerp(startingVolume, 0f, t / half);
            yield return null;
        }

        oldSource.clip = newClip;
        oldSource.loop = loop;
        oldSource.Play();

        // Fade in
        t = 0f;
        while (t < half)
        {
            t += Time.unscaledDeltaTime;
            oldSource.volume = Mathf.Lerp(0f, _musicVolume, t / half);
            yield return null;
        }

        oldSource.volume = _musicVolume;
        _musicFadeCoroutine = null;
    }
}
