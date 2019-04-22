using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    // Update is called once per frame
    public float startPosition = -18.9f;
    public float endPosition = -20.64f;

    public Material reflectionMaterial = null;

    public float blueLineStartPosition = 0.12f;
    public float blueLineEndPosition = 0.085f;

    public float endCompression = 0.5f;

    void Start()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, startPosition, gameObject.transform.position.z);
        reflectionMaterial.SetFloat("_BlueLine", blueLineStartPosition);
        reflectionMaterial.SetFloat("_Compression", 0);
    }

    public void StartAllCoroutine(float duration)
    {
        StartCoroutine(MovingCoroutine(duration));
        StartCoroutine(BlueLineCoroutine(duration));
        StartCoroutine(CompressingCoroutine(duration));
    }

    IEnumerator MovingCoroutine(float duration)
    {
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            float position = startPosition + (endPosition - startPosition) * t / duration;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, position, gameObject.transform.position.z);
            yield return 0;
        }
    }

    IEnumerator BlueLineCoroutine(float duration)
    {
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            float blueLine = blueLineStartPosition + (blueLineEndPosition - blueLineStartPosition) * t / duration;
            reflectionMaterial.SetFloat("_BlueLine", blueLine);
            yield return 0;
        }
    }

    IEnumerator CompressingCoroutine(float duration)
    {
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            float compression = endCompression * t / duration;
            reflectionMaterial.SetFloat("_Compression", compression);
            yield return 0;
        }
    }

    void OnDestroy()
    {
        reflectionMaterial.SetFloat("_BlueLine", blueLineStartPosition);
        reflectionMaterial.SetFloat("_Compression", 0);
    }
}
