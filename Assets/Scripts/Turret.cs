using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _homingRocket;
    [SerializeField] private GameObject _explosionPrefab;
    private AudioManager _audio;
    private Vector3 _rocketOffset;
    GameObject go;

    void Start()
    {
        _audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audio == null)
        {
            Debug.LogError("Turret::AudioManager is null");
        }
        _rocketOffset = new Vector3(0, -1.05f, 0);
        StartCoroutine(ShootRockets());
    }


    private void LookAtPlayer()
    {
        //when player is to the -1
        //turn left
        //when player is to the 1
        //turn right
    }

    IEnumerator ShootRockets()
    {
        int x = Random.Range(9, 15);

        while (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(x);
             go = Instantiate(_homingRocket, transform.position + _rocketOffset, Quaternion.identity);
            go.transform.parent = GameObject.Find("EnemyLaser").transform;
        }
    }

    private void TakeDamage()
    {
        //turret takes 3 hits, when hit,
        //attack with asteroid shake
        //minus one hit
        //when hits is zero, 
        //destroy turret
        //instantiate explosion
        //play explosion fx
    }

}
