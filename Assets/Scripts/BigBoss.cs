using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BigBoss : MonoBehaviour
{
    private AudioManager _audioManager;
    private AsteroidsSpawn _asteroidsSpawn;
    private Animator _anim;
    private AudioSource _backgroundAudioSource;
    private AudioSource _bossbattleAudioSource;
    [SerializeField] private int _bigBossHp = 105;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _smallExplosionPrefab;
    [SerializeField] private GameObject _turret1, _turret2, _turret3;

    void Start()
    {
        _backgroundAudioSource = GameObject.Find("BGMusic").GetComponent<AudioSource>();
        if (_backgroundAudioSource == null)
        {
            Debug.LogError("BigBoss::BGMusic is null");
        }
        _bossbattleAudioSource = GameObject.Find("BossBattle").GetComponent<AudioSource>();
        if (_bossbattleAudioSource == null)
        {
            Debug.LogError("BigBoss::Boss Battle music is null");
        }
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("BigBoss::AudioManager is null");
        }
        _asteroidsSpawn = GetComponent<AsteroidsSpawn>();
        if (_asteroidsSpawn == null)
        {
            Debug.LogError("BigBoss::AsteroidSpawn is null");
        }
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("BigBoss::Animator is null");
        }
        _audioManager.StartFadeOut(_backgroundAudioSource, 3f);
        _audioManager.StartFadeIn(_bossbattleAudioSource, 2f);

        StartCoroutine(BossOrderLayerCo());
    }

    IEnumerator BossOrderLayerCo()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        yield return new WaitForSeconds(4.5f);

        if (sr != null)
        {
            sr.sortingOrder = 1;
        }
    }

    IEnumerator TurretDownedCo()
    {
        yield return new WaitForSeconds(.5f);
        _anim.SetBool("Attacking", true);
        _asteroidsSpawn.UseAsteroidAttack();
        yield return new WaitForSeconds(1.5f);
        _anim.SetBool("Attacking", false);
    }

    public void StartTurretDowned()
    {
        StartCoroutine(TurretDownedCo());
    }

    public void BossTakeDamage()
    {
        _bigBossHp -= 10;

        if (_bigBossHp <= 0 && AllTurretsDestroyed())
        {
            StartCoroutine(ExplodeBigBossCo());
        }
    }

    IEnumerator ExplodeBigBossCo()
    {
        float randX;
        float randY;
        Vector3 pos;
        for (int i = 0; i < 15; i++)
        {
            randX = Random.Range(-4.7f, 4.7f);
            randY = Random.Range(.77f, -1.18f);
            pos = new Vector3(randX, randY, transform.position.z);

            GameObject go = Instantiate(_explosionPrefab, pos, Quaternion.identity);
            _audioManager.PlayExplosionFx();
            Destroy(go, 3f);
            yield return new WaitForSeconds(.25f);
        }

        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser") || other.CompareTag("Homing"))
        {
            BossTakeDamage();
            GameObject go = Instantiate(_smallExplosionPrefab, other.transform.position, Quaternion.identity);
            _audioManager.PlayExplosionFx();
            Destroy(go, 3f);
        }
    }

    private bool AllTurretsDestroyed()
    {
        if (_turret1 != null || _turret2 != null || _turret3 != null)
        {
            if (_turret1.activeInHierarchy || _turret2.activeInHierarchy || _turret3.activeInHierarchy)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
    }
}

