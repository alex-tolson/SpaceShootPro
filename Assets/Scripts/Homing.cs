using UnityEngine;

public class Homing : MonoBehaviour
{
    [SerializeField] private GameObject _closestTarget = null;
    private float _shortestDistance = 50.0f;
    private float _distance;
    [SerializeField] private float _homingSpeed = 2.0f;
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
    }

    private void Update()
    {
        FindTarget();

        if (_closestTarget != null && _closestTarget.activeInHierarchy)
        {
            LookAtTarget();
            transform.position = Vector3.MoveTowards(transform.position, _closestTarget.transform.position, _homingSpeed * Time.deltaTime);
        }
        else
        {
            ExpireHomingRocket();
        }
    }

    public void FindTarget()
    {
        collisions = Physics2D.CircleCastAll(transform.position, 50.0f, Vector3.forward);

        foreach (var obj in collisions)
        {
            if (obj.transform.gameObject != null)
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

                else if (obj.collider.CompareTag("EnemyHoming"))
                {
                    _distance = Vector3.Distance(obj.transform.position, transform.position);
                    if (_distance < _shortestDistance)
                    {
                        _shortestDistance = _distance;
                        _closestTarget = obj.collider.gameObject;
                    }
                }
                else if (obj.collider.CompareTag("Turret"))
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
        else if (other.CompareTag("Turret"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("EnemyHoming"))
        {
            Destroy(gameObject);
        }
    }

    private void ExpireHomingRocket()
    {
        Destroy(gameObject);
    }
}
