using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateAfterTime : MonoBehaviour
{
    public float time;
    Coroutine c;

    // Start is called before the first frame update
    void OnEnable()
    {
        c = StartCoroutine(Deactivate());
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
