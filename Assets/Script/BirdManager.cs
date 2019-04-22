using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BirdManager : MonoBehaviour
{
    public int numOfBirds = 0;
    public int particleLimit = 100;
    //public bool sort = false; //TODO: state machine
    // Start is called before the first frame update
    
    private void Start()
    {
        Screen.SetResolution(Screen.height * 9 / 16, Screen.height, Screen.fullScreen);
    }
    
}