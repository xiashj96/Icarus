using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MusicStage
{
    public AudioClip clip;
    public int transitionStartTime; // in seconds
    public int transitionEndTime;
    public int fadeTime;
}

// if new stage, create a new AudioSource, set clip

public class MusicManager : MonoBehaviour
{
    public MusicStage[] stages;
    GameSystem system;

    /*
     * system.state:
     * 1: 发展
     * 2: 结算
     * 3: 燃烧
     * 4: 落海结局
     * 5: 燃尽结局
     * 6: 轮回结局
     * 
    */

    void Start()
    {
        system = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSystem>();
    }

    void Update()
    {
        
    }
}
