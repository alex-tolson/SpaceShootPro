using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private GameObject _asteroid;
    [SerializeField] private Vector3 _rot;

    [SerializeField] private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Asteroid::SpawnManager is null");
        }
        _rot = new Vector3(0f, 0f, 90f);
    }


    void Update()
    {   
        transform.Rotate(_rot * _speed * Time.deltaTime); 
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            GameObject go = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(go, 3f);
            _spawnManager.StartSpawning();
            Destroy(_asteroid, .3f);

        }
    }

    //check for laser collision (of type trigger)
    //Instantiate explosion at the position of the asteroid (our position)
    //destroy explosion after 3f

}
