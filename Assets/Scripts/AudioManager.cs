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
}


