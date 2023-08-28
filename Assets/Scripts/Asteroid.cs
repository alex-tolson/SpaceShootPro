using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private GameObject _asteroid;
    [SerializeField] private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;
    private AudioManager _audioManager;
    private UIManager _uiManager;
    private Player _player;
    private CameraShake _cameraShake;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Asteroid::SpawnManager is null");
        }
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("Asteroid::AudioSource is null");
        }
        //
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("Asteroid::UIManager is null");
        }
        //
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Asteroid::Player is null");
        }
        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        if (_cameraShake == null)
        {
            Debug.LogError("Asteroid::CameraShake is null");
        }

    }

    void Update()
    {
        AsteroidFall();

    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Laser"))
        {
            Destroy(gameObject);
            GameObject go = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _audioManager.PlayExplosionFx();
            Destroy(go, 3f);
        }
        if (other.CompareTag("Player"))
        {
            _player.Damage();
            _cameraShake.StartCamShake();
            Destroy(gameObject);
            GameObject go = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _audioManager.PlayExplosionFx();
            Destroy(go, 3f);

        }
    }

    public void AsteroidFall()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -9f)
        {
            Destroy(gameObject);
        }

    }
}
