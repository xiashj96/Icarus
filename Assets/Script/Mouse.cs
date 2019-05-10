using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public GameObject birdPrefab;
    public Material bluelineMaterial;
    
    public bool randomLife;
    public bool map;
    public float T;
    public bool s2Generate = false;
    public bool s3Generate = false;
    float timer;

    GameSystem GS;
    BirdManager BM;
    LineManager LM;

    Animator handCreateAnimator;

    public float smoothing;
    Vector2 oldPos;
    bool buttonIsDown;

    GenerateHandsInCircle generateHands;

    public Material reflectionMaterial;
    GenerateLight generateLight;

    void Start()
    {
        GS = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSystem>();
        BM = GameObject.FindGameObjectWithTag("GameController").GetComponent<BirdManager>();
        LM = GameObject.FindGameObjectWithTag("GameController").GetComponent<LineManager>();
        generateHands = GetComponentInChildren<GenerateHandsInCircle>();
        handCreateAnimator = GetComponentInChildren<Animator>();
        buttonIsDown = false;
        oldPos = transform.position;
        generateLight = GameObject.Find("LightManager").GetComponent<GenerateLight>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 handPosNormalized = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        Vector2 mapPos;
        if (map)  // map mouse position around blueline
        {
            float blueline = bluelineMaterial.GetFloat("_BlueLine2");
            float width = 9;
            float height = blueline * 5;
            float x = -4.5f;
            float y = blueline * 16 - 8 - height/2;
            //float xRatio = Screen.height * 9f / 16f / Screen.width;
            
    mapPos = Vector2.Scale(handPosNormalized, new Vector2(width, height)) + new Vector2(x, y);
        }
        else
        {
            mapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (buttonIsDown) // if clicking, apply position smoothing, use smaller t for more smoothing
        {
            Vector2 smoothPos = Vector2.Lerp(oldPos, mapPos, smoothing); 
            transform.position = smoothPos;
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
            generateHands.StopGenerating();
            if(BM.numOfBirds >= 400)
                return;
            if (GS.state == 1)
            {
                var bird = Instantiate(birdPrefab, transform.position, Quaternion.identity).GetComponent<Bird>();
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
                generateLight.Generate(bird.life, transform.position);
            }
            else if (s2Generate)
            {
                var bird = Instantiate(birdPrefab, transform.position, Quaternion.identity).GetComponent<Bird>();
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
                bird.lifeIndex = Random.Range(0, 3);
                generateLight.Generate(bird.life, transform.position);
            }
            else if (s3Generate)
            {
                var bird = Instantiate(birdPrefab, transform.position, Quaternion.identity).GetComponent<Bird>();
                bird.life = 0.015F;
                bird.Burst();
                generateLight.Generate(bird.life, transform.position);
            }
        }
    }
}