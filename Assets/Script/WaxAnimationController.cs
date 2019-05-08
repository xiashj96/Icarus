using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaxAnimationController : MonoBehaviour
{
    public float rate = 0.01f;

    Animator animator = null;

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
    	if(animator != null)
        	animator.SetBool("Flame", true);
    }

    public void EndFlame()
    {
        if(animator != null)
    	   animator.SetBool("Flame", false);
    }
}
