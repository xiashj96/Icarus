using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bird : MonoBehaviour
{
    bool alive = true;
    bool fakeAlive = false;
    int droppingState = 0;
    static float lastDeathTime = 0F;
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
    public float life = 1,startingLife = 1;  // when initialized, set life manually
    public float minAlpha;   // max alpha is always 1

    public int lifeIndex = 0;
    float droppingRate = 0F;
    float state4TargetX = 0F;
    public float minTrailTime = 0.5f;
    public float maxTrailTime = 1f;
    TrailRenderer trail;
    
    public GameObject sun;
    Vector2 sunPosition;
    Rigidbody2D rb2d;
    BirdManager BM;
    GameSystem GS;

    ParticleSystem particle;
    ParticleSystem.EmissionModule emission;
    GenerateLight generateLight;
    Animator animator;
    int numOfBirds = 0;
    
    float ovalAngle = 0F, ovalIntensity = 0F;

    bool fadeOut = false;
    float initialAlpha;
    float initialWidth;
    float initialRate;
    public Material reflectionMaterial;

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
        GS.s1Progress += 1F / 800F;
        BM.numOfBirds += 1;
        BM.birdsAliveCnt++;
        BM.totLife += life;
        BM.maxLife = Mathf.Max(BM.maxLife, life);
        id = BM.numOfBirds;
        initialAlpha = minAlpha + (1 - minAlpha) * life;
        GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, initialAlpha); // alpha channel of sprite
        BM.BirdList.Add(this);

        trail = GetComponentInChildren<TrailRenderer>();
        trail.time = minTrailTime + life * (maxTrailTime - minTrailTime);
        particle = GetComponentInChildren<ParticleSystem>();
        emission = particle.emission;
        generateLight = GameObject.Find("LightManager").GetComponent<GenerateLight>();
        animator = GetComponentInChildren<Animator>();

        sun = GameObject.FindGameObjectWithTag("Sun");
        rb2d.velocity = new Vector2(Random.Range(-1F, 1F), 1F); // random initial velocity
        StartCoroutine(ChangeRadiusCoroutine());
        
        ovalAngle = Random.Range(-Mathf.PI, Mathf.PI);
        ovalIntensity = Mathf.Pow(Random.Range(0.6F, 1.4F), 2F);

        state4TargetX = Random.Range(-2F, 2F) + Random.Range(-1F, 1F);

        startingLife = life;
    }

    IEnumerator DieDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        animator.SetBool("Die", true);
    }
    
    void FixedUpdate()
    {
        sunPosition = sun.transform.position;

        if (GS.state == 4)//dropping to the sea
        {
            if (droppingRate <= BM.maxDroppingRate)
            {
                droppingRate += Time.fixedDeltaTime / (0.01F + life);
                if (droppingRate / BM.maxDroppingRate >= 0.8F)
                    rb2d.AddForce(Vector2.up * 3F * Mathf.Pow((droppingRate / BM.maxDroppingRate - 0.8F) * 5F, 3F));
            }
            else
            {
                StartCoroutine(DieDelay(5f));
                droppingRate += Time.fixedDeltaTime;
                rb2d.AddForce(Vector2.down * 0.8F);
            }

            if (droppingRate / BM.maxDroppingRate >= 0.8F && transform.position.y < sunPosition.y)
                rb2d.AddForce(Vector2.right * (state4TargetX - transform.position.x));
        }
        if (!alive && !fakeAlive)
            rb2d.AddForce(Vector2.down * 0.8F);
        if (droppingState > 0)
            rb2d.AddForce(Vector2.left * rb2d.position.x * 0.2F);
        if (droppingState == 2)
            rb2d.AddForce(Vector2.up * 3F);

        rb2d.AddForce(GetTangentForce());
        rb2d.AddForce(GetNormalForce());
        if(GS.state == 6 && alive)
            rb2d.AddForce(GetS6Force());

    }

    float s6randX = -1F,s6randY = -1F,s6RandS = -1F;
    Vector2 diffDeltaPos = Vector2.zero,deltaPos = Vector2.zero,
        lastFrameDeltaPos = Vector2.zero, targetPos = Vector2.zero;
    Vector2 GetS6Force()
    {
        if(s6randX <0)
        {
            s6randX = Random.Range(0F, 2F * Mathf.PI);
            s6randY = Random.Range(0F, 2F * Mathf.PI);
            s6RandS = Random.Range(0.8F, 1.2F);
        }

        targetPos = sunPosition + Vector2.down * 3.5F +
            new Vector2(0.6F * Mathf.Cos(s6randX + Time.time * 0.8F * s6RandS), 1.2F * Mathf.Cos(s6randY + Time.time * 0.5F * s6RandS));
        deltaPos = targetPos - (Vector2)transform.position;
        diffDeltaPos = (deltaPos - lastFrameDeltaPos) / Time.fixedDeltaTime;
        lastFrameDeltaPos = deltaPos;
        return GS.s6Progress * (4F * deltaPos + 2F * diffDeltaPos);
    }

    Vector2 GetTangentForce()
    {
        if (!alive && !fakeAlive)
            return Vector2.zero;
        Vector2 r = (Vector2)transform.position - sunPosition;

        Vector2 tangent = new Vector2(-r.y, r.x).normalized;
        tangent *= tanForce * BM.velocityRate;
        if (GS.state == 3)
            tangent *= 0.8F + ovalIntensity * 0.2F;
        if(GS.state == 4)
        {
            if (droppingRate / BM.maxDroppingRate >= 0.8F)
                tangent *= 2.5F * Mathf.Max(0F, 1.2F - droppingRate / BM.maxDroppingRate);
        }

        if (GS.state == 6)
            tangent *= 1 - GS.s6Progress;
        return tangent;
    }

    Vector2 GetNormalForce()
    {
        if (!alive && !fakeAlive)
            return Vector2.zero;

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
        if (GS.state == 4)
        {
            if (droppingRate / BM.maxDroppingRate >= 0.8F)
                ret *= 2.5F * Mathf.Max(0F, 1.2F - droppingRate / BM.maxDroppingRate);
            ret *= 2F;
        }
        if (BM.flicking)
            ret += normal * 9F;
        if (GS.state == 3)
        {
            float p = (Mathf.Abs(Mathf.Cos(theta - ovalAngle)) - 0.6F) * ovalIntensity;
            ret += -normal * 10F * p;
        }
        if (GS.state == 6)
            ret *= 1 - GS.s6Progress;
        return ret;
    }

    IEnumerator FakeAliveCoroutine()
    {
        fakeAlive = true;
        yield return new WaitForSeconds(GS.s5FakeAliveTime);
        while(Mathf.Abs(theta) < 0.8*Mathf.PI)
            yield return new WaitForSeconds(0.1F);
        fakeAlive = false;
        BM.lastFalling = true;
        droppingState = 1;
        yield return new WaitForSeconds(2F);
        for (int i = 1; i <= 3; i++)
        {
            yield return new WaitForSeconds(4F);
            droppingState = 2;
            yield return new WaitForSeconds(1.8F - 0.3F * i);
            droppingState = 1;

        }
        StartCoroutine(DieDelay(3f));
    }

    void Update()
    {
    	/*
        if (alive)
            GetComponentInChildren<SpriteRenderer>().color = Color.HSVToRGB(life * 0.6F, 0.8F, 1F);
        else
        {
            if (fakeAlive)
                GetComponentInChildren<SpriteRenderer>().color = new Color(1F, 0.5F, 0.5F);
            else
                GetComponentInChildren<SpriteRenderer>().color = Color.grey;
        }
        */

        if (GS.state == 3)
        {
            life -= BM.burnDamage * Time.deltaTime;
        }
        if (alive && life <= 0F && Time.time - lastDeathTime >= 0.2F)
        {
            lastDeathTime = Time.time; // lastDeathTime is static;
            alive = false;
            BM.birdsAliveCnt--;
            if (BM.birdsAliveCnt == 0)
            {
                StartCoroutine(FakeAliveCoroutine());
            }
            else
            {
                StartCoroutine(DieDelay(2f));
            }
        }
        if (numOfBirds != BM.numOfBirds)
    	{
    		emission.rateOverTime = Mathf.Min((float)BM.particleLimit / BM.numOfBirds / particle.startLifetime, 5f);
    		numOfBirds = BM.numOfBirds;
        }

        if(GS.state >= 3)
        {
        	float screenPos = Camera.main.WorldToScreenPoint(transform.position).y / UnityEngine.Screen.height;
        	float blueLine = reflectionMaterial.GetFloat("_BlueLine");
            float cameraOffset = reflectionMaterial.GetFloat("_CameraOffset");
        	const float buffer = 0.05f;
        	float alpha = Mathf.Max(1 + (screenPos + cameraOffset - blueLine) / buffer, 0);
        	if(alpha >= 1) return;
            if(!fadeOut)
            {
                fadeOut = true;
                generateLight.Generate(startingLife, transform.position);
                initialWidth = GetComponentInChildren<TrailRenderer>().widthMultiplier;
                initialRate = emission.rateOverTime.constant;
            }
            if(GS.state == 5)
            {
                alpha = Mathf.Max(screenPos - 0.05f, 0f);
            }
        	GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, initialAlpha * alpha);
        	GetComponentInChildren<TrailRenderer>().widthMultiplier = initialWidth * (alpha < 0.3f ? 0 : alpha);
        	emission.rateOverTime = initialRate * alpha;
        }
    }

}