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
    //cam shake
    [SerializeField] private CameraShake _camShake;
    //new enemy
    [SerializeField] private int _enemyType;
    //-----New enemy movement
    [SerializeField] private Vector3 _leftSideScreen;
    [SerializeField] private Vector3 _rightSideScreen;
    [SerializeField] private bool _goRight;
    [SerializeField] private float _dist2SideScreen;

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
        //
        _camShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        if (_camShake == null)
        {
            Debug.LogError("Enemy::CameraShake is null");
        }
  
        StartCoroutine(FireLaserRoutine());

        _enemyType = Random.Range(0, 2);

        if (_enemyType == 1)
        {
            transform.position = new Vector3(10.5f, Random.Range(.5f, 5f), transform.position.z);
        }
    }

    void Update()
    {
        _leftSideScreen = new Vector3(-10.5f, transform.position.y, transform.position.z);
        _rightSideScreen = new Vector3(10.5f, transform.position.y, transform.position.z);

        EnemyMov(_enemyType);
    }

    void EnemyMov(int enemyId)
    {

        if (enemyId == 0)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y < -6f)
            {
                float randomX = Random.Range(-10f, 10f);
                transform.position = new Vector3(randomX, 8f, transform.position.z);
            }
        }
        else if (enemyId == 1)
        {
            if (_goRight == false)             
            {
                _dist2SideScreen = Vector3.Distance(transform.position, _leftSideScreen); 

                transform.Translate(Vector3.left * _speed * Time.deltaTime);      
                if (_dist2SideScreen <= 1f)
                {
                    _goRight = true;
                }
            }
            else      
            {
                _dist2SideScreen = Vector3.Distance(transform.position, _rightSideScreen); 

                transform.Translate(Vector3.right * _speed * Time.deltaTime);

                if (_dist2SideScreen <= 1f)                                             
                {
                    _goRight = false;                                             
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_player != null)
            {
                _player.Damage();
                _camShake.StartCamShake();
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

            _speed = 0;
            _animator.SetTrigger("OnEnemyDeath");
            _audioManager.PlayExplosionFx();
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.8f);
        }
    }

    IEnumerator FireLaserRoutine()
    {
        while (true)
        {
            _fireRate = Random.Range(3, 5);
            yield return new WaitForSeconds(_fireRate);

            if (_canFire < Time.deltaTime + _fireRate)
            {
                _canFire += _fireRate;
                GameObject laserGO = Instantiate(_laserPrefab, transform.position + _offset, Quaternion.identity);
                laserGO.transform.parent = this.transform;
            }
        }
    }

}
