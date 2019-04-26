using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public GameObject birdPrefab;
    public bool randomLife;
    public float T;
    float timer;
    //public float timeLowerbound = 1.0f;
    //public float timeUpperbound = 2.0f;

    GameSystem GS;
    LineManager LM;
    ParticleSystem PS;

    void Start()
    {
        GS = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSystem>();
        LM = GameObject.FindGameObjectWithTag("GameController").GetComponent<LineManager>();
        PS = GetComponent<ParticleSystem>();
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
            PS.Play();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(GS.state == 1)
            {
                var bird = GameObject.Instantiate(birdPrefab, mousePos, Quaternion.identity).GetComponent<Bird>();
                float time = Time.time - timer;
                if (randomLife)
                {
                    bird.life = Random.Range(0F, 1F);
                }
                else
                {
                    bird.life = 1 - Mathf.Exp(-time / T);
                }
                if (bird.life < 0.01F)
                    bird.life = 0.01F;
            }
            else if(GS.state == 3)
            {
                LM.Generate(mousePos);
            }
            PS.Stop();
        }
    }
}
