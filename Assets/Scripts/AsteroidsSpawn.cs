using System.Collections;
using TMPro;
using UnityEngine;

public class AsteroidsSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _asteroid;
    private Animator _anim;
    private SpawnManager _spawnManager;
    [SerializeField] private float _shakeTime;
    [SerializeField] private Vector3 _newPosition;
    [SerializeField] private float _newSize;
    private bool _asteroidAttack;


    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("BigBoss::Animator is null");
        }
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("AsteroidSpawn::SpawnManager is null");
        }
        _shakeTime = 9f;
 
    }

    private void Update()
    {

    }

    public bool IsAsteroidSpawnActive()
    {
        return _asteroidAttack;
    }
    //Instantiate many prefabs of the asteroid
    //every 1-2 seconds, instantiate a new asteroid
    //random on the x, 7 on the y

    //instantiate different sizes
    //satellites fall downward and damage player/shield upon contact
    //lasers destroy asteroids
    //some have smoke
    IEnumerator ShakeSpaceCoroutine()
    {
        yield return new WaitForSeconds(.24f);
        _anim.SetBool("Attacking", false);
        for (int i = 0; i < _shakeTime; i++)
        {
            _newSize = Random.Range(.5f, 1.5f);
            _newPosition = new Vector3(Random.Range(-10, 11), 7, 0);
            GameObject go = Instantiate(_asteroid.gameObject, _newPosition, Quaternion.identity);
            go.transform.localScale = new Vector3(_newSize, _newSize, _newSize);
            yield return new WaitForSeconds(1f);
        }
        _asteroidAttack = false;
    }

    public void UseAsteroidAttack()
    {
        _asteroidAttack = true;
        StartCoroutine(ShakeSpaceCoroutine());
        
    }
}

