using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleController : MonoBehaviour
{
	public float scale = 2.0f;
	bool skip = false;

    void Update()
    {
    	if(Input.GetKeyDown(KeyCode.Space))
    	{
    		if(skip = !skip)
    		{
    			Time.timeScale = scale;
    			GetComponent<AudioSource>().pitch = scale;
    		}
    		else
    		{
    			Time.timeScale = 1;
    			GetComponent<AudioSource>().pitch = 1;
    		}
    	}
    }
}
