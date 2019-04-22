using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    BirdManager BM;
    public float life; //[0,1]
    public ParticleSystem particleSystem;
    int numOfBirds = 0;

    private void Start()
    {
        float scale = (life + 1) / 2;
        BM = GameObject.FindGameObjectWithTag("GameController").GetComponent<BirdManager>();
        BM.numOfBirds += 1;
    }

    void Update()
    {
    	if(numOfBirds != BM.numOfBirds)
    	{
    		var emission = particleSystem.emission;
    		emission.rateOverTime = Mathf.Min(BM.particleLimit / BM.numOfBirds / particleSystem.startLifetime, 5f);
    		numOfBirds = BM.numOfBirds;
    	}
    }
}