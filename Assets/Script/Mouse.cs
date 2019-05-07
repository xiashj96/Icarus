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
    //ParticleSystem PS;

    public GameObject handCreateAnimation;

    public float smoothing;
    Vector2 oldPos;
    bool buttonIsDown;

    GenerateHandsInCircle generateHands;

    public Material reflectionMaterial;
    GenerateLight generateLight;

    void Start()
    {
        GS = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSystem>();
        LM = GameObject.FindGameObjectWithTag("GameController").GetComponent<LineManager>();
        generateHands = GetComponentInChildren<GenerateHandsInCircle>();
        buttonIsDown = false;
        generateLight = GameObject.Find("LightManager").GetComponent<GenerateLight>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.y = reflectionMaterial.GetFloat("_BlueLine2") * UnityEngine.Screen.height;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        if (buttonIsDown)
        {
            Vector2 smoothPos = Vector2.Lerp(oldPos, mousePos, smoothing); // if clicking, apply position smoothing, use smaller t for more smoothing\
            transform.position = (Vector2)smoothPos;
        }
        else
        {
            transform.position = mousePos;
        }
        oldPos = transform.position;
        
        if (Input.GetMouseButtonDown(0))
        {
            timer = Time.time;
            buttonIsDown = true;
            handCreateAnimation.SetActive(true);
            generateHands.StartGenerating();
        }
        if (Input.GetMouseButtonUp(0))
        {
            buttonIsDown = false;
            handCreateAnimation.SetActive(false);
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
                generateLight.Generate(bird.life, transform.position);
            }
            else if(GS.state == 3)
            {
                LM.Generate(mousePos);
            }
            generateHands.StopGenerating();
        }
    }
}
