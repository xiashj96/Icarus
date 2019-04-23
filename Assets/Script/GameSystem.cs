using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
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

        state = 0;
        yield return new WaitForSeconds(state0Duration);

        state = 3;
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
