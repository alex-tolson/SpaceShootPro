using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _explosionPrefab;
    private AudioManager _audio;
    private Vector3 _rocketOffset;
    GameObject go;
    [SerializeField] private GameObject _enemyHoming;
    [SerializeField] private int _rocketReloadTime;
    /*    [SerializeField]*/
    public int _turretHp = 0;
    private SpriteRenderer _sr;
    private Color _color;
    private BigBoss _bigBoss;
    private Vector3 _laserOffset;
    private float _fireRate = 2f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _heatSeekLaserPrefab;
    [SerializeField] private GameObject _smallExplosionPrefab;

    void Start()
    {
        _audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audio == null)
        {
            Debug.LogError("Turret::AudioManager is null");
        }
        _laserOffset = new Vector3(0.0f, -1.75f, 0.0f);
        _rocketOffset = new Vector3(0, -1.05f, 0);
        StartCoroutine(ChangeLayerOrder());
        StartCoroutine(ShootRockets());
        StartCoroutine(FireLasersCO());
        _sr = GetComponent<SpriteRenderer>();
        if (_sr == null)
        {
            Debug.LogError("Turret::Color is null");
        }
        _bigBoss = transform.parent.transform.parent.GetComponent<BigBoss>();
        if (_bigBoss == null)
        {
            Debug.LogError("Turret::BigBoss is null");
        }
    }

    IEnumerator ShootRockets()
    {
        _rocketReloadTime = Random.Range(9, 15);

        while (gameObject.activeInHierarchy)
        {
            _rocketReloadTime = Random.Range(9,15);
            yield return new WaitForSeconds(_rocketReloadTime);
            go = Instantiate(_enemyHoming, transform.position + _rocketOffset, Quaternion.identity);
            go.transform.parent = GameObject.Find("EnemyLaser").transform;
        }
    }

    IEnumerator FireLasersCO()
    {
        yield return new WaitForSeconds(Random.Range(9f, 15f));

        while (gameObject.activeInHierarchy)
        {
            int randomNum = Random.Range(0, 2);
            if (randomNum == 0)
            {
                GameObject laserGO = Instantiate(_heatSeekLaserPrefab, transform.position + _laserOffset, Quaternion.identity);
                laserGO.transform.parent = GameObject.Find("BigBossLaserContainer").transform;
            }
            else
            {
                GameObject laserGO = Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
                laserGO.transform.parent = GameObject.Find("BigBossLaserContainer").transform;
            }
            yield return new WaitForSeconds(_fireRate);
        }
    }

    private void TakeLaserDamage()
    {
        _audio.PlayExplosionFx();
        _turretHp -= 10;
        StartCoroutine(HitTurretCo());
        GameObject go = Instantiate(_smallExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(go, 3f);

        if (_turretHp <= 0)
        {
            gameObject.SetActive(false);
            _bigBoss.StartTurretDowned();
        }

        //turret takes 3 hits,
        //when hit,
        //attack with asteroid shake
        //minus one hit
        //when hits is zero, 
        //destroy turret
        //instantiate explosion
        //play explosion fx
    }

    private void TakeRocketDamage()
    {
        _turretHp -= 15;
        StartCoroutine(HitTurretCo());
        GameObject go = Instantiate(_smallExplosionPrefab, transform.position, Quaternion.identity);
        _audio.PlayExplosionFx();
        Destroy(go, 3f);

        if (_turretHp <= 0)
        {
            gameObject.SetActive(false);
            _bigBoss.StartTurretDowned();
        }
    }

    IEnumerator ChangeLayerOrder()
    {
        yield return new WaitForSeconds(4.6f);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.sortingOrder = 0;
        }
    }

    IEnumerator HitTurretCo()
    {
        yield return new WaitForSeconds(.3f);
        _color = Color.red;
        _sr.color = _color;
        yield return new WaitForSeconds(.3f);
        _color = Color.white;
        _sr.color = _color;
        yield return new WaitForSeconds(.3f);
        _color = Color.red;
        _sr.color = _color;
        yield return new WaitForSeconds(.3f);
        _color = Color.white;
        _sr.color = _color;
        yield return new WaitForSeconds(.3f);
        _color = Color.red;
        _sr.color = _color;
        yield return new WaitForSeconds(.3f);
        _color = Color.white;
        _sr.color = _color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            if (other.gameObject.name != "Beams(Clone)")
            {
                Destroy(other);
                TakeLaserDamage();
            }
        }
        else if (other.CompareTag("Homing"))
        {
            Destroy(other);
            TakeRocketDamage();
        }


    }


}
