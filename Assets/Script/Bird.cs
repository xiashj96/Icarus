using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bird : MonoBehaviour
{
    float targetRadius;
    public int id = -1;
    public int col = -1;
    [Header("Kinematic")]
    public float normForce = 6F;
    public float tanForce = 6F;
    public float diffComp = 0.3F;
    public float integComp = 0.3F;
    float integalDist = 0F;
    float individualRadiusRate = 0F;
    float adjustForce = 0F;

    [Header("Position")]
    public float theta;
    public float radius;

    [Header("Life")]
    public float life = 1;  // when initialized, set life manually
                            // life is related trail time
    public float maxTrailTime;
    TrailRenderer trail;
    
    public GameObject sun;
    Vector2 sunPosition;
    Rigidbody2D rb2d;
    BirdManager BM;
    GameSystem GS;

    ParticleSystem particle;
    int numOfBirds = 0;

    //Vector2 ovalAxleP, ovalAxleQ;
    float ovalAngle = 0F, ovalIntensity = 0F;
    IEnumerator ChangeRadiusCoroutine()
    {
        while(true)
        {
            individualRadiusRate = Random.Range(-1F, 0F) + Random.Range(0F, 1F);
            yield return new WaitForSeconds(Random.Range(1.5F, 5F));
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        BM = GameObject.Find("Manager").GetComponent<BirdManager>();
        GS = GameObject.Find("Manager").GetComponent<GameSystem>();
        BM.numOfBirds += 1;
        id = BM.numOfBirds;
        BM.BirdList.Add(this);

        trail = GetComponentInChildren<TrailRenderer>();
        trail.time = life*maxTrailTime;
        particle = GetComponentInChildren<ParticleSystem>();

        sun = GameObject.FindGameObjectWithTag("Sun");
        rb2d.velocity = new Vector2(Random.Range(-1F, 1F), 1F); // random initial velocity
        StartCoroutine(ChangeRadiusCoroutine());

        //Calc oval axles... used in state 3 (burning)
        /*
        float a = Random.Range(-Mathf.PI, Mathf.PI);
        float l = Mathf.Pow(Random.Range(1F,8F),0.167F);

        ovalAxleP = new Vector2(Mathf.Sin(a), Mathf.Cos(a)) * l;
        ovalAxleQ = new Vector2(-Mathf.Cos(a), Mathf.Sin(a)) * 1;
        */
        ovalAngle = Random.Range(-Mathf.PI, Mathf.PI);
        ovalIntensity = Mathf.Pow(Random.Range(0.6F, 1.4F), 2F);

    }
    
    void FixedUpdate()
    {
        sunPosition = sun.transform.position;
        rb2d.AddForce(GetTangentForce());
        rb2d.AddForce(GetNormalForce());
    }

    /*
    Vector2 OvalCoord(Vector2 v)
    {
        return new Vector2(Vector2.Dot(v, ovalAxleP), Vector2.Dot(v, ovalAxleQ));
    }
    Vector2 RectCoord(Vector2 u)
    {
        return u.x * ovalAxleP + u.y * ovalAxleQ;
    }
    */
    Vector2 GetTangentForce()
    {
        Vector2 r = (Vector2)transform.position - sunPosition;

        Vector2 tangent = new Vector2(-r.y, r.x).normalized;
        tangent *= tanForce * BM.velocityRate;
        if (GS.state == 3)
            tangent *= 1 + ovalIntensity * 0.2F;
        return tangent;
    }

    Vector2 GetNormalForce()
    {
        targetRadius = BM.GetRadius(col == -1 ? id % 3 : col, individualRadiusRate);
        Vector2 r = (Vector2)transform.position - sunPosition;
        theta = Mathf.Atan2(r.x, r.y);
        Vector2 normal = r.normalized;
        radius = r.magnitude;


        float normV = Vector2.Dot(rb2d.velocity, normal);
        integalDist += (radius - targetRadius) * Time.fixedDeltaTime;

        Vector2 ret = -normal * (Mathf.Clamp(radius - targetRadius, -1f, 1f) * 1F * normForce
            + Mathf.Clamp(normV, -1f, 1f) * diffComp
            + Mathf.Clamp(integalDist, -1f, 1f) * integComp
            );
        if (GS.state == 0)
            ret *= 4F;
        if (GS.state == 2)
            ret *= 2F;
        if (GS.state == 3)
            ret *= 2F;
        if (BM.flicking)
            ret += normal * 9F;
        if (GS.state == 3)
        {
            float p = (Mathf.Abs(Mathf.Cos(theta - ovalAngle)) - 0.6F) * ovalIntensity;
            ret += -normal * 10F * p;
        }
        return ret;
    }

    void Update()
    {
    	if(numOfBirds != BM.numOfBirds)
    	{
    		var emission = particle.emission;
    		emission.rateOverTime = Mathf.Min((float)BM.particleLimit / BM.numOfBirds / particle.startLifetime, 5f);
    		numOfBirds = BM.numOfBirds;
        }
    }
}