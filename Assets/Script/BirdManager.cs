using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BirdManager : MonoBehaviour
{
    public HashSet<Bird> BirdList;
    //public bool sort = false; //TODO: state machine
    // Start is called before the first frame update
    
    private void Awake()
    {
        BirdList = new HashSet<Bird>();
        Screen.SetResolution(Screen.height * 9 / 16, Screen.height, Screen.fullScreen);
    }
    
}