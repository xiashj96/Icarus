using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sun Controller For State 3
public class SunController2 : MonoBehaviour
{
	public GameObject ring, hole, core, halo, plane;

    public float duration = 0.5f;

	public float coreEndScale = 1.1f;
	public float holeEndScale = 0.94f;

	public float planeStartScale = 0.25f;
	public float planeEndScale = 0.78f;

    public void StartAllCoroutine()
    {
    	StartCoroutine(CoreCoroutine());
    	StartCoroutine(HoleCoroutine());
    	StartCoroutine(PlaneCoroutine());
    }

    IEnumerator CoreCoroutine()
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

    IEnumerator HoleCoroutine()
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

    IEnumerator PlaneCoroutine()
    {
    	for(float t = 0; t < duration; t += Time.deltaTime)
    	{
    		float scale = planeStartScale + (planeEndScale - planeStartScale) * t / duration;
    		plane.transform.localScale = new Vector3(scale, scale, 1.0f);
    		yield return 0;
    	}
    }
}
