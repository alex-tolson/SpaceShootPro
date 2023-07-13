using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8.0f;
    [SerializeField] private CameraShake _camShake;

    private void Start()
    {
        _camShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        if (_camShake == null)
        {
            Debug.LogError("Laser::CameraShake is null");
        }
    }
    void Update()
    {
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
            case "Enemy(Clone)":
                {
                    transform.Translate(Vector3.down * _speed * Time.deltaTime); // has no parent = enemy laser
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
            other.GetComponent<Player>().Damage();
            _camShake.StartCamShake();
        }

    }
}
