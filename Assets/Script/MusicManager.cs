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
    public float timePerBar = 6f; // sync transition to bars
    public float currentStateTime;
    GameSystem system;
    AudioSource source;
    Coroutine currentCoroutine;
    int currentState;

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
        currentState = system.state;
        currentCoroutine = StartCoroutine(StartNewState(system.state));
    }

    void Update()
    {
        currentStateTime = source.time;
    }

    IEnumerator SyncMusic()
    {
        yield return null;
        while (true)
        {
            yield return new WaitForSeconds(timePerBar);
            CheckState();
        }
    }

    void CheckState()
    {
        float time = source.time;
        if (currentState != system.state ) 
            //&& time > states[currentState - 1].transitionStartTime)
            //|| timePassed > currentState.transitionEndTime)
        {
            FadeCurrentState();
            StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(StartNewState(system.state));
        }
    }

    IEnumerator StartNewState(int state)
    {
        source = gameObject.AddComponent<AudioSource>();
        source.clip = states[state - 1].clip;
        source.Play();
        currentState = state;
        while (true)
        {
            yield return new WaitForSeconds(timePerBar);
            CheckState();
        }
    }

    void FadeCurrentState()
    {
        var oldSource = source;
        oldSource.DOFade(0, states[currentState - 1].fadeTime).onComplete +=
            () =>{
                Destroy(oldSource);
            };
    }
}
