using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInfo : MonoBehaviour
{
    Text text;
    BirdManager BM;
    private void Start()
    {
        text = GetComponent<Text>();
        BM = GameObject.FindGameObjectWithTag("GameController").GetComponent<BirdManager>();
        text.text = "";
    }
    // Update is called once per frame
    void Update()
    {
        text.text = "number of birds:" + BM.numOfBirds.ToString()+ "  Time:" +Time.time.ToString("0.#");
    }
}
