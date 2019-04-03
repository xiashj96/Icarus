using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBirdNumber : MonoBehaviour
{
    Text text;
    BirdManager BM;
    private void Start()
    {
        text = GetComponent<Text>();
        BM = GameObject.FindGameObjectWithTag("GameController").GetComponent<BirdManager>();
    }
    // Update is called once per frame
    void Update()
    {
        text.text = "number of birds:" + BM.numOfBirds.ToString();
    }
}
