using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrail : MonoBehaviour
{
    public Texture[] textures1;
    public Texture[] textures2;
    public Texture[] textures3;
    public Texture[] textures4;
    public Rigidbody2D rigidbody;
    public bool to2 = false;
    public bool extinct = false;

    Material material;

    void Start()
    {
    	material = new Material(Shader.Find("Custom/Trail"));
    	Apply();
    }

    public void Apply()
    {
    	GetComponent<TrailRenderer>().material = material;
    	material.SetFloat("_Opacity", 0.7f);
    	StartCoroutine(SwitchCoroutine());
    	StartCoroutine(TimeAdjustion());
    }

    IEnumerator SwitchCoroutine()
    {
    	int st = 0;
    	while(true)
    	{
    		material.SetTexture("_MainTex", textures1[st++]);
    		st %= textures1.Length;
    		yield return new WaitForSeconds(1f / 12);
    		if(to2) break;
    	}

    	for(int i = 0; i < textures2.Length; i++)
    	{
    		material.SetTexture("_MainTex", textures2[i]);
    		yield return new WaitForSeconds(1f / 12);
    	}

    	st = 0;
    	while(true)
    	{
    		material.SetTexture("_MainTex", textures3[st++]);
    		st %= textures3.Length;
    		yield return new WaitForSeconds(1f / 12);
    		if(extinct) break;
    	}

    	for(int i = 0; i < textures4.Length; i++)
    	{
    		material.SetTexture("_MainTex", textures4[i]);
    		yield return new WaitForSeconds(1f / 12);
    	}

    	material.SetFloat("_Opacity", 0f);
    }

    IEnumerator TimeAdjustion()
    {
    	while(true)
    	{
    		GetComponent<TrailRenderer>().time = Mathf.Min(2.0f / rigidbody.velocity.magnitude, 2.5f);
    		yield return 0;
    	}
    }

    void OnDestroy()
    {
    	Destroy(material);
    }
}
