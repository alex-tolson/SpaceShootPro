using System.Collections;
using UnityEngine;

public class AsteroidsSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _asteroid;
    private CameraShake _camShake;
    private Animator _anim;
    private SpawnManager _spawnManager;
    [SerializeField] private float _shakeTime;
    [SerializeField] private Vector3 _newPosition;
    [SerializeField] private float _newSize;


    void Start()
    {
        _camShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        if (_camShake == null)
        {
            Debug.LogError("AsteroidSpawn::CameraShake is null");
        }
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
    }

    public void UseAsteroidAttack()
    {
        StartCoroutine(ShakeSpaceCoroutine());
        _camShake.StartAsteroidFall();
    }
}

