using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Edge Rays Controller For State 3
public class EdgeRaysController : MonoBehaviour
{
    public float duration = 2f;

    public float startStrength = 0.2f;

    public AnimationCurve curve;

    public Material edgeRaysMaterial = null;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        edgeRaysMaterial.SetFloat("_Strength", startStrength);
    }

    public void StartAllCoroutine()
    {
        StartCoroutine(StrengtheningCoroutine());
    }

    IEnumerator StrengtheningCoroutine()
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
