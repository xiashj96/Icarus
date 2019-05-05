using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sun Controller For State 4
public class SunController3 : MonoBehaviour
{
	public GameObject ring, hole, core, halo, plane, wax;

    public float coreEndScale = 0.1f;
    public float holeEndScale = 0.1f;

    public float waxEndPosition = 0.0f;
    public float waxEndScale = 1.0f;

    public void StartAllCoroutine(float duration)
    {
    	StartCoroutine(CoreCoroutine(duration));
    	StartCoroutine(HoleCoroutine(duration));
        StartCoroutine(WaxCoroutine(duration));
    }

    IEnumerator CoreCoroutine(float duration)
    {
        float coreStartScale = core.transform.localScale.x;
    	for(float t = 0; t < duration; t += Time.deltaTime)
    	{
    		float scale = coreStartScale + (coreEndScale - coreStartScale) * Mathf.Pow(t / duration, 2f);
    		core.transform.localScale = new Vector3(scale, scale, 1.0f);
    		halo.transform.localScale = new Vector3(scale, scale, 1.0f);
    		yield return 0;
    	}
        core.transform.localScale = new Vector3(0, 0, 1.0f);
        halo.transform.localScale = new Vector3(0, 0, 1.0f);
    }

    IEnumerator HoleCoroutine(float duration)
    {
    	float holeStartScale = hole.transform.localScale.x;
    	for(float t = 0; t < duration; t += Time.deltaTime)
    	{
    		float scale = holeStartScale + (holeEndScale - holeStartScale) * Mathf.Pow(t / duration, 2f);
    		hole.transform.localScale = new Vector3(scale, scale, 1.0f);
    		ring.transform.localScale = new Vector3(scale, scale, 1.0f);
    		yield return 0;
    	}
        hole.transform.localScale = new Vector3(0, 0, 1.0f);
        ring.transform.localScale = new Vector3(0, 0, 1.0f);
    }

    IEnumerator WaxCoroutine(float duration)
    {
        float waxStartScale = wax.transform.localScale.x;
        float waxStartPosition = wax.transform.localPosition.y;
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            float scale = waxStartScale + (waxEndScale - waxStartScale) * t / duration;
            float position = waxStartPosition + (waxEndPosition - waxStartPosition) * t / duration;
            wax.transform.localScale = new Vector3(scale, scale, 1.0f);
            wax.transform.localPosition = new Vector3(wax.transform.localPosition.x, position, wax.transform.localPosition.z);
            yield return 0;
        }
        wax.transform.localScale = new Vector3(0, 0, 1.0f);
    }
}
