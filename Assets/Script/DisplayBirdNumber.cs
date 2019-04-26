using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBirdNumber : MonoBehaviour
{
    Text text;
    BirdManager BM;
    bool display = false;
    private void Start()
    {
        text = GetComponent<Text>();
        BM = GameObject.FindGameObjectWithTag("GameController").GetComponent<BirdManager>();
        text.text = "";
    }
    // Update is called once per frame
    void Update()
    {
        if(display)
            text.text = "number of birds:" + BM.numOfBirds.ToString()+ "  Time:" +Time.time.ToString("0.#");
        else
            text.text = "";
        if(Input.GetKeyDown(KeyCode.H))
            display = !display;
    }
}
