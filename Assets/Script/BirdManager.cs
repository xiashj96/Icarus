using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BirdManager : MonoBehaviour
{
    public int numOfBirds = 0;
    public int birdsAliveCnt = 0;
    public float totLife = 0;
    public float maxLife = 0;
    public int particleLimit = 100;

    public float basicRadius = 3F;
    public float burnDamage = 0F;

    float state0RadiusRate = 0.85F;
    float state1RadiusRate = 0.4F;
    float state2RadiusRate = 1F;
    float state3RadiusRate = 1.4F;
    float state4RadiusRate = 1.3F;

    public float state1StartRadiusRate = 0.4F;
    public bool flicking = false;

    public float velocityRate = 1F;
    public float maxDroppingRate = 50F;
    public GameSystem GS;
    public List<Bird> BirdList = new List<Bird>();

    int[] ind = { 0, 0, 0 };
    public float[] ringRate = { 1.0F, 0.9F, 1.25F, 1.6F };

    public IEnumerator State1Coroutine(float duration)
    {
        state1RadiusRate = state1StartRadiusRate;
    	for(float t = 0; t < duration; t += Time.deltaTime)
    	{
    		state1RadiusRate = state1StartRadiusRate + (1 - state1StartRadiusRate) * t / duration;
    		yield return 0;
    	}
    }

    public IEnumerator State2Coroutine()
    {
        const float flickTime = 0.4F, barTime = 4F;

        ind[0] = ind[1] = ind[2] = 0;
        state2RadiusRate = 1;
        BirdList.Sort((b1, b2) => b1.theta.CompareTo(b2.theta));
        int k = 0;
        foreach(Bird b in BirdList)
        	b.col = (k++) % 3;
        BirdList.Sort((b1, b2) => b1.life.CompareTo(b2.life));
        k = 0;
        foreach (Bird b in BirdList)
            b.lifeIndex = (k++) % 3;
        while (true)
        {
            ind[0] = 2; ind[1] = 1; ind[2] = 3;

            yield return new WaitForSeconds(barTime);
            ind[0] = 3; ind[1] = 1; ind[2] = 2;

            yield return new WaitForSeconds(barTime);
            ind[0] = 1; ind[1] = 2; ind[2] = 3;

            yield return new WaitForSeconds(barTime - flickTime * 0.5F);
            flicking = true;
            yield return new WaitForSeconds(flickTime);
            flicking = false;

            yield return new WaitForSeconds(barTime - flickTime);
            flicking = true;
            yield return new WaitForSeconds(flickTime);
            flicking = false;

            yield return new WaitForSeconds(barTime - flickTime);
            flicking = true;
            yield return new WaitForSeconds(flickTime);
            flicking = false;

            yield return new WaitForSeconds(barTime - flickTime * 0.5F);
        }
    }

    public IEnumerator State3Coroutine()
    {
        const float flickTime = 0.4F;

        flicking = true;
        yield return new WaitForSeconds(flickTime);
        flicking = false;
    }
    
    //public bool sort = false; //TODO: state machine
    // Start is called before the first frame update
    public float GetRadius(int id, float iRate)
    {
        switch(GS.state)
        {
            case 0:
                return basicRadius * (state0RadiusRate + iRate * 0.2F);
            case 1:
                return basicRadius * (state1RadiusRate + iRate * 0.25F);
            case 2:
                return basicRadius * (state2RadiusRate * ringRate[ind[id % 3]] + iRate * 0.06F);
            case 3:
                return basicRadius * (state3RadiusRate + iRate * 0.2F);
            case 4:
                return basicRadius * (state4RadiusRate + iRate * 0.5F);


        }
        return basicRadius;

    }

    public void DestroyAllBirds()
    {
        foreach(Bird b in BirdList)
            GameObject.Destroy(b);
        BirdList.Clear();
        numOfBirds = 0;
        birdsAliveCnt = 0;
        totLife = 0;
        maxLife = 0;
    }
    
    private void Start()
    {
        GS = GetComponent<GameSystem>();
        Screen.SetResolution(Screen.height * 9 / 16, Screen.height, Screen.fullScreen);
    }
    
}