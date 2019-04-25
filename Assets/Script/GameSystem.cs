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
     * 5: Reborn
     * 6: Burnout
     * 
    */
    public int state = 1;
    public float state0Duration = 3f;
    public float state1Duration = 5f;
    public float state2Duration = 32F;
    public float state3Duration1 = 20F;
    public float state3Duration2 = 2F;
    public float state3Duration3 = 2F;
    public float state6FakeAliveTime = 10F;
    BirdManager BM;
    SunController SC;
    SunController2 SC2;
    SunPositionController SPC;
    SunPositionController2 SPC2;
    BackgroundController BC;
    BackgroundController2 BC2;
    EdgeRaysController ERC;

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
            state = 3;
            if (BM.totLife / BM.numOfBirds < 0.8F)
                BM.burnDamage = BM.maxLife * 1.2F / state3Duration1;
            else
                BM.burnDamage = BM.maxLife * 0.95F / state3Duration1;

            SC2.StartAllCoroutine(state3Duration2);
            SPC2.StartAllCoroutine(state3Duration1);
            BC2.StartAllCoroutine(state3Duration1);
            ERC.StartAllCoroutine(state3Duration3);
            BM.StartCoroutine(BM.State3Coroutine());
            Debug.Log("State 3:" + Time.time.ToString());
            yield return new WaitForSeconds(state3Duration1);

            Debug.Log("AAA");
            if (BM.birdsAliveCnt == 0)
            {
                state = 5;
                Debug.Log("State 5:" + Time.time.ToString());

            }
            else
            {
                Debug.Log("State 6:" + Time.time.ToString());
                state = 6;
            }

        }
        
    }
    
    void Start()
    {
        BM = GetComponent<BirdManager>();
        SC = GameObject.Find("Sun").GetComponent<SunController>();
        SC2 = GameObject.Find("Sun").GetComponent<SunController2>();
        SPC = GameObject.Find("Sun").GetComponent<SunPositionController>();
        SPC2 = GameObject.Find("Sun").GetComponent<SunPositionController2>();
        BC = GameObject.Find("BackgroundSkyAndOcean").GetComponent<BackgroundController>();
        BC2 = GameObject.Find("BackgroundSkyAndOcean").GetComponent<BackgroundController2>();
        ERC = GetComponent<EdgeRaysController>();
        StartCoroutine(SetStateCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
