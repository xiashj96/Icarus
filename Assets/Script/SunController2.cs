using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sun Controller For State 3
public class SunController2 : MonoBehaviour
{
	public GameObject ring, hole, core, halo, plane, wax;

    public float duration = 0.5f;

	public float coreEndScale = 1.1f;
	public float holeEndScale = 0.94f;

	public float planeStartScale = 0.25f;
	public float planeEndScale = 0.78f;

    public float waxEndPosition = 0.0f;
    public float waxEndScale = 1.0f;

    public void StartAllCoroutine()
    {
    	StartCoroutine(CoreCoroutine());
    	StartCoroutine(HoleCoroutine());
    	StartCoroutine(PlaneCoroutine());
        StartCoroutine(WaxCoroutine());
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

    IEnumerator WaxCoroutine()
    {
        wax.GetComponent<WaxAnimationController>().Flame();
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
    }
}
