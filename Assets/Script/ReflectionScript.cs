using UnityEngine;
using System.Collections;
 
//设置在编辑模式下也执行该脚本
[ExecuteInEditMode]
//添加选项到菜单中
[AddComponentMenu("Custom/Reflection")]
public class ReflectionScript : MonoBehaviour
{
    public Material CurMaterial;

    RenderTexture renderBuffer = null;
    RenderTexture tempBuffer = null;

    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        if (CurMaterial != null)
        {
            if(renderBuffer == null)
            {
                int renderWidth = sourceTexture.width;
                int renderHeight = sourceTexture.height;
                renderBuffer = RenderTexture.GetTemporary(renderWidth, renderHeight, 0, sourceTexture.format);
                tempBuffer = RenderTexture.GetTemporary(renderWidth, renderHeight, 0, sourceTexture.format);
            }

            Graphics.Blit(sourceTexture, renderBuffer, CurMaterial, 0);

            float BlurOffsetX = CurMaterial.GetFloat("_BlurOffsetX");
            float BlurOffsetY = CurMaterial.GetFloat("_BlurOffsetY");
            CurMaterial.SetVector("_Offsets", new Vector2(BlurOffsetX, 0));
            Graphics.Blit(renderBuffer, tempBuffer, CurMaterial, 1);
            CurMaterial.SetVector("_Offsets", new Vector2(BlurOffsetX * 3, 0));
            Graphics.Blit(tempBuffer, renderBuffer, CurMaterial, 1);
            CurMaterial.SetVector("_Offsets", new Vector2(0, BlurOffsetY));
            Graphics.Blit(renderBuffer, tempBuffer, CurMaterial, 1);
            CurMaterial.SetVector("_Offsets", new Vector2(0, BlurOffsetY * 3));
            Graphics.Blit(tempBuffer, renderBuffer, CurMaterial, 1);

            CurMaterial.SetTexture("_OriginTex", sourceTexture);
            Graphics.Blit(renderBuffer, destTexture, CurMaterial, 2);
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

        if(renderBuffer != null)
        {
            RenderTexture.ReleaseTemporary(renderBuffer);
            RenderTexture.ReleaseTemporary(tempBuffer);
        }
    }
}
