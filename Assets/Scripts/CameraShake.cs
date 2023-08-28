using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    private float randomXaxis;
    private float randomYaxis;


    void Update()
    {

    }

    IEnumerator ShakeCamCorou()
    {
        for (int i = 0; i < 4; i++)
        {
            randomXaxis = Random.Range(-.21f, .21f);
            randomYaxis = Random.Range(-.50f, .50f);
            transform.position = new Vector3(randomXaxis, randomYaxis, transform.position.z);
            yield return new WaitForSeconds(.05f);
        }
        transform.position = new Vector3(0, 0, -10);
    }

    public void StartCamShake()
    {
        StartCoroutine(ShakeCamCorou());
    }

    IEnumerator AsteroidFall()
    {
        for (int i = 0; i < 30; i++)
        {
            randomXaxis = Random.Range(-.21f, .21f);
            randomYaxis = Random.Range(-.35f, .35f);
            transform.position = new Vector3(randomXaxis, randomYaxis, transform.position.z);
            yield return new WaitForSeconds(.05f);
        }
        transform.position = new Vector3(0, 0, -10);
    }
    public void StartAsteroidFall()
    {
        StartCoroutine(AsteroidFall());
    }
}