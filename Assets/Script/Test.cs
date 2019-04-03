using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            float y = gameObject.transform.position.y;
            y -= 0.37f * Time.deltaTime;
            if(y < -20.78f) y = -20.78f;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);
        }

        if(Input.GetKey(KeyCode.DownArrow))
        {
            float y = gameObject.transform.position.y;
            y += 0.37f * Time.deltaTime;
            if(y > -18.9f) y = -18.9f;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);
        }
    }
}
