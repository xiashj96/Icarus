using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public float generatePossibility = 0.1f;
    public float radius = 6.3f;

	GameObject sun;
	public GameObject line1;
    public GameObject line2;

    void Start()
    {
        sun = GameObject.FindGameObjectWithTag("Sun");
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(Random.Range(0f, 1f) >= generatePossibility)
        	return;

        Vector3 sunPos = sun.transform.position;
        GameObject lineObj = Random.Range(0, 2) == 0 ? Instantiate(line1) : Instantiate(line2);
       	lineObj.transform.position = sunPos + Vector3.down * radius;
       	lineObj.transform.RotateAround(sunPos, Vector3.forward, Random.Range(0f, 360f));
       	GameObject.Destroy(lineObj, 1.125f);
    }
}
