using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sun Position Controller For State 1
public class EdgeRaysController : MonoBehaviour
{
    // Update is called once per frame
    public float startStrength = 0.2f;

    public AnimationCurve curve;

    public Material edgeRaysMaterial = null;

    void Start()
    {
        edgeRaysMaterial.SetFloat("_Strength", startStrength);
    }

    public void StartAllCoroutine(float duration)
    {
        StartCoroutine(StrengtheningCoroutine(duration));
    }

    IEnumerator StrengtheningCoroutine(float duration)
    {
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            float strength = startStrength / curve.Evaluate(0) * curve.Evaluate(t / duration);
            edgeRaysMaterial.SetFloat("_Strength", strength);
            yield return 0;
        }
    }

    void OnDestroy()
    {
        edgeRaysMaterial.SetFloat("_Strength", startStrength);
    }
}
