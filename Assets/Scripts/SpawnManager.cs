using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private WaitForSeconds _timeWaiting = new WaitForSeconds(5f);
    

    [SerializeField] private GameObject _enemyContainer;

    private bool _stopSpawning = false;

    [SerializeField] private GameObject[] powerups;
    [SerializeField] private UIManager _uiManager;


    void Start()
    {
        StartCoroutine("SpawnEnemyRoutine");
        StartCoroutine("SpawnPowerupRoutine");
    }

    IEnumerator SpawnEnemyRoutine()
    {
        Vector3 loc;                                    

        while (_stopSpawning == false)              
        {
            float randX = Random.Range(-10f, 10f); 
            loc = new Vector3(randX, 8.0f, 0f);

            GameObject newEnemy = Instantiate(enemyPrefab, loc, Quaternion.identity);

            newEnemy.transform.parent = _enemyContainer.transform;

            yield return _timeWaiting;
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        Vector3 location;
        while (_stopSpawning == false)
        {
            int randomPowerup = (Random.Range(0, 3));
            int randomSecs = Random.Range(3, 8);
            float randomX = Random.Range(-10f, 10f);
            location = new Vector3(randomX, 8.0f, 0f);

            Instantiate(powerups[randomPowerup], location, Quaternion.identity);

            yield return new WaitForSeconds(randomSecs);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
        _uiManager.GameOverActions();
        
    }
}
