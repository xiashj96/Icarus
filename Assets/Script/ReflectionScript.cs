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

    GameObject sun;

    private void Start()
    {
        sun = GameObject.FindGameObjectWithTag("Sun");
    }

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
        if(CurMaterial != null)
        {
            CurMaterial.SetFloat("_CameraOffset", transform.position.y / 16);

            // update sun position
            float viewPosY = Camera.main.WorldToViewportPoint(sun.transform.position).y;

            float _BlueLine = CurMaterial.GetFloat("_BlueLine");
            float _Compression = CurMaterial.GetFloat("_Compression");
            float _SightPoint = CurMaterial.GetFloat("_SightPoint");


            float _RefCen = _BlueLine * (2 - _Compression);
            float b = _RefCen + _SightPoint;
            float a = (b / _RefCen - 1) * (1 - _RefCen);
            float y2 = b * (1 - viewPosY) / (a + 1 - viewPosY);
            float _BlueLine2 = _RefCen - (1 - y2 / _RefCen) * (_RefCen - _BlueLine * _Compression);

            CurMaterial.SetFloat("_BlueLine2", _BlueLine2);
        }

        
    }

    void OnDisable()
    {
        if(renderBuffer != null)
        {
            RenderTexture.ReleaseTemporary(renderBuffer);
            RenderTexture.ReleaseTemporary(tempBuffer);
        }
    }
}
