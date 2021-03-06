﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    /*
     * States:
     * 1: Generating
     * 2: Ceremony
     * 3: Burning
     * 4: Dropping to the sea
     * 5: Burnout
     * 6: Reborn
     * 
    */
    public int state = 1;
    float s1Duration = 5f;
    float s2Duration = 48f;
    float s3Duration = 78f;
    float s4Duration = 60f;
    //public float state5Duration = 60F;

    public float s1Progress = 0F, s1SmoothProgress = 0F;
    public float s6Progress = 0F;

    public float s5FakeAliveTime = 10F;

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
    SeaLightController SLC;
    MusicManager MM;
    SubtitleManager SM;

    IEnumerator SetStateCoroutine()
    {
    	yield return 0;
        while(true)
        {
            MM.targetState = 1;
            state = 1;
            s1SmoothProgress = 0F;
            SC.StartAllCoroutine();
            SPC.StartAllCoroutine();
            BC.StartAllCoroutine();
            LM.StartCoroutine(LM.MoveTo(LM.generatePossibility2));
            BM.StartCoroutine(BM.State1Coroutine());
            for(s1Progress = 0F; s1SmoothProgress < 1; s1Progress += Time.deltaTime / 240F)
            {
                s1SmoothProgress += Time.deltaTime / 240F;
                s1SmoothProgress += (s1Progress - s1SmoothProgress) * Time.deltaTime;
                yield return 0;
            }

            MM.targetState = 2;
            while(MM.currentState != 2)
                yield return 0;
            state = 2;
            LM.StartCoroutine(LM.MoveTo(LM.generatePossibility3, s2Duration));
            Coroutine c = BM.StartCoroutine(BM.State2Coroutine());

            if(BM.numOfBirds <= 20)
            {
                MM.targetState = 4;
                while(MM.currentState != 4)
                    yield return 0;
                GameObject.Find("Input").GetComponent<Mouse>().s2Generate = false;
                yield return new WaitForSeconds(6f);
                BM.flicking = false;
                BM.StopCoroutine(c);
                state = 4;

                BM.RearrangeLifeOfBirds();
                BM.maxDroppingRate = s4Duration - 25;
                SC3.StartAllCoroutine(s4Duration - 10);
                SPC3.StartAllCoroutine(s4Duration - 10);
                BC3.StartAllCoroutine(s4Duration - 10);
                LM.StartCoroutine(LM.MoveTo(0, s4Duration - 20));
                yield return new WaitForSeconds(s4Duration - 12);

                yield return new WaitForSeconds(10f);

                SR.StartAllCoroutine(2f);
                yield return new WaitForSeconds(2f);

                LM.Initialize();
                BM.DestroyAllBirds();
            }
            else
            {
                MM.targetState = 3;
                while(MM.currentState != 3)
                    yield return 0;
                GameObject.Find("Input").GetComponent<Mouse>().s2Generate = false;
                yield return new WaitForSeconds(6f);
                BM.flicking = false;
                BM.StopCoroutine(c);
                state = 3;

                BM.RearrangeLifeOfBirds();

                //if (BM.totLife / BM.numOfBirds < 0.8F)
                    BM.burnDamage = 1.2F / s3Duration;
                //else
                //    BM.burnDamage = (BM.BirdList[BM.numOfBirds - 3].life + BM.BirdList[BM.numOfBirds - 4].life) / 2 / s3Duration;

                SC2.StartAllCoroutine();
                SPC2.StartAllCoroutine(s3Duration);
                BC2.StartAllCoroutine(5);
                SLC.StartCoroutine(SLC.FadeIn(2.5f, 2.5f));
                ERC.StartAllCoroutine();
                BM.StartCoroutine(BM.BurstCoroutine(0.2f));
                LM.StartCoroutine(LM.MoveTo(0, s3Duration));
                BM.StartCoroutine(BM.State3Coroutine());
                yield return new WaitForSeconds(s3Duration * 0.7f);
                GameObject.Find("Input").GetComponent<Mouse>().s3Generate = false;
                yield return new WaitForSeconds(s3Duration * 0.3f);
                var emission = GameObject.Find("dropper").GetComponent<ParticleSystem>().emission;
                emission.rateOverTimeMultiplier = 0;

                if (BM.birdsAliveCnt == 0)
                {
                    MM.targetState = 5;
                    state = 5;

                    while(!BM.lastFalling)
                        yield return 0;
                    yield return new WaitForSeconds(15);

                    SM.StartCoroutine(SM.SubtitleStart());

                    CPC.StartAllCoroutine(63);
                    yield return new WaitForSeconds(63);
                    
                    SC.Initialize();
                    SPC.Initialize();
                    BC.Initialize();
                    ERC.Initialize();
                    SLC.Disappear();

                    CPC.StartAllCoroutine2(5);
                    yield return new WaitForSeconds(5);

                    LM.Initialize();
                    BM.DestroyAllBirds();
                }
                else
                {
                    MM.targetState = 6;
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
    
    void Start()
    {
    	GameObject sun = GameObject.Find("Sun");
    	GameObject bg = GameObject.Find("BackgroundSkyAndOcean");
        GameObject camera = GameObject.Find("Main Camera");
        GameObject seaLight = GameObject.Find("SeaLight");
        GameObject music = GameObject.Find("Music");
        GameObject subtitleManager = GameObject.Find("SubtitleManager");

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
        SLC = seaLight.GetComponent<SeaLightController>();
        MM = music.GetComponent<MusicManager>();
        SM = subtitleManager.GetComponent<SubtitleManager>();
        StartCoroutine(SetStateCoroutine());
    }
}
