using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    public GameObject birdPrefab;
    public float T;
    float timer;

    // Update is called once per frame
    void Update()
    {

        //Touch[] touches;
        if (Input.GetMouseButtonDown(0))
        {
            timer = Time.time;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            var bird = Instantiate(birdPrefab, mousePos, Quaternion.identity);
            
            float time = Time.time - timer;
            float life = 1 - Mathf.Exp(-time / T);
            bird.GetComponent<Bird>().life = life;
        }
        
    }
}
