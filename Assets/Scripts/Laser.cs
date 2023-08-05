using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8.0f;
    [SerializeField] private CameraShake _camShake;
    private Enemy _enemy;
    private Player _player;
    private float _dist;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null )
        {
            Debug.LogError("Laser::Player is null");
        }
        _camShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        if (_camShake == null)
        {
            Debug.LogError("Laser::CameraShake is null");
        }
    }
    void Update()
    {
        _dist = Vector3.Distance(gameObject.transform.position, _player.transform.position);

        switch (transform.parent.name)
        {
            case "PlayerLaserContainer":        //has PlayerLaserContainer parent = Player's regular laser
                {
                    transform.Translate(Vector3.up * _speed * Time.deltaTime);
                    if (transform.position.y > 8)
                    {
                        Destroy(gameObject);
                    }
                    break;
                }

            case "TripleShot(Clone)":           // has a triple shot parent = triple shot
                {
                    transform.Translate(Vector3.up * _speed * Time.deltaTime);

                    if (transform.position.y > 8)
                    {
                        Destroy(transform.parent.gameObject);
       }
                    break;
                }
            case "EnemyLaser":
                {
                    if (gameObject.name == "HeatSeekLaser(Clone)")
                    {
                        _speed = 2f;

                        if (_dist < 5.0f)
                        {
                            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
                        }
                    }

                    transform.Translate(Vector3.down * _speed * Time.deltaTime);

                    if (transform.position.y < -8)
                    {
                        Destroy(gameObject);
                    }

                    break;
                }

            default:
                {
                    //Debug.Log("Hi Beams");
                    break;
                }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player.Damage();
            _camShake.StartCamShake();
        }
    }
}



