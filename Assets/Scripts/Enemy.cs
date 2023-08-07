using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    private Animator _animator;
    private Player _player;
    private AudioManager _audioManager;

    private SpawnManager _spawnManager;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _heatSeekOffset;
    [SerializeField] private int _fireRate;
    [SerializeField] private float _canFire = -1f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _heatSeekLaserPrefab;
    [SerializeField] private GameObject _shield;

    //cam shake
    private CameraShake _camShake;
    //new enemy
    [SerializeField] private int _enemyType;
    //-----New enemy movement--------////
    private Vector3 _leftSideScreen;
    private Vector3 _rightSideScreen;
    private bool _goRight;
    private float _dist2SideScreen;
    //New Enemy Movement - enemy Type 2-----/////
    [SerializeField] private float _waveSpeed = 4f;
    private float _waveHeight = 2;
    private float _waveFrequency = 2;
    private float _sineWave;
    private Vector3 _sineCurve;
    private bool _enemyIsType2 = false;


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)//-----------NullChecking Player------------
        {
            Debug.LogError("Enemy::Player null");
        }

        _animator = GetComponent<Animator>();
        if (_animator == null)//-----------NullChecking Animator------------
        {
            Debug.LogError("Enemy::Animator null");
        }

        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audioManager == null)//-----------NullChecking Audio Manager------------
        {
            Debug.LogError("Asteroid::AudioSource is null");
        }
        _camShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        if (_camShake == null)//-----------NullChecking Camera Shake------------
        {
            Debug.LogError("Enemy::CameraShake is null");
        }

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Enemy::SpawnManager is null");
        }
        _offset = new Vector3(0.0f, -1.75f, 0.0f);
        _heatSeekOffset = new Vector3(0.0f, -2.0f, 0.0f);
        //-------------Enemies by Waves---------------/////

        // ------------------------------------------------------------------------
        EnemyBalancedSpawning();
        StartCoroutine(FireLaserRoutine());   
    } 

    void Update()
    {
        _leftSideScreen = new Vector3(-10.5f, transform.position.y, transform.position.z);
        _rightSideScreen = new Vector3(10.5f, transform.position.y, transform.position.z);

        EnemyMov(_enemyType);
    }

    void EnemyMov(int enemyId)
    {
        switch (enemyId)
        {
            case 0:
                {
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);

                    if (transform.position.y < -6f)
                    {
                        float randomX = Random.Range(-10f, 10f);
                        transform.position = new Vector3(randomX, 8f, transform.position.z);
                    }
                    break;
                }
            case 1:
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
                    break;

                }
            case 2:
                {

                    _sineWave = _waveHeight * Mathf.Sin((_waveSpeed * Time.time) + (_waveFrequency));
                    _sineCurve = new Vector3(_sineWave, -1, 0);

                    transform.Translate(_sineCurve * _speed * Time.deltaTime);

                    if (transform.position.y < -6f)
                    {
                        float randomX = Random.Range(-10f, 10f);
                        transform.position = new Vector3(randomX, 8f, transform.position.z);
                    }
                    break;
                }
            case 3:
                {
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);

                    if (transform.position.y < -6f)
                    {
                        float randomX = Random.Range(-10f, 10f);
                        transform.position = new Vector3(randomX, 8f, transform.position.z);
                    }
                    break;

                }
            default:
                { 
                    break; 
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

        else if (other.CompareTag("Laser"))
        {
            if (_shield.activeInHierarchy == true)
            {
                _shield.SetActive(false);
                _audioManager.PlayExplosionFx();
            }
            else
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
    }

    IEnumerator FireLaserRoutine()
    {
        while (true)
        {
            _fireRate = Random.Range(2, 5);
            yield return new WaitForSeconds(_fireRate);

            if (isEnemyType2() && (_canFire < Time.deltaTime + _fireRate))
            {
                _canFire += _fireRate;
                GameObject laserGO = Instantiate(_heatSeekLaserPrefab, transform.position + _heatSeekOffset, Quaternion.identity);
                laserGO.transform.parent = GameObject.Find("EnemyLaser").transform;
            }
            else if (_canFire < Time.deltaTime + _fireRate)
            {
                _canFire += _fireRate;
                GameObject laserGO = Instantiate(_laserPrefab, transform.position + _offset, Quaternion.identity);
                laserGO.transform.parent = GameObject.Find("EnemyLaser").transform;
            }
        }
    }

    public bool isEnemyType2()
    {
        return _enemyIsType2;
    }

    private void EnemyBalancedSpawning()
    {
        if (_spawnManager.whatWaveCountIsIt() <= 3)
        {
            _enemyType = Random.Range(0, 3);

        }
        else if (_spawnManager.whatWaveCountIsIt() >= 4 && _spawnManager.whatWaveCountIsIt() <= 6)
        {
            _enemyType = Random.Range(0, 4);

        }
        else if (_spawnManager.whatWaveCountIsIt() >= 7)
        {
            _enemyType = Random.Range(0, 8);
        }
        //----------------------------------------------------------------------------------------------
        if (_enemyType == 1)
        {
            transform.position = new Vector3(10.5f, Random.Range(.5f, 5f), transform.position.z);
        }
        else if (_enemyType == 2)
        {
            _speed = 1.75f;
            _enemyIsType2 = true;
        }
        else if (_enemyType == 3)
        {
            _shield.SetActive(true);
            
        }
    }
}
