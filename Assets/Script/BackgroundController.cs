using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Background Controller For State 1
public class BackgroundController : MonoBehaviour
{
    // Update is called once per frame
    public float startPosition = -18.9f;
    public float endPosition = -20.64f;

    public Material reflectionMaterial = null;

    public float blueLineStartPosition = 0.12f;
    public float blueLineEndPosition = 0.085f;

    public float endCompression = 0.5f;
    GameSystem GS;

    void Start()
    {
        Initialize();
        GS = FindObjectOfType<GameSystem>();
    }

    public void Initialize()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, startPosition, gameObject.transform.position.z);
        reflectionMaterial.SetFloat("_BlueLine", blueLineStartPosition);
        reflectionMaterial.SetFloat("_Compression", 0);
    }

    public void StartAllCoroutine()
    {
        StartCoroutine(MovingCoroutine());
        StartCoroutine(BlueLineCoroutine());
        StartCoroutine(CompressingCoroutine());
    }

    IEnumerator MovingCoroutine()
    {
        while (GS.state == 1)
        {
            float position = startPosition + (endPosition - startPosition) * GS.s1SmoothProgress;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, position, gameObject.transform.position.z);
            yield return 0;
        }
    }

    IEnumerator BlueLineCoroutine()
    {
        while (GS.state == 1)
        {
            float blueLine = blueLineStartPosition + (blueLineEndPosition - blueLineStartPosition) * GS.s1SmoothProgress;
            reflectionMaterial.SetFloat("_BlueLine", blueLine);
            yield return 0;
        }
    }

    IEnumerator CompressingCoroutine()
    {
        while (GS.state == 1)
        {
            float compression = endCompression * GS.s1SmoothProgress;
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
