using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaLightController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    void Start()
    {
    	spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    	Disappear();
    }

    public IEnumerator FadeIn(float delay, float duration)
    {
    	yield return new WaitForSeconds(delay);
    	for(float t = 0; t < duration; t += Time.deltaTime)
    	{
    		spriteRenderer.color = new Color(1, 1, 1, t / duration);
    		yield return 0;
    	}
    	spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void Disappear()
    {
    	spriteRenderer.color = new Color(1, 1, 1, 0);
    }
}
