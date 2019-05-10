using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleController : MonoBehaviour
{
	public float scale = 2.0f;
	bool skip = false;
    MusicManager musicManager;

    void Start()
    {
        musicManager = GameObject.Find("Music").GetComponent<MusicManager>();
    }

    void Update()
    {
    	if(Input.GetKeyDown(KeyCode.Space))
    	{
    		if(skip = !skip)
    		{
    			Time.timeScale = scale;
                musicManager.SetPitch(scale);
    		}
    		else
    		{
    			Time.timeScale = 1;
                musicManager.SetPitch(1);
            }
    	}
    }
}
