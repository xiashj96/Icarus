using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    /*
     * States:
     * 0: transition
     * 1: Generating
     * 2: Ceremony
     * 3: Burning
     * 4: Dropping to the sea
     * 
    */
    public int state = 1;
    public float state0Duration = 3f;
    public float state1Duration = 5f;
    public float state2Duration = 32F;

    BirdManager BM;
    SunController SC;
    SunPositionController SPC;
    BackgroundController BC;

    IEnumerator SetStateCoroutine()
    {
        state = 1;
        SC.StartAllCoroutine(state1Duration);
        SPC.StartAllCoroutine(state1Duration);
        BC.StartAllCoroutine(state1Duration);
        BM.StartCoroutine(BM.State1Coroutine(state1Duration));
        yield return new WaitForSeconds(state1Duration);

        state = 0;
        yield return new WaitForSeconds(state0Duration);

        state = 2;
        Coroutine c = BM.StartCoroutine(BM.State2Coroutine());
        yield return new WaitForSeconds(state2Duration);
        BM.flicking = false;
        BM.StopCoroutine(c);

        if(BM.numOfBirds <= 20)
        {
            state = 0;
            yield return new WaitForSeconds(state0Duration);
            state = 4;
        }
        else
        {
        }

        // Start is called before the first frame update
        void Start()
    {
        BM = GetComponent<BirdManager>();
        SC = GameObject.Find("Sun").GetComponent<SunController>();
        SPC = GameObject.Find("Sun").GetComponent<SunPositionController>();
        BC = GameObject.Find("BackgroundSkyAndOcean").GetComponent<BackgroundController>();
        StartCoroutine(SetStateCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
