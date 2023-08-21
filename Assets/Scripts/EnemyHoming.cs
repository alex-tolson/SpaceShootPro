using UnityEngine;

public class EnemyHoming : MonoBehaviour
{
    private Player _player;
    [SerializeField] private float _homingSpeed = 2;
    private Vector3 _direction;
    [SerializeField] private GameObject _explosionPrefab;
    private AudioManager _audio;
    private CameraShake _camShake;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("EnemyHoming::Player is null");
        }
        _audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audio == null)
        {
            Debug.LogError("EnemyHoming::AudioManager is null");
        }
        _camShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        if (_camShake == null)//-----------NullChecking Camera Shake------------
        {
            Debug.LogError("EnemyHoming::CameraShake is null");
        }

        //DestroyRocket();
    }

    private void Update()
    {

        if (_player != null)
        {
            LookAtTarget();
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _homingSpeed * Time.deltaTime);
        }

    }

    private void LookAtTarget()
    {
        _direction = _player.transform.position - transform.position;
        Quaternion rocketRotation = Quaternion.LookRotation(new Vector3(0, 0, 1), _direction);
        transform.rotation = rocketRotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if (other.CompareTag("Laser"))
        {
            Destroy(gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _audio.PlayExplosionFx();
        }
        if (other.CompareTag("Player"))
        {
            //_player.Damage();
            _camShake.StartCamShake();
            Destroy(gameObject);
        }
    }

    private void DestroyRocket()
    {
        Destroy(gameObject, 7.0f);
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _audio.PlayExplosionFx();
    }
}
