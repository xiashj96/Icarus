using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bird : MonoBehaviour
{
    BirdManager BM;

    public bool changingRadius;
    public float minRadius;
    public float maxRadius;
    public float period;
    float targetRadius;

    // 切向力，主要影响角速度
    [Header("Tangent Force")]
    public float minTangentForce;
    public float maxTangentForce;
    float tangentForce;
    //public float tangentNoiseMagnitude;

    //法向力， 由目标半径和到太阳的距离决定
    [Header("Normal Force")]
    public float normalForceScale;
    public float comp = 1f; // set higher to converge more quickly
    //float integalDist = 0;
    //public float normalNoiseMagnitude;

    public float life = 10f;
    //bool innerLoop = false;
    float randTime;
    Vector2 sunPosition;
    Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        // register to bird manager
        BM = GameObject.Find("BirdManager").GetComponent<BirdManager>();
        BM.BirdList.Add(gameObject);

        rb2d = GetComponent<Rigidbody2D>();
        sunPosition = GameObject.FindGameObjectWithTag("Sun").transform.position;
        rb2d.velocity = new Vector2(Random.Range(-2f, 2f), 2f); // random initial velocity
        tangentForce = Random.Range(minTangentForce, maxTangentForce);

        // change target radius periodically
        targetRadius = minRadius;
        if (changingRadius)
        {
            DOTween.To(() => targetRadius, x => targetRadius = x, maxRadius, period).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            targetRadius = Random.Range(minRadius, maxRadius);
        }
    }

    void FixedUpdate()
    {
        rb2d.AddForce(GetTangentForce());
        rb2d.AddForce(GetNormalForce());
    }

    Vector2 GetTangentForce()
    {
        Vector2 r = (Vector2)transform.position - sunPosition;
        Vector2 tangent = new Vector2(-r.y, r.x).normalized;
        return tangent * tangentForce;
    }

    Vector2 GetNormalForce()
    {
        Vector2 r = (Vector2)transform.position - sunPosition;
        Vector2 normal = r.normalized;
        float distance = r.magnitude;
        float normV = Vector2.Dot(rb2d.velocity, normal);
        //integalDist += (distance - targetRadius) * Time.fixedDeltaTime;

        Vector2 ret =  -normal * (Mathf.Clamp(distance - targetRadius, -0.2f, 0.2f) * normalForceScale
            + Mathf.Clamp(normV, -1f, 1f) * comp
            //+ integalDist * comp
            );

        return ret;
    }
}
