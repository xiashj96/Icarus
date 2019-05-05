using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaxAnimationController : MonoBehaviour
{
    public float rate = 0.01f;

    Animator animator;

    void Start()
    {
    	animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        if(Random.Range(0f, 1f) < rate)
        	animator.SetTrigger("Drop");
    }

    public void Flame()
    {
    	Debug.Log("flame");
    	animator.SetBool("Flame", true);
    }

    public void EndFlame()
    {
    	animator.SetBool("Flame", false);
    }
}
