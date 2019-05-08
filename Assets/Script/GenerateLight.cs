using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLight : MonoBehaviour
{
    public GameObject[] lights;
    public float threshold1 = 0.5f;
    public float threshold2 = 0.8f;

    public void Generate(float life, Vector2 position)
    {
    	var light = life < threshold1 ? lights[0] : (life < threshold2 ? lights[1] : lights[2]);
    	var instant = GameObject.Instantiate(light, position + (Vector2)light.transform.localPosition, Quaternion.identity);
    	instant.SetActive(true);
    	Destroy(instant, 1f);
    }
}
