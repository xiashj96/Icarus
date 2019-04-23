using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bird : MonoBehaviour
{
    float targetRadius;
    public int id = -1;
    [Header("Kinematic")]
    public float normForce = 6F;
    public float tanForce = 6F;
    public float diffComp = 0.3F;
    public float integComp = 0.3F;
    float integalDist = 0F;
    float individualRadiusRate = 0F;

    [Header("Position")]
    public float theta;
    public float radius;

    [Header("Life")]
    public float life;
    
    public GameObject sun;
    Vector2 sunPosition;
    Rigidbody2D rb2d;
    BirdManager BM;
    GameSystem GS;

    public ParticleSystem particleSystem;
    int numOfBirds = 0;

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

        sun = GameObject.FindGameObjectWithTag("Sun");
        rb2d.velocity = new Vector2(Random.Range(-1F, 1F), 1F); // random initial velocity

        StartCoroutine(ChangeRadiusCoroutine());
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
        return tangent * tanForce * BM.velocityRate;
    }

    Vector2 GetNormalForce()
    {
        targetRadius = BM.GetRadius(id,individualRadiusRate);
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
        if (GS.state == 2)
            ret *= 2F;
        return ret;
    }

    void Update()
    {
    	if(numOfBirds != BM.numOfBirds)
    	{
    		var emission = particleSystem.emission;
    		emission.rateOverTime = Mathf.Min((float)BM.particleLimit / BM.numOfBirds / particleSystem.startLifetime, 5f);
    		numOfBirds = BM.numOfBirds;
    	}
    }
}