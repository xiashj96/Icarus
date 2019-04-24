using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sun Position Controller For State 3
public class SunPositionController2 : MonoBehaviour
{
    public float endPosition = 4.23f;

    public void StartAllCoroutine(float duration)
    {
        StartCoroutine(MovingCoroutine(duration));
    }

    IEnumerator MovingCoroutine(float duration)
    {
        float startPosition = gameObject.transform.position.y;
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            float position = startPosition + (endPosition - startPosition) * t / duration;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, position, gameObject.transform.position.z);
            yield return 0;
        }
    }
}
