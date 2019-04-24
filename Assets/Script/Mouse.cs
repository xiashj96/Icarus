using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public GameObject birdPrefab;
    public float T;
    float timer;
    //public float timeLowerbound = 1.0f;
    //public float timeUpperbound = 2.0f;

    GameSystem GS;
    LineManager LM;

    void Start()
    {
        GS = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSystem>();
        LM = GameObject.FindGameObjectWithTag("GameController").GetComponent<LineManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;

        if (Input.GetMouseButtonDown(0))
        {
            timer = Time.time;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(GS.state == 1)
            {
                var bird = GameObject.Instantiate(birdPrefab, mousePos, Quaternion.identity).GetComponent<Bird>();
                float time = Time.time - timer;
                bird.life = 1 - Mathf.Exp(-time / T);
            }
            else if(GS.state == 3)
            {
                LM.Generate(mousePos);
            }
        }
    }
}
