using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    //BirdManager BM;
    public float life; //[0,1]

    private void Start()
    {
        float scale = (life + 1) / 2;
        //transform.localScale = new Vector3(scale, scale, 1);
    }
}