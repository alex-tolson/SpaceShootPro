using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(-10f, 10f), 8f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {


        EnemyMov();
        if (transform.position.y < -4f)
        {
            float randomX = Random.Range(-10f, 10f);
            transform.position = new Vector3(randomX, 8f, transform.position.z);
        }
    }

    void EnemyMov()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();

                Destroy(gameObject);
            }
        }


        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

}
