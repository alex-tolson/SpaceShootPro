using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private GameObject _asteroid;
    [SerializeField] private Vector3 _rot;
    [SerializeField] private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;
    private AudioManager _audioManager;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Asteroid::SpawnManager is null");
        }
        _rot = new Vector3(0f, 0f, 90f);

        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("Asteroid::AudioSource is null");
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
            Destroy(go, 3f);
            _spawnManager.StartSpawning();
            Destroy(GetComponent<Collider2D>());
            Destroy(_asteroid, .3f);
        }
    }

}
