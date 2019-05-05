using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sun Controller For State 1
public class SunController : MonoBehaviour
{
	public GameObject ring, hole, core, halo, plane, wax;

	public float coreStartScale = 0.3f;
	public float coreEndScale = 1.0f;

	public float holeStartTime = 0.25f;
	public float holeEndScale = 0.85f;

    public float waxStartPosition = 0.0f;
    public float waxEndPosition = 0.0f;
    public float waxStartScale = 1.0f;
    public float waxEndScale = 1.0f;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        core.transform.localScale = new Vector3(coreStartScale, coreStartScale, 1.0f);
        halo.transform.localScale = new Vector3(coreStartScale, coreStartScale, 1.0f);
        hole.transform.localScale = new Vector3(0, 0, 1);
        ring.transform.localScale = new Vector3(0, 0, 1);
        plane.transform.localScale = new Vector3(0, 0, 1);
        wax.GetComponent<WaxAnimationController>().EndFlame();
        wax.transform.localPosition = new Vector3(wax.transform.localPosition.x, waxStartPosition, wax.transform.localPosition.z);
        wax.transform.localScale = new Vector3(waxStartScale, waxStartScale, 1);
    }

    public void StartAllCoroutine(float duration)
    {
    	StartCoroutine(CoreCoroutine(duration));
    	StartCoroutine(HoleCoroutine(duration));
        StartCoroutine(WaxCoroutine(duration));
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

    IEnumerator WaxCoroutine(float duration)
    {
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
