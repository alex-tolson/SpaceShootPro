using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;

    private static List<GameObject> _enemies = new List<GameObject>();
   
    [SerializeField] private GameObject[] powerups;
    [SerializeField] private UIManager _uiManager;

    [SerializeField] private float _timeWaiting = 5.0f;
    [SerializeField] private float _timeTillSpawn = 5.0f;

    [SerializeField] private int _waveCount = 0;
    [SerializeField] private float _waveTime = 0;
    [SerializeField] private float _timer;


    public void StartSpawning()
    {
        StartCoroutine("SpawnEnemyRoutine");
        StartCoroutine("SpawnPowerupRoutine");
        StartCoroutine("SpawnRarePowerupRoutine");
    }
    private void Update()
    {

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
        yield return new WaitForSeconds(3.5f);

        Vector3 location;
        while (_timer <= _waveTime)
        {
            int randomPowerup = Random.Range(0,6);
            int randomSecs = Random.Range(3, 8);
            float randomX = Random.Range(-10f, 10f);
            location = new Vector3(randomX, 10.0f, 0f);

            Instantiate(powerups[randomPowerup], location, Quaternion.identity);

            yield return new WaitForSeconds(randomSecs);
        }
    }

    IEnumerator SpawnRarePowerupRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (_timer <= _waveTime)
        {
            int randomNum = (Random.Range(15, 27));
            yield return new WaitForSeconds(randomNum);

            float randomX = Random.Range(-10f, 10f);
            Vector3 location = new Vector3(randomX, 10.0f, 0f);

            Instantiate(powerups[6], location, Quaternion.identity);
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
        _uiManager.DisplayWaveInfo(_waveCount, _waveTime);
    }

    public void EndWave()
    {
        _timeTillSpawn -= .3f;

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
}

