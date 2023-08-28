using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    private Player player;
    [SerializeField] private int powerupID; // 0 = Triple Shot -- 1 = Speed -- 2 = Shields
    private AudioManager _audioManager;
    [SerializeField] private GameObject _explosion;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        if (player == null)
        {
            Debug.LogError("Powerup::Player is null");
        }

        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (_audioManager == null)
        {
            Debug.LogError("Powerup::AudioManager is null");
        }

    }

    void Update()
    {
        if (player.IsCPressed() && gameObject.CompareTag("Powerup"))
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, (1 + _speed) * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y < -6f)
            {
                Destroy(this.gameObject);
            }
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
                        _audioManager.PlayPowerupFx();
                        break;
                    }
                case 1:
                    {
                        player.SpeedBoostActive();
                        _audioManager.PlayPowerupFx();
                        break;
                    }
                case 2:
                    {
                        player.ShieldsActive();
                        _audioManager.PlayPowerupFx();
                        break;
                    }
                case 3:
                    {
                        player.AmmoPowerup();
                        _audioManager.PlayReloadChamberFx();
                        break;
                    }
                case 4:
                    {
                        player.OneUpPowerup();
                        _audioManager.PlayPowerupFx();
                        break;
                    }
                case 5:
                    {
                        player.AmmoJamPickup();
                        _audioManager.PlayEmptyChamberFx();
                        break;
                    }
                case 6:
                    {
                        player.BeamsPowerup();
                        _audioManager.PlayPowerupFx();
                        break;
                    }
                case 7:
                    {
                        player.HomingPowerup();
                        _audioManager.PlayPowerupFx();
                        break;
                    }

                default:
                    {
                        break;
                    }
            }


            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Laser"))
        {
            GameObject go = Instantiate(_explosion, transform.position, Quaternion.identity);
            _audioManager.PlayExplosionFx();
            Destroy(gameObject);
            Destroy(go, 2.8f);

        }
    }

}
