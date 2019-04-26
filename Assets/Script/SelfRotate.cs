using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class SelfRotate : MonoBehaviour
{
    public bool clockwise;
    public float period;

    // Start is called before the first frame update
    void Start()
    {
        if (clockwise)
        {
            transform.DORotate(new Vector3(0, 0, -360f), period, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }
        else
        {
            transform.DORotate(new Vector3(0, 0, 360f), period, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }
    }

}
