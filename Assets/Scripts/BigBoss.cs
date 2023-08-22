using UnityEngine;

public class BigBoss : MonoBehaviour
{
    private AudioManager _audioManager;
    private AsteroidsSpawn _asteroidsSpawn;
    private Animator _anim;
    private AudioSource _backgroundAudioSource;
    private AudioSource _bossbattleAudioSource;
    // Start is called before the first frame update
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

        _audioManager.StartFadeOut(_backgroundAudioSource, 4f);
        _audioManager.StartFadeIn(_bossbattleAudioSource, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{

        //    //_anim.SetBool("Attacking", true);
        //    //_asteroidsSpawn.UseAsteroidAttack();
        //}
    }



}
