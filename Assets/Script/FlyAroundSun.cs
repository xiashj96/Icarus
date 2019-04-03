using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlyAroundSun : MonoBehaviour
{
    public bool changingRadius;
    public float minRadius;
    public float maxRadius;
    public float period;
    float targetRadius;

    // 切向力主要影响角速度
    [Header("Tangent Force")]
    public float minTangentForce;
    public float maxTangentForce;
    float tangentForce;

    //法向力由目标半径和到太阳的距离决定
    [Header("Normal Force")]
    public float normalForceScale;
    public float comp = 1f; // set higher to converge more quickly
    //float integalDist = 0;
    
    float randTime;
    GameObject sun;
    Vector2 sunPosition;
    Rigidbody2D rb2d;
    BirdManager BM;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        BM = GameObject.FindGameObjectWithTag("GameController").GetComponent<BirdManager>();

        sun = GameObject.FindGameObjectWithTag("Sun");
        rb2d.velocity = new Vector2(Random.Range(-2f, 2f), 2f); // random initial velocity
        tangentForce = Random.Range(minTangentForce, maxTangentForce);

        // the more the birds, the greater the radius
        //maxRadius = minRadius + 0.5f + BM.numOfBirds / 100;

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
        sunPosition = sun.transform.position;
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
