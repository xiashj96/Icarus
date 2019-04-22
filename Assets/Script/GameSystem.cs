using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public int state = 0;
    public float state1Duration = 5f;

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

        state = 2;
        BM.StartCoroutine(BM.State2Coroutine());
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
