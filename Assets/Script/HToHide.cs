using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HToHide : MonoBehaviour
{
	bool display = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 1955, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            if(display = !display)
                transform.position = new Vector3(0, 1920, 0);
            else
                transform.position = new Vector3(0, 1955, 0);
        }
    }
}
