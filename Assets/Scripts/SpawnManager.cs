using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject _bigBoss;
    private static List<GameObject> _enemies = new List<GameObject>();
   
    [SerializeField] private GameObject[] powerups;
    private UIManager _uiManager;
    private Player _player;
    private float _timeWaiting = 5.0f;
    private float _timeTillSpawn = 5f;
    private float _timeBetweenRolls = 3.0f;
    private int _waveCount = 0;
    private float _waveTime = 0;
    private float _timer;
    [SerializeField] private int _percentiles;
    [SerializeField] private int _randomPowerup;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("SpawnManager::UIManager is null");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("SpawnManager::Player is null");
        }
    }


    public void StartSpawning()
    {
        StartCoroutine("SpawnEnemyRoutine");
        StartCoroutine("SpawnPowerupRoutine");
    }


    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);

        Vector3 loc;
        while (_timer <= _waveTime)
        {
            float randX = Random.Range(-10f, 10f);
            loc = new Vector3(randX, 10.0f, 0f);

            GameObject newEnemy = Instantiate(enemyPrefab, loc, Quaternion.identity);
            _enemies.Add(newEnemy);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_timeTillSpawn);
            _timer += _timeWaiting + Time.deltaTime;
        }
        EndWave();
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(4f);

        Vector3 location;

        while (_timer <= _waveTime)
        {
            _percentiles = Random.Range(1, 101);

            float randomX = Random.Range(-10f, 10f);
            location = new Vector3(randomX, 10.0f, 0f);

            if (_percentiles <= 55)  
            {
                _randomPowerup = Random.Range(0, 6);
                if ((_randomPowerup == 0) || (_randomPowerup == 2) || (_randomPowerup == 4))
                {
                    _randomPowerup += 1;
                }

                Instantiate(powerups[_randomPowerup], location, Quaternion.identity);
            }//low tier is 1, 3, and 5
            else if ((_percentiles > 56) && (_percentiles < 86)) 
            {
                _randomPowerup = Random.Range(0, 2);

                if (_randomPowerup > 0)
                {
                    _randomPowerup += 1;
                }

                Instantiate(powerups[_randomPowerup], location, Quaternion.identity);
            }// middle tier is 0 and 2
            else if ((_percentiles < 85)) // High tier is 4, 6, and 7
            {
                if (_player.HasTakenDamage())
                {
                    _randomPowerup = Random.Range(4, 7);

                    if (_randomPowerup > 4)
                    {
                        _randomPowerup += 1;
                    }
                }
                else
                {
                    _randomPowerup = Random.Range(6, 8);
                }

                Instantiate(powerups[_randomPowerup], location, Quaternion.identity);
            }
            yield return new WaitForSeconds(_timeBetweenRolls);
        }
    }

    public void OnPlayerDeath()
    {
        _uiManager.GameOverActions();
    }

    public void BeginWave(int Count, float Time)
    {
        _waveCount += Count;
        _waveTime += Time;
        if (_waveCount < 7)
        {
            _uiManager.DisplayWaveInfo(_waveCount, _waveTime);
        }
        else
        {
            _uiManager.FinalWaveUI();
            BeginFinalWave();
        }
    }

    public void EndWave()
    {
        _timeBetweenRolls += .2f;
        _timeTillSpawn -= .2f;

        if (_timeBetweenRolls > 10)
        {
            _timeBetweenRolls = 10;
        }
        if (_timeTillSpawn < 1)
        {
            _timeTillSpawn = 1;
        }

        foreach (GameObject enemy in _enemies)
        {
            if (enemy != null && enemy.CompareTag("Enemy"))
            {
                Destroy(enemy);
            }          
        }
        _timer = 0;
        _enemies.Clear();
        StopAllCoroutines();
        BeginWave(1, 10);
    }

    public int whatWaveCountIsIt()
    {
        return _waveCount;
    }

    private void BeginFinalWave()
    {
        _bigBoss.SetActive(true);
        StartCoroutine(InvincibleFrames());
        StartCoroutine("SpawnPowerupRoutine");
    }
    IEnumerator InvincibleFrames()
    {
        _bigBoss.GetComponent<Collider2D>().gameObject.SetActive(false);
        yield return new WaitForSeconds(4.5f);
        _bigBoss.GetComponent<Collider2D>().gameObject.SetActive(true);
        //Introduce boss. turn off collider2d
        //till animation finishes playing
    }
}

