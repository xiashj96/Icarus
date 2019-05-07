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
    public float s1Duration = 5f;
    public float s2Duration = 32F;
    public float s3Duration = 20F;
    public float s4Duration = 60F;
    //public float state5Duration = 60F;

    public float s1Progress = 0F, s1SmoothProgress = 0F;
    public float s6Progress = 0F;

    public float s5FakeAliveTime = 10F;

    AudioSource AS;
    BirdManager BM;
    LineManager LM;
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
            s1SmoothProgress = 0F;
            AS.Play();
            SC.StartAllCoroutine();
            SPC.StartAllCoroutine();
            BC.StartAllCoroutine();
            LM.StartCoroutine(LM.MoveTo(LM.generatePossibility2, s1Duration));
            BM.StartCoroutine(BM.State1Coroutine());
            for(s1Progress = 0F; s1SmoothProgress < 1; s1Progress += Time.deltaTime / 240F)
            {
                s1SmoothProgress += Time.deltaTime / 240F;
                s1SmoothProgress += (s1Progress - s1SmoothProgress) * Time.deltaTime;
                yield return 0;
            }

            state = 0;
            yield return new WaitForSeconds(state0Duration);

            state = 2;
            LM.StartCoroutine(LM.MoveTo(LM.generatePossibility3, s2Duration));
            Coroutine c = BM.StartCoroutine(BM.State2Coroutine());
            yield return new WaitForSeconds(s2Duration);
            BM.flicking = false;
            BM.StopCoroutine(c);

            if(BM.numOfBirds <= 20)
            {
                state = 4;
                BM.RearrangeLifeOfBirds();
                BM.maxDroppingRate = s4Duration - 25;
                SC3.StartAllCoroutine(s4Duration - 10);
                SPC3.StartAllCoroutine(s4Duration - 10);
                BC3.StartAllCoroutine(s4Duration - 10);
                LM.StartCoroutine(LM.MoveTo(0, s4Duration - 20));
                yield return new WaitForSeconds(s4Duration - 12);

                StartCoroutine(AudioFadeOut(10f));
                yield return new WaitForSeconds(10f);

                SR.StartAllCoroutine(2f);
                yield return new WaitForSeconds(2f);

                LM.Initialize();
                BM.DestroyAllBirds();
            }
            else
            {
                state = 3;
                BM.RearrangeLifeOfBirds();

                if (BM.totLife / BM.numOfBirds < 0.8F)
                    BM.burnDamage = 1.2F / s3Duration;
                else
                    BM.burnDamage = (BM.BirdList[BM.numOfBirds-3].life-0.001F)/ s3Duration;

                SC2.StartAllCoroutine();
                SPC2.StartAllCoroutine(s3Duration);
                BC2.StartAllCoroutine(s3Duration);
                ERC.StartAllCoroutine();
                LM.StartCoroutine(LM.MoveTo(0, s3Duration));
                BM.StartCoroutine(BM.State3Coroutine());
                yield return new WaitForSeconds(s3Duration);

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

                    LM.Initialize();
                    BM.DestroyAllBirds();
                }
                else
                {
                    state = 6;
                    while(true)
                    {
                        if (s6Progress < 1F)
                            s6Progress += 0.1F * Time.deltaTime;
                        yield return 0;
                    }
                }

            }
        }
    }

    IEnumerator AudioFadeOut(float duration)
    {
        float startVolume = AS.volume;
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            AS.volume = startVolume * Mathf.Pow(1 - t / duration, 5);
            yield return 0;
        }
        AS.Stop();
        AS.volume = startVolume;
    }
    
    void Start()
    {
    	GameObject sun = GameObject.Find("Sun");
    	GameObject bg = GameObject.Find("BackgroundSkyAndOcean");
        GameObject camera = GameObject.Find("Main Camera");

        AS = GetComponent<AudioSource>();
        BM = GetComponent<BirdManager>();
        LM = GetComponent<LineManager>();
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
}
