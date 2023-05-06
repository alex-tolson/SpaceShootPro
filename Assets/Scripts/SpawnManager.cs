using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private WaitForSeconds _timeWaiting = new WaitForSeconds(5f);

    [SerializeField] private GameObject _enemyContainer;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnRoutine");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnRoutine()
    {
        Vector3 loc;                                    

        while (_stopSpawning == false)              
        {
            float randX = Random.Range(-10f, 10f); 
            loc = new Vector3(randX, 8, 0);

            GameObject newEnemy = Instantiate(enemyPrefab, loc, Quaternion.identity);

            newEnemy.transform.parent = _enemyContainer.transform;

            yield return _timeWaiting;
        }
    }

  public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }


}
