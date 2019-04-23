using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
	public GameObject ring, hole, core, halo, plane;

	public float coreStartScale = 0.3f;
	public float coreEndScale = 1.0f;

	public float holeStartTime = 0.25f;
	public float holeEndScale = 0.85f;

	public float planeStartTime = 0.3f;
	public float planeStartScale = 0.25f;
	public float planeEndScale = 0.78f;

    void Start()
    {
        core.transform.localScale = new Vector3(coreStartScale, coreStartScale, 1.0f);
        halo.transform.localScale = new Vector3(coreStartScale, coreStartScale, 1.0f);
        hole.transform.localScale = new Vector3(0, 0, 1);
        ring.transform.localScale = new Vector3(0, 0, 1);
        plane.transform.localScale = new Vector3(0, 0, 1);
    }

    public void StartAllCoroutine(float duration)
    {
    	StartCoroutine(CoreCoroutine(duration));
    	StartCoroutine(HoleCoroutine(duration));
    	//StartCoroutine(PlaneCoroutine(duration));
    }

    IEnumerator CoreCoroutine(float duration)
    {
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
    	yield return new WaitForSeconds(duration * holeStartTime);
    	for(float t = duration * holeStartTime; t < duration; t += Time.deltaTime)
    	{
    		float scale = holeEndScale * (t / duration - holeStartTime) / (1 - holeStartTime);
    		hole.transform.localScale = new Vector3(scale, scale, 1.0f);
    		ring.transform.localScale = new Vector3(scale, scale, 1.0f);
    		yield return 0;
    	}
    }

    IEnumerator PlaneCoroutine(float duration)
    {
    	yield return new WaitForSeconds(duration * planeStartTime);
    	for(float t = duration * planeStartTime; t < duration; t += Time.deltaTime)
    	{
    		float scale = planeStartScale + (planeEndScale - planeStartScale) * (t / duration - planeStartTime) / (1 - planeStartTime);
    		plane.transform.localScale = new Vector3(scale, scale, 1.0f);
    		yield return 0;
    	}
    }
}
