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
    float state1RatdiusRate;

    public float basicRadiusRate = 2F;
    public float velocityRate = 1F;
    public GameSystem GS;
    public HashSet<Bird> BirdList = new HashSet<Bird>();
    int[] ind = { 0, 1, 2 };
    float[] ringRate = { 0.9F, 1.25F, 1.6F };

    public IEnumerator State1Coroutine(float duration)
    {
    	for(float t = 0; t < duration; t += Time.deltaTime)
    	{
    		state1RatdiusRate = state1StartRadiusRate + (1 - state1StartRadiusRate) * t / duration;
    		yield return 0;
    	}
    }

    public IEnumerator State2Coroutine()
    {
        yield return new WaitForSeconds(0.2F);
        while (true)
        {
            yield return new WaitForSeconds(3.8F);
            ind[0] = 1;ind[1] = 0;

            yield return new WaitForSeconds(4F);
            ind[0] = 2; ind[2] = 1;

            yield return new WaitForSeconds(4F);
            ind[0] = 0; ind[1] = 1; ind[2] = 2;

            yield return new WaitForSeconds(3.8F);
            basicRadius *= basicRadiusRate;
            yield return new WaitForSeconds(0.4F);
            basicRadius /= basicRadiusRate;

            yield return new WaitForSeconds(3.6F);
            basicRadius *= basicRadiusRate;
            yield return new WaitForSeconds(0.4F);
            basicRadius /= basicRadiusRate;

            yield return new WaitForSeconds(3.6F);
            basicRadius *= basicRadiusRate;
            yield return new WaitForSeconds(0.4F);
            basicRadius /= basicRadiusRate;
        }
    }
    
    //public bool sort = false; //TODO: state machine
    // Start is called before the first frame update
    public float GetRadius(int id, float iRate)
    {
        switch(GS.state)
        {
            case 1:
                return basicRadius * (state1RatdiusRate + iRate * 0.2F);
            case 2:
                int i = id % 3;
                float r = ringRate[ind[i]];
                return basicRadius * r * (1 + iRate * 0.06F);
        }
        return basicRadius;

    }
    
    private void Start()
    {
        GS = GetComponent<GameSystem>();
        state1RatdiusRate = state1StartRadiusRate;
        Screen.SetResolution(Screen.height * 9 / 16, Screen.height, Screen.fullScreen);
    }
    
}