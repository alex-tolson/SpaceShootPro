using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    private AudioSource _audioSourcePlayer;
    private AudioSource _audioSource;
    [SerializeField] private float _volume = .7f;
    [SerializeField] private AudioClip _laserClip;
    [SerializeField] private AudioClip _explosionClip;
    [SerializeField] private AudioClip _powerUpClip;
    [SerializeField] private AudioClip _emptyChamberClip;
    [SerializeField] private AudioClip _reloadChamberClip;
    [SerializeField] private AudioSource _bossBattle;
    [SerializeField] private AudioSource _bgMusic;

    private void Start()
    {
        _audioSourcePlayer = GameObject.Find("Player").GetComponent<AudioSource>();
        if (_audioSourcePlayer == null)
        {
            Debug.LogError("AudioManager::AudioSourcePlayer is null");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("AudioManager::AudioSource is null");
        }
        _bossBattle = GameObject.Find("BossBattle").GetComponent<AudioSource>();
        if (_bossBattle == null)
        {
            Debug.LogError("AudioManager::Boss Battle is null");
        }
        _bgMusic = GameObject.Find("BGMusic").GetComponent<AudioSource>();
        if (_bgMusic == null)
        {
            Debug.LogError("AudioManager::BGMusic is null");
        }
    }

    public void PlayLaserFx()
    {
        _audioSourcePlayer.PlayOneShot(_laserClip, _volume);
    }

    public void PlayExplosionFx()
    {
        _audioSource.PlayOneShot(_explosionClip, _volume);
    }

    public void PlayPowerupFx()
    {
        _audioSource.PlayOneShot(_powerUpClip, _volume);
    }

    public void PlayEmptyChamberFx()
    {
        _audioSource.PlayOneShot(_emptyChamberClip, _volume);
    }
    public void PlayReloadChamberFx()
    {
        _audioSource.PlayOneShot(_reloadChamberClip, _volume);
    }
    public void PlayBossBattleMusic()
    {
        _bossBattle.Play();
    }
    public void PlayBackgroundMusic()
    {
        _bgMusic.Play();
    }

    IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        _bgMusic.Stop();
    }
    IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
    {
        _bossBattle.Play();
        float startVolume = audioSource.volume;
        while (audioSource.volume < .8f)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
    }

    public void StartFadeOut(AudioSource audioSource, float fadeTime)
    {
        StartCoroutine(FadeOut(audioSource, fadeTime));
    }
    public void StartFadeIn(AudioSource audioSource, float fadeTime)
    {
        StartCoroutine(FadeIn(audioSource, fadeTime));
    }
}


