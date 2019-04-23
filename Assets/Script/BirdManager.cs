using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BirdManager : MonoBehaviour
{
    public int numOfBirds = 0;
    public int particleLimit = 100;

    public float basicRadius = 3F;

    public float state1StartRadiusRate = 0.4F;
    float state1RadiusRate = 0.4F;

    public float state2BasicRadiusRate = 2F;
    float state2RadiusRate = 1F;

    public bool flicking = false;

    public float velocityRate = 1F;
    public GameSystem GS;
    public HashSet<Bird> BirdList = new HashSet<Bird>();
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

    float flickTime = 0.4F, barTime = 4F;

    public IEnumerator State2Coroutine()
    {
        ind[0] = ind[1] = ind[2] = 0;
        state2RadiusRate = 1;
        yield return new WaitForSeconds(5F);
        while (true)
        {
            yield return new WaitForSeconds(barTime - flickTime * 0.5F);
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
        }
    }
    
    //public bool sort = false; //TODO: state machine
    // Start is called before the first frame update
    public float GetRadius(int id, float iRate)
    {
        switch(GS.state)
        {
            case 1:
                return basicRadius * (state1RadiusRate + iRate * 0.2F);
            case 2:
                return basicRadius * (state2RadiusRate * ringRate[ind[id % 3]] + iRate * 0.06F);
        }
        return basicRadius;

    }
    
    private void Start()
    {
        GS = GetComponent<GameSystem>();
        Screen.SetResolution(Screen.height * 9 / 16, Screen.height, Screen.fullScreen);
    }
    
}