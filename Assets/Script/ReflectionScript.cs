using UnityEngine;
using System.Collections;
 
//设置在编辑模式下也执行该脚本
[ExecuteInEditMode]
//添加选项到菜单中
[AddComponentMenu("Custom/Reflection")]
public class ReflectionScript : MonoBehaviour
{
    //-------------------变量声明部分-------------------
 
    //材质实例
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

            sourceTexture.filterMode = FilterMode.Bilinear;
 
            Graphics.Blit(sourceTexture, destTexture, CurMaterial, 0);
        }
 
        //着色器实例为空，直接拷贝屏幕上的效果。此情况下是没有实现屏幕特效的
        else
        {
            //直接拷贝源纹理到目标渲染纹理
            Graphics.Blit(sourceTexture, destTexture);
        }
    }
}
