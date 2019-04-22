using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    public GameObject birdPrefab;
    public float T;
    float timer;

    public float timeLowerbound = 1.0f;
    public float timeUpperbound = 2.0f;

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
            for (int i = 0; i < bird.transform.childCount; i++)
	        {
	            var child = bird.transform.GetChild(i).gameObject;
	            if(child.name == "Trail")
	            {
	            	child.GetComponent<TrailRenderer>().time = Random.Range(timeLowerbound, timeUpperbound);
	            	break;
	            }
	        }
            
            float time = Time.time - timer;
            float life = 1 - Mathf.Exp(-time / T);
            bird.GetComponent<Bird>().life = life;
        }
        
    }
}
