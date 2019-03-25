using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    public GameObject birdPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0;
            GameObject.Instantiate(birdPrefab, mouse, Quaternion.identity);
            Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition)); 
        }
    }
}
