using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public struct MusicState
{
    public AudioClip clip;
    public int transitionStartTime; // in seconds
    public int transitionEndTime;
    public int fadeTime;
}

// if new stage, create a new AudioSource, set clip

public class MusicManager : MonoBehaviour
{
    public MusicState[] states;
    public int timePerBar = 6; // sync transition to bars
    GameSystem system;
    int current;
    float timer;

    /*
     * system.state:
     * 1: 发展
     * 2: 结算
     * 3: 燃烧
     * 4: 落海结局
     * 5: 燃尽结局
     * 6: 轮回结局
    */

    void Start()
    {
        system = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSystem>();
        StartNewState(system.state);
        current = system.state;
        timer = 0;
        StartCoroutine(SyncMusic());
    }

    IEnumerator SyncMusic()
    {
        while (true)
        {
            yield return new WaitForSeconds(timePerBar);
            CheckState();
        }
    }

    void CheckState()
    {
        var currentState = states[current - 1];
        float timePassed = Time.time - timer;
        if ((current != system.state && timePassed > currentState.transitionStartTime)
            || timePassed > currentState.transitionEndTime)
        {
            FadeCurrentState();
            StartNewState(system.state);
            timer = Time.time;
        }
    }

    void StartNewState(int state)
    {
        var source = gameObject.AddComponent<AudioSource>();
        source.clip = states[state - 1].clip;
        source.Play();
        current = state;
    }

    void FadeCurrentState()
    {
        var source = GetComponent<AudioSource>(); // assume there is only one source in the GameObject
        source.DOFade(0, states[current - 1].fadeTime).onComplete +=
            () =>{
                Destroy(source);
            };
    }
}
