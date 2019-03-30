using UnityEngine;
using System.Collections;
 
//设置在编辑模式下也执行该脚本
[ExecuteInEditMode]
//添加选项到菜单中
[AddComponentMenu("Custom/Reflection")]
public class ReflectionScript : MonoBehaviour
{
    public Material CurMaterial;

    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        //着色器实例不为空，就进行参数设置
        if (CurMaterial != null)
        {
            Graphics.Blit(sourceTexture, destTexture, CurMaterial, 0);
        }
 
        //着色器实例为空，直接拷贝屏幕上的效果。此情况下是没有实现屏幕特效的
        else
        {
            //直接拷贝源纹理到目标渲染纹理
            Graphics.Blit(sourceTexture, destTexture);
        }
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            float compression = CurMaterial.GetFloat("_Compression");
            compression += 0.2f * Time.deltaTime;
            if(compression > 1) compression = 1;
            CurMaterial.SetFloat("_Compression", compression);
        }

        if(Input.GetKey(KeyCode.DownArrow))
        {
            float compression = CurMaterial.GetFloat("_Compression");
            compression += -0.2f * Time.deltaTime;
            if(compression < 0) compression = 0;
            CurMaterial.SetFloat("_Compression", compression);
        }
    }

    void OnDisable()
    {
        CurMaterial.SetFloat("_Compression", 0);
    }
}
