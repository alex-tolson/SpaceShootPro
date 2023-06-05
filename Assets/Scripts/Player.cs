using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;
    private UIManager _uiManager;
    [SerializeField] private float _speedBoostSpeed = 8.5f;
    private float _horizontalInput;
    private float _verticalInput;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _shieldVisual;
    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private int _lives = 3;
    [SerializeField] private float _fireRate = .15f;
    private float _canFire = -1f;
    
    private SpawnManager _spawnManager;

    private bool _isTripleShotActive;
    [SerializeField] private GameObject _tripleShotPrefab;
    private bool _isSpeedBoostActive;
    private bool _isShieldsActive;

    [SerializeField] private int _score;
    private AudioManager _audioManager;

    void Start()
    {
        _offset = new Vector3(0f, 1.05f, 0f);
        transform.position = new Vector3(0, 0, 0);
        
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("Player::_uiManager is null");
        }

        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("Player::AudioManager is null");
        }
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        
        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + _offset, Quaternion.identity);
        }

        _audioManager.PlayLaserFx();
    }

    void CalculateMovement()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        Vector3 dir = (new Vector3(_horizontalInput, _verticalInput, 0));

        if (_isSpeedBoostActive)
        {
            transform.Translate(dir * _speedBoostSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(dir * _speed * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.6f, 0));

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    public void Damage()
    {
        if (_isShieldsActive == true)
        {
            _isShieldsActive= false;
            _shieldVisual.gameObject.SetActive(false);
            return;
        }

        _lives -= 1;

        if (_lives == 2)
        {
            int randEngine = Random.Range(0, 2);
            if (randEngine == 0)
            {
                _leftEngine.gameObject.SetActive(true);
            }
            else if (randEngine == 1)
            {
                _rightEngine.gameObject.SetActive(true);
            }
        }

        else if (_lives == 1)
        {
            if (_leftEngine.gameObject.activeInHierarchy == true)
            {
                _rightEngine.gameObject.SetActive(true);   
            }
            else
            {
                _leftEngine.gameObject.SetActive(true);
            }
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            if (_spawnManager == null)
            {
                Debug.LogError("Player::_spawnManager is null");
            }

            _spawnManager.OnPlayerDeath();

            Destroy(this.gameObject);

        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine("TripleShotPowerDownRoutine");

    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive() 
    {
        _isSpeedBoostActive = true;
        StartCoroutine("SpeedBoostPowerDownRoutine");
    }

    IEnumerator SpeedBoostPowerDownRoutine() 
    {
        yield return new WaitForSeconds(5f);
        _isSpeedBoostActive = false;
    }

    public void ShieldsActive()
    {
        _isShieldsActive = true;
        _shieldVisual.gameObject.SetActive(true);
    }

    public void ScoreUpdate(int points)
    {
        _score += points;
        _uiManager.UIScoreUpdate(_score);

    }
}

