using UnityEngine;
using System.Collections;
 
//设置在编辑模式下也执行该脚本
[ExecuteInEditMode]
//添加选项到菜单中
[AddComponentMenu("Custom/EdgeRays")]
public class EdgeRaysScript : MonoBehaviour
{
    public Material CurMaterial;

    RenderTexture renderBuffer = null;
    RenderTexture tempBuffer = null;

    GameObject sun;

    private void Start()
    {
        sun = GameObject.FindGameObjectWithTag("Sun");
    }

    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        //着色器实例不为空，就进行参数设置
        if (CurMaterial != null)
        {
            if(renderBuffer == null)
            {
                int renderWidth = sourceTexture.width;
                int renderHeight = sourceTexture.height;
                renderBuffer = RenderTexture.GetTemporary(renderWidth, renderHeight, 0, sourceTexture.format);
                tempBuffer = RenderTexture.GetTemporary(renderWidth, renderHeight, 0, sourceTexture.format);
            }
            
            CurMaterial.SetTexture("_OriginTex", sourceTexture);
            
            Graphics.Blit(sourceTexture, renderBuffer, CurMaterial, 0);

            float BlurOffsetX = CurMaterial.GetFloat("_BlurOffsetX");
            float BlurOffsetY = CurMaterial.GetFloat("_BlurOffsetY");
            CurMaterial.SetVector("_Offsets", new Vector2(0, BlurOffsetY));
            Graphics.Blit(renderBuffer, tempBuffer, CurMaterial, 1);
            CurMaterial.SetVector("_Offsets", new Vector2(BlurOffsetX, 0));
            Graphics.Blit(tempBuffer, renderBuffer, CurMaterial, 1);

            Graphics.Blit(renderBuffer, tempBuffer, CurMaterial, 2);
            Graphics.Blit(tempBuffer, renderBuffer, CurMaterial, 3);
            Graphics.Blit(renderBuffer, destTexture, CurMaterial, 4);
 
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
        //若程序在运行，进行赋值
        if (Application.isPlaying)
        {
            Vector2 offset = CurMaterial.GetTextureOffset("_NoiseTex");
            offset += new Vector2(0.02f, 0.02f) * Time.deltaTime;
            CurMaterial.SetTextureOffset("_NoiseTex", offset);

            // update sun position
            Vector3 viewPos = Camera.main.WorldToViewportPoint(sun.transform.position);
            CurMaterial.SetFloat("_StartPointV", viewPos.y);
        }
    }

    void OnDisable()
    {
        CurMaterial.SetTextureOffset("_NoiseTex", new Vector2(0, 0));

        if(renderBuffer != null)
        {
            RenderTexture.ReleaseTemporary(renderBuffer);
            RenderTexture.ReleaseTemporary(tempBuffer);
        }
    }
 
}
