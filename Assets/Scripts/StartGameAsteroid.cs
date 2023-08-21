using UnityEngine;


public class StartGameAsteroid : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private GameObject _asteroid;
    [SerializeField] private Vector3 _rot;
    [SerializeField] private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;
    private AudioManager _audioManager;
    private UIManager _uiManager;
    private Player _player;


    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Asteroid::SpawnManager is null");
        }
        //
        _rot = new Vector3(0f, 0f, 90f);
        //
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
    }

    void Update()
    {
        transform.Rotate(_rot * _speed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Laser"))
        {
            GameObject go = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _audioManager.PlayExplosionFx();
            _player.ResetAmmoCount();
            Destroy(go, 3f);
            _spawnManager.BeginWave(1, 10f);
            Destroy(GetComponent<Collider2D>());
            //Destroy(_asteroid, .3f);
            gameObject.SetActive(false);
        }
    }

}
