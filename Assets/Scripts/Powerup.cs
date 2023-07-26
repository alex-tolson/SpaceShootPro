using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    private Player player;
    [SerializeField] private int powerupID; // 0 = Triple Shot -- 1 = Speed -- 2 = Shields
    private AudioManager _audioManager;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        if (player == null)
        {
            Debug.LogError("Powerup::Player is null");
        }

        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if(_audioManager == null)
        {
            Debug.LogError("Powerup::AudioManager is null");
        }

    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (powerupID)
            {
                case 0:
                    {
                        player.TripleShotActive();
                        break;
                    }
                case 1:
                    {
                        player.SpeedBoostActive();
                        break;
                    }
                case 2:
                    {
                        player.ShieldsActive();
                        break;
                    }
                case 3:
                    {
                        player.AmmoPowerup();
                        break;
                    }
                case 4:
                    {
                        player.OneUpPowerup();
                        break;
                    }
                case 5:
                    {
                        player.AmmoJamPickup();
                        break;
                    }
                case 6:
                    {
                        player.BeamsPowerup();
                        break;
                    }

                default:
                    {
                        Debug.Log("Default");
                        break;
                    }
            }

            _audioManager.PlayPowerupFx();
            Destroy(this.gameObject);
        }
    }

}
