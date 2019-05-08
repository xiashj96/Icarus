using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolution : MonoBehaviour
{
    //public int width; // in pixels
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(Screen.height * 9 / 16, Screen.height, false);
    }
}
