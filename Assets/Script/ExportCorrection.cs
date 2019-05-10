using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExportCorrection : MonoBehaviour
{
	public Material[] materials;

    void Update()
    {
        float correction = 9f / 16f * Screen.height / Screen.width;
        foreach(var material in materials)
        	material.SetFloat("_ExportCorrection", correction);
    }
}
