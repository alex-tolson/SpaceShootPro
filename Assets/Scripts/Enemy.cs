using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    private Animator _animator;
    private Player _player;
    private AudioManager _audioManager;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Enemy::Player null");
        }
        //
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Enemy::Animator null");
        }
        //
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("Asteroid::AudioSource is null");
        }
    }

    void Update()
    {
        EnemyMov();
        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-10f, 10f);
            transform.position = new Vector3(randomX, 8f, transform.position.z);
        }
    }

    void EnemyMov()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (_player != null)
            {
                _player.Damage();                
            }
            _speed = 0;
            _animator.SetTrigger("OnEnemyDeath");
            _audioManager.PlayExplosionFx();
            Destroy(gameObject, 2.5f);
        }
        if (other.CompareTag("Laser"))
        {
            if (_player != null)
            {
                _player.ScoreUpdate(10);
            }
            Destroy(other.gameObject);
            _speed = 0;
            _animator.SetTrigger("OnEnemyDeath");
            _audioManager.PlayExplosionFx();

            Destroy(gameObject, 2.5f);
        }
    }
}
