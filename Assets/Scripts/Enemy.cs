using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    private Animator _animator;
    private Player _player;
    private AudioManager _audioManager;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private int _fireRate;
    [SerializeField] private float _canFire = -1f;
    [SerializeField] private GameObject _laserPrefab;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Enemy::Player null");
        }
        //
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Enemy::Animator null");
        }
        //
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("Asteroid::AudioSource is null");
        }
        _offset = new Vector3(0.0f, -1.75f, 0.0f);

        StartCoroutine(FireLaserRoutine());
    }

    void Update()
    {
        EnemyMov();
        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-10f, 10f);
            transform.position = new Vector3(randomX, 8f, transform.position.z);
        }
    }

    void EnemyMov()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (_player != null)
            {
                _player.Damage();                
            }
            _speed = 0;
            _animator.SetTrigger("OnEnemyDeath");
            _audioManager.PlayExplosionFx();
            Destroy(gameObject, 2.5f);
        }
        if (other.CompareTag("Laser"))
        {
            if (_player != null)
            {
                _player.ScoreUpdate(10);
            }
            Destroy(other.gameObject);
            _speed = 0;
            _animator.SetTrigger("OnEnemyDeath");
            _audioManager.PlayExplosionFx();
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.5f);
        }
    }

    IEnumerator FireLaserRoutine()
    {
        while (true)
        {
            _fireRate = Random.Range(3, 6);
            yield return new WaitForSeconds(_fireRate);

            if (_canFire < Time.deltaTime + _fireRate)
            {
                _canFire += _fireRate;
                GameObject laserGo = Instantiate(_laserPrefab, transform.position + _offset, Quaternion.identity);
                laserGo.transform.parent = this.transform;
            }
           
        }
    }
}
