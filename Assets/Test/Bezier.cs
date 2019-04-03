using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bezier : MonoBehaviour
{
	public Transform transforms;
	public float duration = 2;

    // Start is called before the first frame update
    void Start()
    {
    	int num = transforms.childCount;
    	Vector3 []wayPoints = new Vector3[num];
    	for(int i = 0; i < num; i++)
    		wayPoints[i] = transforms.GetChild(i).position;
        transform.DOPath(wayPoints, duration, PathType.CubicBezier, PathMode.Ignore).SetEase(Ease.InSine).Play();
    }

}
