using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sun Recoverer For State 4
public class SunRecoverer : MonoBehaviour
{
	public GameObject ring, hole, core, halo, plane;

    public float endPosition = 0f;
	public float coreEndScale = 0.2f;

    public void StartAllCoroutine(float duration)
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, endPosition, gameObject.transform.position.z);
    	StartCoroutine(CoreCoroutine(duration));
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
}
