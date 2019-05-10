using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInfo : MonoBehaviour
{
    Text text;
    BirdManager BM;
    MusicManager MM;
    private void Start()
    {
        text = GetComponent<Text>();
        BM = GameObject.FindGameObjectWithTag("GameController").GetComponent<BirdManager>();
        MM = GameObject.Find("Music").GetComponent<MusicManager>();
        text.text = "";
    }
    // Update is called once per frame
    void Update()
    {
        text.text = "number of birds:" + BM.numOfBirds.ToString()
            + "  Time:" + Time.time.ToString("0.#")
            + "  State Time: " + MM.currentStateTime.ToString()
            + "  Resolution: " + Screen.width.ToString() + "x" + Screen.height.ToString();
    }
}
