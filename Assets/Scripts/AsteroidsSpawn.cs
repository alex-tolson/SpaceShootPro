using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidsSpawn : MonoBehaviour
{
    [SerializeField] private Asteroid _asteroid;

    [SerializeField] private float _shakeTime;
    [SerializeField] private Vector3 _newPosition;
    [SerializeField] private float _newSize;
    private bool _asteroidAttack;


    // Start is called before the first frame update
    void Start()
    {
        _shakeTime = 9f;
        StartCoroutine(SpaceShakeCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpaceShakeCoroutine()
    {
        _asteroidAttack = true;
        for (int i = 0; i < _shakeTime; i++)
        {
            _newSize = Random.Range(.5f, 1.5f);
            _newPosition = new Vector3(Random.Range(-10, 11), 7, 0);
            GameObject go = Instantiate(_asteroid.gameObject, _newPosition, Quaternion.identity);
            go.transform.localScale = new Vector3(_newSize, _newSize, _newSize);
            _asteroid.AsteroidFall();
            yield return new WaitForSeconds(1f);
        }
        _asteroidAttack = false;
    }

    public bool IsAsteroidSpawnActive()
    {
        return _asteroidAttack;
    }
    //instantiate many prefabs of the asteroid

    //every 1-2 seconds, instantiate a new asteroid
    //random on the x, 7 on the y

    //instantiate different sizes
    //satellites fall downward and damage player/shield upon contact
    //lasers destroy asteroids
    //some have smoke

    public void UseSpaceShake()
    {
        //anim.change attacking to true
        _asteroidAttack = true;
        //StartCoroutine(SpaceShakeCoroutine())
        //anim.cange attackingto false;
    }
}

