using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public float generatePossibility1 = 0.5f;
    public float generatePossibility2 = 1f;
    public float generatePossibility3 = 5f;
    public float generatePossibility;
    float radius = 6.3f;
    float space = 2f;

    GameSystem GS;

	GameObject sun;
	public GameObject line1;
    public GameObject line2;

    void Start()
    {
        sun = GameObject.FindGameObjectWithTag("Sun");
        GS = FindObjectOfType<GameSystem>();
        Initialize();
        StartCoroutine(GenerateCoroutine());
    }

    public void Initialize()
    {
        generatePossibility = generatePossibility1;
    }

    public IEnumerator MoveTo(float p)
    {
        float startP = generatePossibility;
        while(GS.state == 1)
        {
            generatePossibility = startP + (p - startP) * GS.s1SmoothProgress;
            yield return 0;
        }
        generatePossibility = p;
    }

    public IEnumerator MoveTo(float p, float duration)
    {
        float startP = generatePossibility;
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            generatePossibility = startP + (p - startP) * t / duration;
            yield return 0;
        }
        generatePossibility = p;
    }

    IEnumerator Generate(float delay, float angle)
    {
        yield return new WaitForSeconds(delay);
        Vector3 sunPos = sun.transform.position;
        GameObject lineObj = Random.Range(0, 2) == 0 ? Instantiate(line1) : Instantiate(line2);
       	lineObj.transform.position = sunPos + Vector3.down * radius;
       	lineObj.transform.RotateAround(sunPos, Vector3.forward, angle);
       	GameObject.Destroy(lineObj, 1.125f);
    }

    IEnumerator GenerateCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(space);
            int upperbound = (int)(space * generatePossibility * 2);
            int cnt = Random.Range(0, upperbound + 1);
            cnt = Mathf.Min(cnt, 6);
            float offset = Random.Range(0f, 360f);
            for(int i = 0; i < cnt; i++)
                StartCoroutine(Generate(Random.Range(0f, 0.3f), offset + i * 360f / cnt + Random.Range(-15f, 15f)));
        }
    }
}
