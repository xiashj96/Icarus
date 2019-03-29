using UnityEngine;
using System.Collections;
 
//设置在编辑模式下也执行该脚本
[ExecuteInEditMode]
//添加选项到菜单中
[AddComponentMenu("Custom/EdgeRays")]
public class EdgeRaysScript : MonoBehaviour
{
    //-------------------变量声明部分-------------------
    #region Variables
    
    //指定Shader名称
    private string ShaderName = "Custom/EdgeRays";
 
    //着色器和材质实例
    public Shader CurShader;
    private Material CurMaterial;
 
    //几个用于调节参数的中间变量
    public static float ChangeValue1;
    public static float ChangeValue2;
    public static float ChangeValue3;
    public static float ChangeValue4;

    public static float ChangeValue5;
    public static float ChangeValue6;
    public static float ChangeValue7;
    public static float ChangeValue8;

    public static float ChangeValue9;
 
    //Differencing
    [Range(0, 0.01f), Tooltip("[横向差分间隔]差分时，横向间隔的距离。")]
    public float DifferencingScaleX = 0.0000f;

    [Range(0, 0.01f), Tooltip("[纵向差分间隔]差分时，纵向间隔的距离。")]
    public float DifferencingScaleY = 0.00239f;

    //Gauss Blur
    [Range(0, 0.01f), Tooltip("[横向模糊间隔]模糊时，横向间隔的距离。")]
    public float BlurOffsetX = 0.003f;
 
    [Range(0, 0.01f), Tooltip("[纵向模糊间隔]模糊时，纵向间隔的距离。")]
    public float BlurOffsetY = 0.003f;

    //Noising
    public Texture NoiseTexture;
    public Vector2 NoiseTextureScale = new Vector2(1, 1);
    public Vector2 NoiseTextureOffset = new Vector2(0, 0);

    //Baby Step
    [Range(0, 1), Tooltip("[光源点横坐标]光源点的横坐标。")]
    public float StartPointU = 0.5f;

    [Range(0, 1), Tooltip("[光源点纵坐标]光源点的纵坐标。")]
    public float StartPointV = 0.65f;

    [Range(0, 1), Tooltip("[光强]光的强度，数值越大，光照越强。")]
    public float Strength = 0.821f;

    [Range(0, 1), Tooltip("[衰减系数]光的衰减系数，数值越大，光衰减越快。")]
    public float AttenuateRatio = 0.46f;

    //Giant Step
    [Range(0, 2), Tooltip("[距离衰减]光的距离衰减，数值越大，远处的光衰减越多。")]
    public float DistanceAttenuation = 1.14f;

    #endregion
 
    //-------------------------材质的get&set----------------------------
    #region MaterialGetAndSet
    Material material
    {
        get
        {
            if (CurMaterial == null)
            {
                CurMaterial = new Material(CurShader);
                CurMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return CurMaterial;
        }
    }
    #endregion
 
    #region Functions
    //-----------------------------------------【Start()函数】---------------------------------------------  
    // 说明：此函数仅在Update函数第一次被调用前被调用
    //--------------------------------------------------------------------------------------------------------
    void Start()
    {
        //依次赋值
        ChangeValue1 = DifferencingScaleX;
        ChangeValue2 = DifferencingScaleY;
        ChangeValue3 = BlurOffsetX;
        ChangeValue4 = BlurOffsetY;

        ChangeValue5 = StartPointU;
        ChangeValue6 = StartPointV;
        ChangeValue7 = Strength;
        ChangeValue8 = AttenuateRatio;

        ChangeValue9 = DistanceAttenuation;
 
        //找到当前的Shader文件
        CurShader = Shader.Find(ShaderName);
    }
 
    //-------------------------------------【OnRenderImage()函数】------------------------------------  
    // 说明：此函数在当完成所有渲染图片后被调用，用来渲染图片后期效果
    //--------------------------------------------------------------------------------------------------------
    void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture)
    {
        //着色器实例不为空，就进行参数设置
        if (CurShader != null)
        {
            //为Differencing效果添加参数
            material.SetFloat("_DifferencingScaleX", DifferencingScaleX);
            material.SetFloat("_DifferencingScaleY", DifferencingScaleY);
            //为Gauss Blur效果添加参数
            material.SetFloat("_BlurOffsetX", BlurOffsetX);
            material.SetFloat("_BlurOffsetY", BlurOffsetY);
            //为Noising效果添加参数
            material.SetTexture("_NoiseTex", NoiseTexture);
            material.SetTextureScale("_NoiseTex", NoiseTextureScale);
            material.SetTextureOffset("_NoiseTex", NoiseTextureOffset);
            //为Baby Step效果添加参数
            material.SetFloat("_StartPointU", StartPointU);
            material.SetFloat("_StartPointV", StartPointV);
            material.SetFloat("_Strength", Strength);
            material.SetFloat("_AttenuateRatio", AttenuateRatio);
            //为Giant Step效果添加参数
            material.SetFloat("_DistanceAttenuation", DistanceAttenuation);
            material.SetTexture("_OriginTex", sourceTexture);

            //设置渲染模式：双线性
            sourceTexture.filterMode = FilterMode.Bilinear;
            int renderWidth = 1080;
            int renderHeight = 1920;
 
            //准备一个缓存renderBuffer，用于准备存放最终数据
            RenderTexture renderBuffer = RenderTexture.GetTemporary(renderWidth, renderHeight, 0, sourceTexture.format);
            //拷贝sourceTexture中的渲染数据到renderBuffer,并仅绘制指定的pass0的纹理数据
            Graphics.Blit(sourceTexture, renderBuffer, material, 0);

            //高斯模糊，两次模糊，横向纵向，使用pass1进行高斯模糊
            RenderTexture tempBuffer = RenderTexture.GetTemporary(renderWidth, renderHeight, 0, sourceTexture.format);
            material.SetVector("_Offsets", new Vector2(0, BlurOffsetY));
            Graphics.Blit(renderBuffer, tempBuffer, material, 1);
            material.SetVector("_Offsets", new Vector2(BlurOffsetX, 0));
            Graphics.Blit(tempBuffer, renderBuffer, material, 1);


            // 拷贝renderBuffer中的渲染数据到tempBuffer,并仅绘制指定的pass1的纹理数据
            Graphics.Blit(renderBuffer, tempBuffer, material, 2);
            Graphics.Blit(tempBuffer, renderBuffer, material, 3);
            Graphics.Blit(renderBuffer, tempBuffer, material, 4);
            //  清空renderBuffer

            //拷贝最终的renderBuffer到目标纹理，并绘制所有通道的纹理到屏幕
            Graphics.Blit(tempBuffer, destTexture);
            //清空Buffer
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
 
 
    //-----------------------------------------【OnValidate()函数】--------------------------------------  
    // 说明：此函数在编辑器中该脚本的某个值发生了改变后被调用
    //--------------------------------------------------------------------------------------------------------
    void OnValidate()
    {
        //将编辑器中的值赋值回来，确保在编辑器中值的改变立刻让结果生效
        ChangeValue1 = DifferencingScaleX;
        ChangeValue2 = DifferencingScaleY;
        ChangeValue3 = BlurOffsetX;
        ChangeValue4 = BlurOffsetY;

        ChangeValue5 = StartPointU;
        ChangeValue6 = StartPointV;
        ChangeValue7 = Strength;
        ChangeValue8 = AttenuateRatio;

        ChangeValue9 = DistanceAttenuation;
    }
 
    //-----------------------------------------【Update()函数】--------------------------------------  
    // 说明：此函数每帧都会被调用
    //--------------------------------------------------------------------------------------------------------
    void Update()
    {
        //若程序在运行，进行赋值
        if (Application.isPlaying)
        {
            //赋值
            DifferencingScaleX = ChangeValue1;
            DifferencingScaleY = ChangeValue2;
            BlurOffsetX = ChangeValue3;
            BlurOffsetY = ChangeValue4;

            StartPointU = ChangeValue5;
            StartPointV = ChangeValue6;
            Strength = ChangeValue7;
            AttenuateRatio = ChangeValue8;

            DistanceAttenuation = ChangeValue9;

            NoiseTextureOffset += new Vector2(0.02f, 0.02f) * Time.deltaTime;
        }
        //若程序没有在运行，去寻找对应的Shader文件
#if UNITY_EDITOR
        if (Application.isPlaying != true)
        {
            CurShader = Shader.Find(ShaderName);
        }
#endif
 
    }
 
    //-----------------------------------------【OnDisable()函数】---------------------------------------  
    // 说明：当对象变为不可用或非激活状态时此函数便被调用  
    //--------------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        if (CurMaterial)
        {
            //立即销毁材质实例
            DestroyImmediate(CurMaterial);
        }
 
    }
 
 #endregion
 
}
