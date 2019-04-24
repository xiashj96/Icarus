using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sun Controller For State 3
public class SunController2 : MonoBehaviour
{
	public GameObject ring, hole, core, halo, plane;

	public float coreEndScale = 1.1f;
	public float holeEndScale = 0.94f;

	public float planeStartScale = 0.25f;
	public float planeEndScale = 0.78f;

    public void StartAllCoroutine(float duration)
    {
    	StartCoroutine(CoreCoroutine(duration));
    	StartCoroutine(HoleCoroutine(duration));
    	StartCoroutine(PlaneCoroutine(duration));
    }

    IEnumerator CoreCoroutine(float duration)
    {
        float coreStartScale = core.transform.localScale.x;
    	for(float t = 0; t < duration; t += Time.deltaTime)
    	{
    		float scale = coreStartScale + (coreEndScale - coreStartScale) * t / duration;
    		core.transform.localScale = new Vector3(scale, scale, 1.0f);
    		halo.transform.localScale = new Vector3(scale, scale, 1.0f);
    		yield return 0;
    	}
    }

    IEnumerator HoleCoroutine(float duration)
    {
    	float holeStartScale = hole.transform.localScale.x;
    	for(float t = 0; t < duration; t += Time.deltaTime)
    	{
    		float scale = holeStartScale + (holeEndScale - holeStartScale) * t / duration;
    		hole.transform.localScale = new Vector3(scale, scale, 1.0f);
    		ring.transform.localScale = new Vector3(scale, scale, 1.0f);
    		yield return 0;
    	}
    }

    IEnumerator PlaneCoroutine(float duration)
    {
    	for(float t = 0; t < duration; t += Time.deltaTime)
    	{
    		float scale = planeStartScale + (planeEndScale - planeStartScale) * t / duration;
    		plane.transform.localScale = new Vector3(scale, scale, 1.0f);
    		yield return 0;
    	}
    }
}
