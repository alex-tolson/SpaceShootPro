using System.Collections;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private WaitForSeconds _timeWaiting = new WaitForSeconds(5f);
    
    [SerializeField] private GameObject _enemyContainer;

    private bool _stopSpawning = false;

    [SerializeField] private GameObject[] powerups;
    [SerializeField] private UIManager _uiManager;

    public void StartSpawning()
    {
        StartCoroutine("SpawnEnemyRoutine");
        StartCoroutine("SpawnPowerupRoutine");
        StartCoroutine("SpawnRarePowerupRoutine");
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);

        Vector3 loc;                                    
        while (_stopSpawning == false)              
        {
            float randX = Random.Range(-10f, 10f); 
            loc = new Vector3(randX, 10.0f, 0f);

            GameObject newEnemy = Instantiate(enemyPrefab, loc, Quaternion.identity);

            newEnemy.transform.parent = _enemyContainer.transform;

            yield return _timeWaiting;
        }
        Debug.Log("StopSpawning set to true");
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.5f);

        Vector3 location;
        while (_stopSpawning == false)
        {
            int randomPowerup = (Random.Range(0, 5));
            int randomSecs = Random.Range(3, 8);
            float randomX = Random.Range(-10f, 10f);
            location = new Vector3(randomX, 10.0f, 0f);

            Instantiate(powerups[randomPowerup], location, Quaternion.identity);

            yield return new WaitForSeconds(randomSecs);
        }
        Debug.Log("StopSpawning set to true");
    }

    IEnumerator SpawnRarePowerupRoutine()
    {
        while (_stopSpawning == false)
        {
            int randomNum = (Random.Range(15, 27));
            yield return new WaitForSeconds(randomNum);

            float randomX = Random.Range(-10f, 10f);
            Vector3 location = new Vector3(randomX, 10.0f, 0f);

            Instantiate(powerups[5], location, Quaternion.identity);
        }
        Debug.Log("StopSpawning set to true");
    }

    public void OnPlayerDeath()
    {
        Debug.Log("StopSpawning set to true");
        _stopSpawning = true;
        _uiManager.GameOverActions();
        
    }
}
