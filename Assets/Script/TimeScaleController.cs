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
                foreach (var source in GetComponents<AudioSource>())
                {
                    source.pitch = scale;
                }
    		}
    		else
    		{
    			Time.timeScale = 1;
                foreach (var source in GetComponents<AudioSource>())
                {
                    source.pitch = 1;
                }
            }
    	}
    }
}
