using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    private Player player;
    [SerializeField] private int powerupID;
    //ID for Powerups
    //0 = Triple Shot
    //1 = Speed
    //2 = Shields


    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        if (player == null)
        {
            Debug.LogError("Powerup::Player is null");
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
            //if powerupID = 0
            player.TripleShotActive();
            //else if powerupID is 1
            // Debug.Log("Speed Powerup collected");
            //else if powerupID is 2
            // Debug.Log("Shield Powerup collected");
            Destroy(this.gameObject);
        }
    }

}
