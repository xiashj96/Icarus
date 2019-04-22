using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public int state = 1;
    BirdManager BM;
    IEnumerator SetStateCoroutine()
    {
        yield return new WaitForSeconds(20F);
        state = 2;
        BM.StartCoroutine(BM.SPCoroutine());
    }

    // Start is called before the first frame update
    void Start()
    {
        BM = GetComponent<BirdManager>();
        StartCoroutine(SetStateCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
