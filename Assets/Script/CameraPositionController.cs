using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Camera Position Controller For State 6
public class CameraPositionController : MonoBehaviour
{
    public float endPosition = -42.56f;
    public float startPosition = 12.3f;

    public void StartAllCoroutine(float duration)
    {
        StartCoroutine(MovingCoroutine(duration));
    }

    public void StartAllCoroutine2(float duration)
    {
        StartCoroutine(MovingCoroutine2(duration));
    }

    IEnumerator MovingCoroutine(float duration)
    {
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            float position = endPosition * t / duration;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, position, gameObject.transform.position.z);
            yield return 0;
        }
    }

    IEnumerator MovingCoroutine2(float duration)
    {
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            float position = startPosition * (1 - t / duration);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, position, gameObject.transform.position.z);
            yield return 0;
        }
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
    }
}
