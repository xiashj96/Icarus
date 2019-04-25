using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Background Controller For State 4
public class BackgroundController3 : MonoBehaviour
{
    public float endPosition = -17.92f;

    public Material reflectionMaterial = null;

    public float blueLineEndPosition = 0.13f;

    public void StartAllCoroutine(float duration)
    {
        StartCoroutine(MovingCoroutine(duration));
        StartCoroutine(BlueLineCoroutine(duration));
        StartCoroutine(CompressingCoroutine(duration));
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

    IEnumerator BlueLineCoroutine(float duration)
    {
        float blueLineStartPosition = reflectionMaterial.GetFloat("_BlueLine");
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            float blueLine = blueLineStartPosition + (blueLineEndPosition - blueLineStartPosition) * t / duration;
            reflectionMaterial.SetFloat("_BlueLine", blueLine);
            yield return 0;
        }
    }

    IEnumerator CompressingCoroutine(float duration)
    {
        float startCompression = reflectionMaterial.GetFloat("_Compression");
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            float compression = startCompression * (1 - t / duration);
            reflectionMaterial.SetFloat("_Compression", compression);
            yield return 0;
        }
    }
}
