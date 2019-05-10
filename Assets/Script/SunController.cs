using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sun Controller For State 1
public class SunController : MonoBehaviour
{
	public GameObject ring, hole, core, halo, plane, wax, dropper;
    GameSystem GS;
	public float coreStartScale = 0.3f;
	public float coreEndScale = 1.0f;

	public float holeEndScale = 0.85f;

    public float waxStartPosition = 0.0f;
    public float waxEndPosition = 0.0f;
    public float waxStartScale = 1.0f;
    public float waxEndScale = 1.0f;

    void Start()
    {
        Initialize();
        GS = FindObjectOfType<GameSystem>();
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
        var trails = dropper.GetComponent<ParticleSystem>().trails;
        trails.widthOverTrail = 0.03f;
        trails.lifetimeMultiplier = 0.02f;
    }

    public void StartAllCoroutine()
    {
    	StartCoroutine(CoreCoroutine());
    	StartCoroutine(HoleCoroutine());
        StartCoroutine(WaxCoroutine());
        var emission = dropper.GetComponent<ParticleSystem>().emission;
        emission.rateOverTimeMultiplier = 0.25f;
    }

    IEnumerator CoreCoroutine()
    {
    	while(GS.state == 1)
    	{
    		float scale = coreStartScale + (coreEndScale - coreStartScale) * GS.s1SmoothProgress;
    		core.transform.localScale = new Vector3(scale, scale, 1.0f);
    		halo.transform.localScale = new Vector3(scale, scale, 1.0f);
    		yield return 0;
    	}
    }

    IEnumerator HoleCoroutine()
    {
    	yield return new WaitForSeconds(60F);
        float startSP = GS.s1SmoothProgress;
        while (GS.state == 1)
        {
    		float scale = holeEndScale * (GS.s1SmoothProgress - startSP) / (1 - startSP);
    		hole.transform.localScale = new Vector3(scale, scale, 1.0f);
    		ring.transform.localScale = new Vector3(scale, scale, 1.0f);
    		yield return 0;
    	}
    }

    IEnumerator WaxCoroutine()
    {
        while (GS.state == 1)
        {
            float scale = waxStartScale + (waxEndScale - waxStartScale) * GS.s1SmoothProgress;
            float position = waxStartPosition + (waxEndPosition - waxStartPosition) * GS.s1SmoothProgress;
            wax.transform.localScale = new Vector3(scale, scale, 1.0f);
            wax.transform.localPosition = new Vector3(wax.transform.localPosition.x, position, wax.transform.localPosition.z);
            yield return 0;
        }
    }
}
