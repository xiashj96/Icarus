using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// move 4 hands randomly inside a circle

public class GenerateHandsInCircle : MonoBehaviour
{
    public GameObject[] hands;

    public float radius;
    public float period;
    Coroutine c = null;

    public void StartGenerating()
    {
        if(c != null)
            StopGenerating();
        c = StartCoroutine(GenerateHands()); 
    }

    public void StopGenerating()
    {
        if(c == null)
            return;
        StopCoroutine(c);
        c = null;
    }

    IEnumerator GenerateHands()
    {
        for (int i = 0;;i++)
        {
            var hand = hands[i % hands.Length];
            var instance = GameObject.Instantiate(hand, transform.position, Quaternion.identity);
            instance.SetActive(true);
            instance.transform.position = (Vector2)instance.transform.position + Vector2.Scale(Random.insideUnitCircle, new Vector2(radius, radius));
            Destroy(instance, 1f);
            yield return new WaitForSeconds(period);
        }
    }

}
