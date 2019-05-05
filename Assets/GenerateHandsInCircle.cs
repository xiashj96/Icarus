using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// move 4 hands randomly inside a circle

public class GenerateHandsInCircle : MonoBehaviour
{
    public GameObject[] hands;

    public float radius;
    public float period;
    Coroutine c;

    public void StartGenerating()
    {
        c = StartCoroutine(GenerateHands()); 
    }

    public void StopGenerating()
    {
        StopCoroutine(c);
    }

    IEnumerator GenerateHands()
    {
        for (int i = 0;;i++)
        {
            var hand = hands[i % hands.Length];
            hand.SetActive(true);
            hand.transform.localPosition = Vector2.Scale(Random.insideUnitCircle, new Vector2(radius, radius));
            yield return new WaitForSeconds(period);
        }
    }

}
