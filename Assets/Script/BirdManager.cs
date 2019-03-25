using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BirdManager : MonoBehaviour
{
    public HashSet<GameObject> BirdList;
    public bool sort = false; //TODO: state machine
    // Start is called before the first frame update
    public float radiusScale;
    void Start()
    {
        Screen.SetResolution(Screen.height * 9 / 16, Screen.height, Screen.fullScreen);

        BirdList = new HashSet<GameObject>();
        DOTween.To(() => radiusScale, x => radiusScale = x, 0.5f, 2f).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
    }
}
