using UnityEngine;
using System.Collections;
 
//设置在编辑模式下也执行该脚本
[ExecuteInEditMode]
//添加选项到菜单中
[AddComponentMenu("Custom/EdgeRays")]
public class EdgeRaysScript : MonoBehaviour
{
    public Material CurMaterial;
 
    //-------------------------------------【OnRenderImage()函数】------------------------------------  
    // 说明：此函数在当完成所有渲染图片后被调用，用来渲染图片后期效果
    //--------------------------------------------------------------------------------------------------------
    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        //着色器实例不为空，就进行参数设置
        if (CurMaterial != null)
        {
            int renderWidth = sourceTexture.width;
            int renderHeight = sourceTexture.height;

            CurMaterial.SetTexture("_OriginTex", sourceTexture);

            RenderTexture renderBuffer = RenderTexture.GetTemporary(renderWidth, renderHeight, 0, sourceTexture.format);
            RenderTexture tempBuffer = RenderTexture.GetTemporary(renderWidth, renderHeight, 0, sourceTexture.format);

            sourceTexture.filterMode = FilterMode.Bilinear;
            renderBuffer.filterMode = FilterMode.Bilinear;
            tempBuffer.filterMode = FilterMode.Bilinear;
            
            Graphics.Blit(sourceTexture, renderBuffer, CurMaterial, 0);

            float BlurOffsetX = CurMaterial.GetFloat("_BlurOffsetX");
            float BlurOffsetY = CurMaterial.GetFloat("_BlurOffsetY");
            CurMaterial.SetVector("_Offsets", new Vector2(0, BlurOffsetY));
            Graphics.Blit(renderBuffer, tempBuffer, CurMaterial, 1);
            CurMaterial.SetVector("_Offsets", new Vector2(BlurOffsetX, 0));
            Graphics.Blit(tempBuffer, renderBuffer, CurMaterial, 1);

            Graphics.Blit(renderBuffer, tempBuffer, CurMaterial, 2);
            Graphics.Blit(tempBuffer, renderBuffer, CurMaterial, 3);
            Graphics.Blit(renderBuffer, tempBuffer, CurMaterial, 4);

            //Graphics.Blit(renderBuffer, destTexture);
            Graphics.Blit(tempBuffer, destTexture);

            RenderTexture.ReleaseTemporary(renderBuffer);
            RenderTexture.ReleaseTemporary(tempBuffer);
 
        }
 
        //着色器实例为空，直接拷贝屏幕上的效果。此情况下是没有实现屏幕特效的
        else
        {
            //直接拷贝源纹理到目标渲染纹理
            Graphics.Blit(sourceTexture, destTexture);
        }
    }
 
    //-----------------------------------------【Update()函数】--------------------------------------  
    // 说明：此函数每帧都会被调用
    //--------------------------------------------------------------------------------------------------------
    void Update()
    {
        //若程序在运行，进行赋值
        if (Application.isPlaying)
        {
            Vector2 offset = CurMaterial.GetTextureOffset("_NoiseTex");
            offset += new Vector2(0.02f, 0.02f) * Time.deltaTime;
            CurMaterial.SetTextureOffset("_NoiseTex", offset);
        }
    }
 
}
