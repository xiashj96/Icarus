using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFlyAnimation : MonoBehaviour
{
    public float minTime;
    public float maxTime;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(WaitSeconds());
    }

    IEnumerator WaitSeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            if (Random.value > 0.5f)
            { 
                animator.SetTrigger("Fly1");
            }
            /*else
            {
                animator.SetTrigger("Fly2");
            }*/
            yield return new WaitUntil(
                () => animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
                );
        }
    }
}
