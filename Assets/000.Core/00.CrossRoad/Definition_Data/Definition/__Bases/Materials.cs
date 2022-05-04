using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
    using Piglet;

    public static class Materials
    {
        static Material MAT_DEFAULT;
        static Material MAT_TRANSPARENT;

        static Material MAT_ISSUE;
        static Material MAT_DAMAGE;
        static Material MAT_RECOVER;

        public static void Init(MaterialType type)
        {
            Set(type);
        }

        public static Material Set(MaterialType type)
        {
            Material result = Resources.Load<Material>("3D/DefaultMat");

            RenderPipelineType pipeline = RenderPipelineUtil.GetRenderPipeline();

            switch (type)
            {
                case MaterialType.Default:
                    if (pipeline == RenderPipelineType.BuiltIn)
                    {
                        MAT_DEFAULT = Resources.Load<Material>("3D/BltIn_Default");
                        MAT_TRANSPARENT = Resources.Load<Material>("3D/BltIn_Transparent");

                        result = MAT_DEFAULT;
                    }
                    else if (pipeline == RenderPipelineType.URP)
                    {
                        //MAT_DEFAULT = Resources.Load<Material>("3D/URP_Default2");  // 홀로그램
                        MAT_DEFAULT = Resources.Load<Material>("3D/URP_Default");       // 기본
                        MAT_TRANSPARENT = Resources.Load<Material>("3D/URP_Transparent");


                        //MAT_DEFAULT = Resources.Load<Material>("3D/URP_Default2");
                        //MAT_TRANSPARENT = Resources.Load<Material>("3D/URP_Transparent2");
                        result = MAT_DEFAULT;
                    }
                    break;

                case MaterialType.Issue:
                    {
                        if (pipeline == RenderPipelineType.BuiltIn)
                        {
                            MAT_ISSUE = Resources.Load<Material>("3D/Issue/BltIn/Issue");
                        }
                        else if (pipeline == RenderPipelineType.URP)
                        {
                            MAT_ISSUE = Resources.Load<Material>("3D/Issue/URP/Issue");
                        }
                        result = MAT_ISSUE;
                    }
                    break;

                case MaterialType.Issue_dmg:
                    {
                        if (pipeline == RenderPipelineType.BuiltIn)
                        {
                            MAT_DAMAGE = Resources.Load<Material>("3D/Issue/BltIn/Issue_dmg");
                        }
                        else if (pipeline == RenderPipelineType.URP)
                        {
                            MAT_DAMAGE = Resources.Load<Material>("3D/Issue/URP/Issue_dmg");
                        }
                        result = MAT_DAMAGE;
                    }
                    break;

                case MaterialType.Issue_rcv:
                    {
                        if (pipeline == RenderPipelineType.BuiltIn)
                        {
                            MAT_RECOVER = Resources.Load<Material>("3D/Issue/BltIn/Issue_rcv");
                        }
                        else if (pipeline == RenderPipelineType.URP)
                        {
                            MAT_RECOVER = Resources.Load<Material>("3D/Issue/URP/Issue_rcv");
                        }
                        result = MAT_RECOVER;
                    }
                    break;



                case MaterialType.Default1:
                    result = Resources.Load<Material>("3D/DefaultMat");
                    break;

                case MaterialType.White:
                    throw new System.Exception("White 구현");

                case MaterialType.ObjDefault1:
                    result = Resources.Load<Material>("3D/ObjMat");
                    break;
            }

            return result;
        }

        static public void Set(MeshRenderer _render, GraphicCode _gCode, ColorType _colorType, float _trans, bool isOnSelect)
        {
            Material mat = _render.material;
            Color colr = mat.color;
            string colrKey = "";

            RenderPipelineType pipeline = RenderPipelineUtil.GetRenderPipeline();
            if (pipeline == RenderPipelineType.URP)
            {
                //colrKey = "_MainColor";
                colrKey = "_BaseColor";
            }
            else if (pipeline == RenderPipelineType.BuiltIn)
            {
                colrKey = "_Color";
            }
            else
            {
                colrKey = "_BaseColor";
            }

            // Material이 여러개인 경우 할 행위
            int index = _render.materials.Length;
            for (int i = 0; i < index; i++)
            {
                if(_gCode == GraphicCode.Single_Color)
                {
                    _render.materials[i].SetColor(colrKey, Colors.Set(_colorType, _trans));
                }
                else if(_gCode == GraphicCode.Platform_Texturing)
                {
                    if(isOnSelect)
                    {
                        _render.materials[i].SetColor(colrKey, Colors.Set(_colorType, _trans));
                    }
                    else
                    {
                        // 기존 색에서 변경
                        _render.materials[i].SetColor(colrKey, Colors.Set(_colorType, _render.materials[i].color, _trans));
                    }
                }
            }
            //_render.material.SetColor(colrKey, Colors.Set(_colorType, _trans));
        }

        public static void ToOpaqueMode(MeshRenderer render)
        {
            //UnityEditor.Rendering.Universal.ShaderGUI.SimpleLitGUI
            {
                Material[] bef = render.materials;
                for (int i = 0; i < bef.Length; i++)
                {
                    //bef[i].SetFloat("_Mode", 0.0f);
                    bef[i].SetOverrideTag("RenderType", "Opaque");
					bef[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
					bef[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    bef[i].SetInt("_Surface", 0);
                    bef[i].SetInt("_ZWrite", 1);
                    bef[i].doubleSidedGI = false;
                    //bef[i].DisableKeyword("_ALPHATEST_ON"); 
                    //bef[i].DisableKeyword("_ALPHABLEND_ON");
                    //bef[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    bef[i].SetShaderPassEnabled("SHADOWCASTER", true);

                    bef[i].renderQueue = 2000;


                    //bef[i].SetOverrideTag("RenderType", "Opaque"); 
                    //bef[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One); 
                    //bef[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero); 
                    //bef[i].SetInt("_ZWrite", 1); 
                    ////bef[i].doubleSidedGI = false;
                    //bef[i].DisableKeyword("_ALPHATEST_ON"); 
                    //bef[i].DisableKeyword("_ALPHABLEND_ON");
                    //bef[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    ////bef[i].SetShaderPassEnabled("SHADOWCASTER", true);
                    //bef[i].renderQueue = 2000;

                }

                render.materials = bef;
            }

            //{
            //	Material[] bef = render.materials;
            //	for (int i = 0; i < bef.Length; i++)
            //	{
            //		bef[i].SetOverrideTag("RenderType", "Opaque");
            //		bef[i].doubleSidedGI = false;

            //		bef[i].renderQueue = 2000;

            //		bef[i].SetShaderPassEnabled("SHADOWCASTER", true);
            //	}
            //}

            {
                //Material before = render.material;
                //Color bColor = before.color;

                //Material now = new Material(MAT_DEFAULT);	// 이 방식은 텍스처 모드에서 정상동작하지 않음.
                //now.color = bColor;

                //render.material = now;
            }
        }

        public static void ToFadeMode(MeshRenderer render)
        {
            {
                Material[] bef = render.materials;
                for (int i = 0; i < bef.Length; i++)
                {
                    //bef[i].SetFloat("_Mode", 3.0f);
                    bef[i].SetOverrideTag("RenderType", "Transparent");
					bef[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha); 
					bef[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    bef[i].SetInt("_Surface", 1);
					bef[i].SetInt("_ZWrite", 0);
					bef[i].doubleSidedGI = true;
                    //bef[i].DisableKeyword("_ALPHATEST_ON"); 
                    //bef[i].DisableKeyword("_ALPHABLEND_ON"); 
                    //bef[i].EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    bef[i].SetShaderPassEnabled("SHADOWCASTER", false);

                    bef[i].renderQueue = 3000;


                    //bef[i].SetOverrideTag("RenderType", "Transparent"); 
                    //bef[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One); 
                    //bef[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha); 
                    //bef[i].SetInt("_ZWrite", 0); 
                    //bef[i].DisableKeyword("_ALPHATEST_ON"); 
                    //bef[i].DisableKeyword("_ALPHABLEND_ON"); 
                    //bef[i].EnableKeyword("_ALPHAPREMULTIPLY_ON"); 
                    //bef[i].renderQueue = 3000;
                }

                render.materials = bef;
            }

            {
                //Material before = render.material;
                //Color bColor = before.color;

                //Material now = new Material(MAT_TRANSPARENT);	// 이 방식은 텍스처 모드에서 정상동작하지 않음.
                //now.color = bColor;

                //render.material = now;
            }
        }
    }
}
