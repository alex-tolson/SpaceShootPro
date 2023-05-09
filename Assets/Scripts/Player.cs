using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;

    private float _horizontalInput;
    private float _verticalInput;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private int _lives = 3;
    [SerializeField] private float _fireRate = .15f;
    private float _canFire = -1f;

    private SpawnManager _spawnManager;

    [SerializeField] private bool _isTripleShotActive;
    [SerializeField] private GameObject _tripleShotPrefab;


    void Start()
    {
        _offset = new Vector3(transform.position.x, 1.05f, transform.position.z);
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();    
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
    }

    void CalculateMovement()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * _horizontalInput * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * _verticalInput * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4f, 0));

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
        _lives -= 1;

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
}

