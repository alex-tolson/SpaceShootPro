using System.Net.Sockets;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8.0f;

    void Update()
    {
        if (this.transform.parent == null) //No parent = regular laser
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            if (transform.position.y > 8)
            {
                Destroy(this.gameObject);
            }
        }

        else if (this.transform.parent.name == "TripleShot(Clone)") // has a triple shot parent = triple shot
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);

            if (transform.position.y > 8)
            {
                Destroy(this.transform.parent.gameObject);
               
            }
        }
        else if (transform.parent.name == "Enemy(Clone)")
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            if (transform.position.y < -8)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().Damage();
        }

        //// if laser collides with Player

        //if (other.CompareTag("Enemy")) // if laser collides with enemy, if lasers' parent is beams
        //{ 
        //    if (this.transform.parent.name == "Beams(Clone)")
        //    {
        //        //Destroy(this.transform.parent.gameObject); 
        //        //destroy laser
        //    }
        //}
    }
}
