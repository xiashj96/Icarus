using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SubtitleManager : MonoBehaviour
{
    public SpriteRenderer[] subtitles;

    float fadeInDuration = 1f;
    float duration = 3f;
    float fadeOutDuration = 1f;
    float space = 1f;

    public IEnumerator SubtitleStart()
    {
    	yield return new WaitForSeconds(15f);
    	foreach(var subtitle in subtitles)
    	{
    		subtitle.DOColor(new Color(1, 1, 1, 0.85f), fadeInDuration);
    		yield return new WaitForSeconds(fadeInDuration);

    		yield return new WaitForSeconds(duration);

    		subtitle.DOColor(new Color(1, 1, 1, 0), fadeOutDuration);
    		yield return new WaitForSeconds(fadeOutDuration);

    		yield return new WaitForSeconds(space);
    	}
    }
}
