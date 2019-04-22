using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BirdManager : MonoBehaviour
{
    public int numOfBirds = 0;
    public float basicRadius = 3F;
    public float velocityRate = 1F;
    public GameSystem GS;
    public HashSet<Bird> BirdList = new HashSet<Bird>();
    int[] ind = { 0, 1, 2 };
    float[] ringRate = { 0.5F, 0.85F, 1.2F };
    public IEnumerator SPCoroutine()
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
            basicRadius *= 2F;
            yield return new WaitForSeconds(0.4F);
            basicRadius /= 2F;

            yield return new WaitForSeconds(3.6F);
            basicRadius *= 2F;
            yield return new WaitForSeconds(0.4F);
            basicRadius /= 2F;

            yield return new WaitForSeconds(3.6F);
            basicRadius *= 2F;
            yield return new WaitForSeconds(0.4F);
            basicRadius /= 2F;
        }
    }
    //public bool sort = false; //TODO: state machine
    // Start is called before the first frame update
    public float GetRadius(int id,float iRate)
    {
        switch(GS.state)
        {
            case 1:
                return basicRadius * (1 + iRate * 0.2F);
            case 2:
                int i = id % 3;
                float r = ringRate[ind[i]];
                return basicRadius * r *(1 + iRate * 0.12F);
        }
        return basicRadius;

    }
    private void Start()
    {
        GS = GetComponent<GameSystem>();
        Screen.SetResolution(Screen.height * 9 / 16, Screen.height, Screen.fullScreen);
    }
    
}