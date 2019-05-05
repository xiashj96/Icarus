using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sun Recoverer For State 4
public class SunRecoverer : MonoBehaviour
{
	public GameObject ring, hole, core, halo, plane, wax;

    public float endPosition = 0f;
	public float coreEndScale = 0.2f;
    public float waxEndPosition = -0.27f;
    public float waxEndScale = 0.04f;

    public void StartAllCoroutine(float duration)
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, endPosition, gameObject.transform.position.z);
    	StartCoroutine(CoreCoroutine(duration));
        StartCoroutine(WaxCoroutine(duration));
    }

    IEnumerator CoreCoroutine(float duration)
    {
    	for(float t = 0; t < duration; t += Time.deltaTime)
    	{
    		float scale = coreEndScale * t / duration;
    		core.transform.localScale = new Vector3(scale, scale, 1.0f);
    		halo.transform.localScale = new Vector3(scale, scale, 1.0f);
    		yield return 0;
    	}
    }

    IEnumerator WaxCoroutine(float duration)
    {
        for(float t = 0; t < duration; t += Time.deltaTime)
        {
            float scale = waxEndScale * t / duration;
            float position = waxEndPosition * t / duration;
            wax.transform.localScale = new Vector3(scale, scale, 1.0f);
            wax.transform.localPosition = new Vector3(wax.transform.localPosition.x, position, wax.transform.localPosition.z);
            yield return 0;
        }
    }
}
