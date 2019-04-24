using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public float generatePossibility = 0.1f;
    float radius = 6f;

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

    public void Generate(Vector2 pos)
    {
        Vector3 sunPos = sun.transform.position;
        GameObject lineObj = Random.Range(0, 2) == 0 ? Instantiate(line1) : Instantiate(line2);
        float scale = (pos - (Vector2)sunPos).magnitude / (radius * Mathf.Sqrt(5f));
        lineObj.transform.localScale = new Vector3(scale, scale, 0);
        lineObj.transform.position = sunPos + Vector3.down * radius * scale;
        lineObj.transform.RotateAround(sunPos, Vector3.forward, Mathf.Rad2Deg * (Mathf.Atan2(pos.y - sunPos.y, pos.x - sunPos.x) - Mathf.Atan2(-2f, -1f)) - 1f);
        GameObject.Destroy(lineObj, 1.125f);
    }
}
