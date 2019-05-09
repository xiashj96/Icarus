using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public struct MusicState
{
    public AudioClip clip;
    public int fadeTime;
}

// if new stage, create a new AudioSource, set clip

public class MusicManager : MonoBehaviour
{
    public MusicState[] states;
    public float timePerBar = 6f; // sync transition to bars
    public float currentStateTime;
    GameSystem GS;
    AudioSource source1, source2;
    Coroutine currentCoroutine;
    public int currentState, targetState;

    /*
     * GS.state:
     * 1: 发展
     * 2: 结算
     * 3: 燃烧
     * 4: 落海结局
     * 5: 燃尽结局
     * 6: 轮回结局
    */

    void Start()
    {
        source1 = gameObject.AddComponent<AudioSource>();
        source2 = gameObject.AddComponent<AudioSource>();

        GS = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSystem>();
        currentState = targetState = 1;
        currentCoroutine = StartCoroutine(StartNewState(1));
    }

    void Update()
    {
        currentStateTime = source1.time;
    }

    void CheckState()
    {
        if (currentState != targetState )
        {
            Debug.Log(currentStateTime);
            FadeCurrentState();
            StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(StartNewState(targetState));
        }
    }

    IEnumerator StartNewState(int state)
    {
        source1.clip = states[state - 1].clip;
        source1.volume = 1f;
        source1.Play();
        currentState = state;
        while (true)
        {
            float tmp = currentStateTime / timePerBar;
            if( state == 2 ? (currentStateTime > 42f) : (tmp - Mathf.Floor(tmp) < 0.1f) ) 
            {
                CheckState();
                yield return new WaitForSeconds(timePerBar / 2);
            }
            yield return 0;
        }
    }

    void FadeCurrentState()
    {
        var tmp = source1;
        source1 = source2;
        source2 = tmp;
        source2.DOFade(0, states[currentState - 1].fadeTime).onComplete += () => { source2.Stop(); };
    }

    public void SetPitch(float pitch)
    {
        source1.pitch = pitch;
        source2.pitch = pitch;
    }
}
