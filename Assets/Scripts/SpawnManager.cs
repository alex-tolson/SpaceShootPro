using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private Coroutine spawnEnemy = null;
    private WaitForSeconds _timeWaiting = new WaitForSeconds(2.0f);

    [SerializeField] private GameObject _enemyContainer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnEnemy == null) 
        {
            spawnEnemy = StartCoroutine("SpawnRoutine");
        }
    }

    IEnumerator SpawnRoutine()
    {
        Vector3 loc;                   
        int count = 10;                 

        while (count > 0)              
        {
            float randX = Random.Range(-10f, 10f); 
            loc = new Vector3(randX, 8, 0);

            GameObject newEnemy = Instantiate(enemyPrefab, loc, Quaternion.identity);

            newEnemy.transform.parent = _enemyContainer.transform;

            yield return _timeWaiting;

            count--; 
        }

        spawnEnemy = null;
    }
}
