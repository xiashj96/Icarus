using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    BirdManager BM;
    public float life; //[0,1]

    private void Start()
    {
        float scale = (life + 1) / 2;
        BM = GameObject.FindGameObjectWithTag("GameController").GetComponent<BirdManager>();
        BM.numOfBirds += 1;
    }
}