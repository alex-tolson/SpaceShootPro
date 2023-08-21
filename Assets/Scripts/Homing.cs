using UnityEngine;

public class Homing : MonoBehaviour
{
    [SerializeField] private GameObject _closestTarget = null;
    private float _shortestDistance = 50.0f;
    private float _distance;
    [SerializeField] private float _homingSpeed = 2;
    RaycastHit2D[] collisions;
    private Vector3 _direction;
    [SerializeField] private GameObject _explosionPrefab;
    private AudioManager _audio;

    private void Start()
    {
        _audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if(_audio == null)
        {
            Debug.LogError("Homing::AudioManager is null");
        }
        DestroyRocket();
    }

    private void Update()
    {
        FindTarget();
        
        if (_closestTarget != null)
        {
            LookAtTarget();
            transform.position = Vector3.MoveTowards(transform.position, _closestTarget.transform.position, _homingSpeed * Time.deltaTime);
        }

    }

    public void FindTarget()
    {
         collisions = Physics2D.CircleCastAll( transform.position, 50.0f, Vector3.forward);

        foreach (var obj in collisions)
        {
            if (obj.collider.CompareTag("Enemy"))
            {
                _distance = Vector3.Distance(obj.transform.position, transform.position);
                if (_distance < _shortestDistance)
                {
                    _shortestDistance = _distance;
                    _closestTarget = obj.collider.gameObject;
                }
            }
        }
    }

    private void LookAtTarget()
    {
        _direction = _closestTarget.transform.position - transform.position;
        Quaternion rocketRotation = Quaternion.LookRotation(new Vector3(0,0,1), _direction);
        transform.rotation = rocketRotation;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }

        if (other.transform.parent.name == "PlayerLaserContainer")
        {
            Destroy(gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _audio.PlayExplosionFx();
        }
    }

    private void DestroyRocket()
    {
        Destroy(gameObject, 7.0f);
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _audio.PlayExplosionFx();
    }
}
