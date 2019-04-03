﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Update is called once per frame
    public float start = -18.9f;
    public float endng = -20.64f;
    public float speed = 0.2f;

    private float delta;

    void Start()
    {
        delta = (endng - start) * speed;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            float y = gameObject.transform.position.y;
            y += delta * Time.deltaTime;
            if(y < endng) y = endng;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);
        }

        if(Input.GetKey(KeyCode.DownArrow))
        {
            float y = gameObject.transform.position.y;
            y -= delta * Time.deltaTime;
            if(y > start) y = start;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);
        }
    }
}
