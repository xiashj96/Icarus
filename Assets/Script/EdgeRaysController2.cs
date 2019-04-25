using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sun Position Controller For State 4
public class EdgeRaysController2 : MonoBehaviour
{
    public Material edgeRaysMaterial = null;

    public void StartAllCoroutine(float duration)
    {
        StartCoroutine(StrengtheningCoroutine(duration));
    }

    IEnumerator StrengtheningCoroutine(float duration)
    {
    	float startStrength = edgeRaysMaterial.GetFloat("_Strength");
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            float strength = startStrength * (1 - t / duration);
            edgeRaysMaterial.SetFloat("_Strength", strength);
            yield return 0;
        }
    }
}
