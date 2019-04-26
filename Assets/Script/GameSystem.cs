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
     * 5: Burnout
     * 6: Reborn
     * 
    */
    public int state = 1;
    public float state0Duration = 3f;
    public float state1Duration = 5f;
    public float state2Duration = 32F;
    public float state3Duration = 20F;
    public float state4Duration = 60F;
    //public float state5Duration = 60F;

    public float state6FakeAliveTime = 10F;

    BirdManager BM;
    SunController SC;
    SunController2 SC2;
    SunController3 SC3;
    SunPositionController SPC;
    SunPositionController2 SPC2;
    SunPositionController3 SPC3;
    SunRecoverer SR;
    BackgroundController BC;
    BackgroundController2 BC2;
    BackgroundController3 BC3;
    EdgeRaysController ERC;
    EdgeRaysController2 ERC2;
    CameraPositionController CPC;

    IEnumerator SetStateCoroutine()
    {
        while(true)
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
                state = 4;
                BM.maxDroppingRate = state4Duration - 25;
                SC3.StartAllCoroutine(state4Duration - 10);
                SPC3.StartAllCoroutine(state4Duration - 10);
                BC3.StartAllCoroutine(state4Duration - 10);
                yield return new WaitForSeconds(state4Duration - 2);
                SR.StartAllCoroutine(2f);
                yield return new WaitForSeconds(2f);

                BM.DestroyAllBirds();
            }
            else
            {
                state = 3;

                if (BM.totLife / BM.numOfBirds < 0.8F)
                    BM.burnDamage = BM.maxLife * 1.2F / state3Duration;
                else
                    BM.burnDamage = BM.maxLife * 0.95F / state3Duration;

                SC2.StartAllCoroutine();
                SPC2.StartAllCoroutine(state3Duration);
                BC2.StartAllCoroutine(state3Duration);
                ERC.StartAllCoroutine();
                BM.StartCoroutine(BM.State3Coroutine());
                yield return new WaitForSeconds(state3Duration);

                if (BM.birdsAliveCnt == 0)
                {
                    state = 5;
                    while(!BM.lastFalling)
                        yield return 0;
                    yield return new WaitForSeconds(15);

                    CPC.StartAllCoroutine(63);
                    yield return new WaitForSeconds(63);
                    
                    SC.Initialize();
                    SPC.Initialize();
                    BC.Initialize();
                    ERC.Initialize();

                    CPC.StartAllCoroutine2(5);
                    yield return new WaitForSeconds(5);

                    BM.DestroyAllBirds();
                }
                else
                {
                    state = 6;
                    while(true) yield return 0;
                }

            }
        }
    }
    
    void Start()
    {
    	GameObject sun = GameObject.Find("Sun");
    	GameObject bg = GameObject.Find("BackgroundSkyAndOcean");
        GameObject camera = GameObject.Find("Main Camera");

        BM = GetComponent<BirdManager>();
        SC = sun.GetComponent<SunController>();
        SC2 = sun.GetComponent<SunController2>();
        SC3 = sun.GetComponent<SunController3>();
        SPC = sun.GetComponent<SunPositionController>();
        SPC2 = sun.GetComponent<SunPositionController2>();
        SPC3 = sun.GetComponent<SunPositionController3>();
        SR = sun.GetComponent<SunRecoverer>();
        BC = bg.GetComponent<BackgroundController>();
        BC2 = bg.GetComponent<BackgroundController2>();
        BC3 = bg.GetComponent<BackgroundController3>();
        ERC = GetComponent<EdgeRaysController>();
        ERC2 = GetComponent<EdgeRaysController2>();
        CPC = camera.GetComponent<CameraPositionController>();
        StartCoroutine(SetStateCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
