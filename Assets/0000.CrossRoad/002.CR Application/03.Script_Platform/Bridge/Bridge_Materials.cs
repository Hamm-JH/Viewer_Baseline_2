using Definition;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Platform.Bridge
{
    public static class Bridge_Materials
    {
        //public static void Set(MeshRenderer source, GraphicCode _gCode, BridgeCode _bCode, BridgeCodeDetail _bCodeDetail)
        public static void Set(MeshRenderer source, GraphicCode _gCode, string _name)
        {
            Material[] mats = Set(_gCode, _name);
            //Material[] mats = Set(_gCode, _bCode, _bCodeDetail);

            source.materials = mats;
        }

        //private static Material[] Set(GraphicCode _gCode, BridgeCode _bCode, BridgeCodeDetail _bCodeDetail)
        private static Material[] Set(GraphicCode _gCode, string _name)
        {
            Material[] result = null;

            switch(_gCode)
            {
                case GraphicCode.Single_Color:
                    result = Set_SingleColor(_name);
                    break;

                case GraphicCode.Platform_Texturing:
                    result = Set_PlatformTexturing(_name);
                    break;

                default:
                    throw new Definition.Exceptions.GraphicCodeNotDefined(_gCode);
            }

            return result;
        }

        //private static Material[] Set_SingleColor(BridgeCode _bCode, BridgeCodeDetail _bCodeDetail)
        private static Material[] Set_SingleColor(string _name)
        {
            List<Material> result = new List<Material>();

            result.Add(Resources.Load<Material>($"Projects/Tunnel/Materials/Default"));

            return result.ToArray();
        }

        //private static Material[] Set_PlatformTexturing(BridgeCode _bCode, BridgeCodeDetail _bCodeDetail)
        private static Material[] Set_PlatformTexturing(string _name)
        {
            List<Material> result = new List<Material>();

            string basePath = "Projects/Bridge/Materials";
            //string AddPath = "";

            List<string> mList = new List<string>();
            List<string> spls = _name.Split(',').ToList();

            // 문자열 분할 2개 이상일때
            if(spls.Count > 1)
            {
                // 2번째 요소부터 쭉 mList에 넣기
                mList.AddRange(spls.GetRange(1, spls.Count - 1));
            }

            mList.ForEach(x =>
            {
                result.Add(Resources.Load<Material>($"{basePath}/{x[0]}/{x}"));
            });

            //return Set_SingleColor(_name);
            return result.ToArray();
        }
    }
}
