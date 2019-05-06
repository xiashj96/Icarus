using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public GameObject birdPrefab;
    public Material bluelineMaterial;
    public bool map; // if set to true, map the whole input range to blueline
    public float yRatio; // compression ration of y
    public bool randomLife;
    public float T;
    float timer;
    

    GameSystem GS;
    LineManager LM;
    //ParticleSystem PS;

    Animator handCreateAnimator;

    public float smoothing;
    Vector2 oldPos;
    bool buttonIsDown;

    GenerateHandsInCircle generateHands;

    void Start()
    {
        GS = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSystem>();
        LM = GameObject.FindGameObjectWithTag("GameController").GetComponent<LineManager>();
        generateHands = GetComponentInChildren<GenerateHandsInCircle>();
        handCreateAnimator = GetComponentInChildren<Animator>();
        buttonIsDown = false;
        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mapPos;
        if (map) // map mouse to area around blueline
        {
            float blueline = bluelineMaterial.GetFloat("_BlueLine");
            float y = blueline * 16 - 8;
            mapPos = Vector2.Scale(mousePos, new Vector2(1, yRatio)) + new Vector2(0, y);
        }
        else
        {
            mapPos = mousePos;
        }

        if (buttonIsDown) // if clicking, apply position smoothing, use smaller t for more smoothing
        {
            Vector2 smoothPos = Vector2.Lerp(oldPos, mapPos, smoothing); 
            transform.position = (Vector2)smoothPos;
        }
        else // no smoothing
        {
            transform.position = mapPos;
        }
        oldPos = transform.position;
        
        if (Input.GetMouseButtonDown(0))
        {
            timer = Time.time;
            buttonIsDown = true;
            handCreateAnimator.SetBool("Play",true);
            generateHands.StartGenerating();
        }
        if (Input.GetMouseButtonUp(0))
        {
            buttonIsDown = false;
            handCreateAnimator.SetBool("Play", false);
            if (GS.state == 1)
            {
                var bird = GameObject.Instantiate(birdPrefab, transform.position, Quaternion.identity).GetComponent<Bird>();
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
            generateHands.StopGenerating();
        }
    }
}
