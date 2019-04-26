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

    public GameObject SeaCreate;
    public GameObject SeaHand1;
    public GameObject SeaHand2;
    public GameObject SeaHand3;
    public GameObject SeaHand4;

    Coroutine c1 = null;
    Coroutine c2 = null;

    void Start()
    {
        GS = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSystem>();
        LM = GameObject.FindGameObjectWithTag("GameController").GetComponent<LineManager>();
        PS = GetComponent<ParticleSystem>();
    }

    IEnumerator GenerateSeaCreate()
    {
        while(true)
        {
            GameObject s = GameObject.Instantiate(SeaCreate, transform);
            s.transform.localScale = new Vector3(1f, 1f, 1f) * 0.15f;
            Destroy(s, 1);
            yield return new WaitForSeconds(0.5f);
        }
        
    }

    IEnumerator GenerateSeaHand()
    {
        while(true)
        {
            int tmp = Random.Range(0, 4);
            GameObject s = GameObject.Instantiate(tmp == 0 ? SeaHand1 : (tmp == 1 ? SeaHand2 : (tmp == 2 ? SeaHand3 : SeaHand4)));
            s.transform.position = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 0.3f;
            s.transform.localScale = new Vector3(1f, 1f, 1f) * 0.08f;
            Destroy(s, 1);
            yield return new WaitForSeconds(0.2f);
        }
        
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
            c1 = StartCoroutine(GenerateSeaCreate());
            c2 = StartCoroutine(GenerateSeaHand());
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
            StopCoroutine(c1);
            StopCoroutine(c2);
            PS.Stop();
        }
    }
}
