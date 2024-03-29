using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 5f;
    private UIManager _uiManager;
    [SerializeField] private float _speedBoostSpeed = 12.0f;
    [SerializeField] private float _currentSpeed;
    private float _horizontalInput;
    private float _verticalInput;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _beamsPrefab;
    [SerializeField] private GameObject _shieldVisual;
    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private GameObject _rightEngine;
    Vector3 _offset;
    private Vector3 _offsetTripleShot;
    private Vector3 _offsetBeams;
    private int _lives = 3;
    private float _fireRate = .15f;
    private float _canFire = -1f;
    [SerializeField] private float _fireRocketRate = 1f;
    [SerializeField] private float _canFireRockets = -1f;
    //-----
    private SpawnManager _spawnManager;
    //-----Activating Powerups
    private bool _isTripleShotActive;
    [SerializeField] private GameObject _tripleShotPrefab;
    private bool _isSpeedBoostActive;
    private bool _isShieldsActive;
    private bool _collectedBeamsPowerup;
    private bool _beamsActivated;
    private bool _isAmmoJamActive = false;

    //-----
    [SerializeField] private int _score;
    private AudioManager _audioManager;
    //----Shield Powerup Variables
    [SerializeField] private int _shieldCount = 0;
    private Color _color = Color.white;
    [SerializeField] private SpriteRenderer _shieldColor;
    //-----Ammo Count UI
    [SerializeField] private int _ammoCount;
    private int _ammoMaxCount = 99;
    //-----Rare Powerup
    GameObject beamsGO;
    //-----Thrusters
    private ThrustersSlider _thrustersSlider;
    //-----Correction of Lasers from following the Enemy movement
    [SerializeField] private GameObject _playerLaserContainer;
    //Player Presses C to Collect Powerups
    private bool _collectPowerups;
    //Homing Missile
    [SerializeField] private bool _isHomingActive;
    [SerializeField] private GameObject _homingPrefab;
    [SerializeField] private float _time;


    void Start()
    {
        _currentSpeed = _speed;
        _offset = new Vector3(0f, 1.05f, 0f);
        _offsetTripleShot = new Vector3(0f, 1.5f, 0f);
        _offsetBeams = new Vector3(0f, -1f, 0f);
        transform.position = new Vector3(0f, -4.5f, 0f);

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

        _thrustersSlider = _uiManager.transform.Find("ThrustersSlider").GetComponent<ThrustersSlider>();

        if (_thrustersSlider == null)
        {
            Debug.LogError("Player::ThrustersSlider is null");
        }
        _thrustersSlider.SetThrusters(100);
        _thrustersSlider.UpdateThrustersUI();
    }

    void Update()
    {
        _time = Time.time;
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && _isHomingActive == true) //Homing Missile
        {
            if (Time.time > _canFireRockets)
            {
                _canFireRockets = Time.time + _fireRocketRate;
                FireHoming();
            }
        }

        else if (Input.GetKeyDown(KeyCode.Space) && (_collectedBeamsPowerup)) //Rare Beams Powerup
        {
            if (_beamsActivated)
            {
                return;
            }
            else if (!_beamsActivated)
            {
                _beamsActivated = true;
                beamsGO = Instantiate(_beamsPrefab, transform.position + _offsetBeams, Quaternion.identity);
                beamsGO.transform.parent = this.transform;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && (_isAmmoJamActive == true))
        {
            _audioManager.PlayEmptyChamberFx();
        }

        else if (Input.GetKeyDown(KeyCode.Space) && _isHomingActive == false && Time.time > _canFire) //Fire Laser
        {
            if (_ammoCount <= 0)
            {
                _ammoCount = 0;
                _audioManager.PlayEmptyChamberFx();
            }
            else
            {
                FireLaser();
            }
        }


        CalculateThrusters();

        if (Input.GetKeyDown(KeyCode.C))
        {
            _collectPowerups = true;
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            _collectPowerups = false;
        }

    }

    void CalculateThrusters()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _thrustersSlider.AreThrustersCharging() == false)
        {
            _thrustersSlider.BurnThrusters(.055f + Time.deltaTime);
            _thrustersSlider.UpdateThrustersUI();

            _currentSpeed = _speed * 1.8f;

            if (_thrustersSlider.AreThrustersDepleted() == true)
            {
                _currentSpeed = _speed;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _thrustersSlider.RechargeThrusters(.055f + Time.deltaTime);
            _currentSpeed = _speed;
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            GameObject laserGo = Instantiate(_tripleShotPrefab, transform.position + _offsetTripleShot, Quaternion.identity);
            laserGo.transform.parent = GameObject.Find("PlayerLaserContainer").transform;
        }
        else
        {
            GameObject laserGameObject = Instantiate(_laserPrefab, transform.position + _offset, Quaternion.identity);
            laserGameObject.transform.parent = _playerLaserContainer.transform;
        }
        _ammoCount--;
        _uiManager.AmmoCountUpdate(_ammoCount);
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
            transform.Translate(dir * _currentSpeed * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.5f, 0));

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
            _shieldCount -= 1;
            switch (_shieldCount)
            {
                case 1:
                    {
                        _color.a = .5f;
                        _shieldColor.material.color = _color;
                        break;
                    }
                case 2:
                    {
                        _color.a = .75f;
                        _shieldColor.material.color = _color;
                        break;
                    }
                case 3:
                    {
                        _color.a = 1.0f;
                        _shieldColor.material.color = _color;
                        break;
                    }
                default:
                    {
                        if (_shieldCount > 3)
                        {
                            _shieldCount = 3;
                        }
                        else if (_shieldCount < 3)
                        {
                            _shieldCount = 0;
                        }

                        break;
                    }
            }

            if (_shieldCount == 0)
            {
                _isShieldsActive = false;
                _shieldVisual.gameObject.SetActive(false);
            }
            return;
        }

        _lives -= 1;
        _audioManager.PlayExplosionFx();

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
            _lives = 0;

            if (_spawnManager == null)
            {
                Debug.LogError("Player::_spawnManager is null");
            }

            _spawnManager.OnPlayerDeath();

            Destroy(gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isSpeedBoostActive = false;
    }

    public void ShieldsActive()
    {
        _isShieldsActive = true;
        _shieldCount += 1;

        _shieldVisual.gameObject.SetActive(true);

        switch (_shieldCount)
        {
            case 1:
                {
                    _color.a = .5f;
                    _shieldColor.material.color = _color;
                    break;
                }
            case 2:
                {
                    _color.a = .75f;
                    _shieldColor.material.color = _color;
                    break;
                }
            case 3:
                {
                    _color.a = 1.0f;
                    _shieldColor.material.color = _color;
                    break;
                }
            default:
                {
                    if (_shieldCount > 3)
                    {
                        _shieldCount = 3;
                    }
                    else if (_shieldCount < 3)
                    {
                        _shieldCount = 0;
                    }

                    break;
                }
        }
    }

    public void ScoreUpdate(int points)
    {
        _score += points;
        _uiManager.UIScoreUpdate(_score);

    }

    public void ResetAmmoCount()
    {
        _ammoCount = 15;
        _uiManager.AmmoCountUpdate(_ammoCount);
    }

    public void AmmoPowerup()
    {
        _ammoCount += 15;
        if (_ammoCount > _ammoMaxCount)
        {
            _ammoCount = _ammoMaxCount;
        }
        _uiManager.AmmoCountUpdate(_ammoCount);
    }

    public void OneUpPowerup()
    {
        if (_lives == 2)
        {
            int x = Random.Range(0, 2);
            if (x == 0)
            {
                if (_rightEngine.activeInHierarchy == true)
                {
                    _rightEngine.SetActive(false);

                }
                else
                {
                    _leftEngine.SetActive(false);
                }
            }
            else if (x == 1)
            {
                if (_leftEngine.activeInHierarchy == true)
                {
                    _leftEngine.SetActive(false);
                }
                else
                {
                    _rightEngine.SetActive(false);
                }
            }
        }
        else if (_lives == 1)
        {
            int y = Random.Range(0, 2);

            if (y == 0)
            {
                _leftEngine.SetActive(false);
            }
            else if (y == 1)
            {
                _rightEngine.SetActive(false);
            }
        }

        _lives += 1;

        if (_lives > 3)
        {
            _lives = 3;
        }

        _uiManager.UpdateLives(_lives);
    }

    public void BeamsPowerup()
    {
        _collectedBeamsPowerup = true;
        StartCoroutine(BeamsPowerDownRoutine());
    }

    IEnumerator BeamsPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _collectedBeamsPowerup = false;
        _beamsActivated = false;
        Destroy(beamsGO);
    }

    public void HomingPowerup()
    {
        _isHomingActive = true;
        StartCoroutine(HomingPowerDownRoutine());
    }

    IEnumerator HomingPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isHomingActive = false;
    }

    public void AmmoJamPickup()
    {
        _isAmmoJamActive = true;
        StartCoroutine(AmmoJamPowerDownRoutine());
    }

    IEnumerator AmmoJamPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isAmmoJamActive = false;
    }

    public bool HasTakenDamage()
    {
        if (_lives < 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsCPressed()
    {
        return _collectPowerups;
    }

    private void FireHoming()
    {

        if (_isHomingActive == true)
        {
            GameObject go = Instantiate(_homingPrefab, transform.position + _offsetTripleShot, Quaternion.identity);
        }
    }


    //powerup is collected
    //Powerup up script calls homing Powerup method.
    //Homing Powerup method turns _isHomingActive to true
    //homing poweurp method starts homingPowerdownRoutine
    //homingPowerDownRoutine waits 8 seconds and returns _isHomingActive to false
    //create new method to dictate rocket behavior
    //rocket will cast a circle from point of origin that is the player
    //rocket will put results inside an array
    //results will be sorted by distance from player
    //loop through results/enemies detected within the circle raycast
    //homing missile is sent out to nearest target of result with the least amount of distance

}

