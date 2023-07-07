using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    private float randomXaxis;
    private float randomYaxis;

    // Update is called once per frame
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
}
