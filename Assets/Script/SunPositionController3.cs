using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sun Position Controller For State 4
public class SunPositionController3 : MonoBehaviour
{
    public float endPosition = 5f;

    public void StartAllCoroutine(float duration)
    {
        StartCoroutine(MovingCoroutine(duration));
    }

    IEnumerator MovingCoroutine(float duration)
    {
        float startPosition = gameObject.transform.position.y;
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            float position = startPosition + (endPosition - startPosition) * Mathf.Pow(t / duration, 2f);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, position, gameObject.transform.position.z);
            yield return 0;
        }
    }
}
