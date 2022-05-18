using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
    using Bearroll.UltimateDecals;
    using ch.sycoforge.Decal;
    using View;

    public static class Issues
    {
        public enum ResourceType
        {
            Cube,
            EasyDecal,
            UltimateDecal
        }

        public static GameObject CreateIssue(int _index, WebType _webT, _Issue.Issue _issue)
        {
            if(_index == 0)
            {
                return CreateIssue_Cube(_webT, _issue);
            }
            else if(_index == 1)
            {
                return CreateIssue_EasyDecal(_webT, _issue);
                //return CreateIssue_UltimateDecal(_webT, _issue);
            }
            else
            {
                throw new System.Exception("Issue index not correct");
            }
        }

        private static GameObject CreateIssue_Cube(WebType _webT, _Issue.Issue _issue)
        {
            ResourceType rType = ResourceType.Cube;

            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = _issue.IssueOrderCode;
            obj.transform.position = _issue.PositionVector;
            obj.transform.rotation = Quaternion.Euler(_issue.RotationVector);
            //obj.transform.rotation = Quaternion.Euler(new Vector3(45, 45, 0));
            obj.AddComponent<Issue_Selectable>().Issue = _issue;

            MeshRenderer render;
            if(obj.TryGetComponent<MeshRenderer>(out render))
            {
                MaterialType mType = GetMaterialType(rType, _webT);

                TextureType tType = GetTextureType(_issue.IssueCode);

                render.material = Materials.Set(mType);
                Textures.Set(render, tType);
            }

            return obj;
        }

        private static GameObject CreateIssue_EasyDecal(WebType _webT, _Issue.Issue _issue)
        {
            ResourceType rType = ResourceType.EasyDecal;
            GameObject obj = null;

            if(!Prefabs.TrySet(PrefabType.EasyDecal, out obj))
            {
                throw new System.Exception("Prefab not existed");
            }

            EasyDecal decal = obj.GetComponent<EasyDecal>();

            obj.name = _issue.IssueOrderCode;
            obj.transform.position = _issue.PositionVector;
            obj.transform.rotation = Quaternion.Euler(_issue.RotationVector);
            obj.AddComponent<Issue_Selectable>().Issue = _issue;

            Items.Item_waypoint wp;
            if(obj.TryGetComponent<Items.Item_waypoint>(out wp))
            {
                if(_webT == WebType.Issue_Dmg)
                {
                    wp.SetColor(new Color(0xee / 255f, 0x57 / 255f, 0x30 / 255f, 1));
                }
                else if(_webT == WebType.Issue_Rcv)
                {
                    wp.SetColor(new Color(0x3e / 255f, 0xb0 / 255f, 0xc9 / 255f, 1));
                }
                else
                {
                    Debug.LogWarning("--- WebType이 dmg, rcv가 아님. ---");
                }
            }

            MaterialType mType = GetMaterialType(rType, _webT);

            TextureType tType = GetTextureType(_issue.IssueCode);

            Material mat = Materials.Set(mType);
            Textures.Set(mat, tType);
            decal.DecalMaterial = Materials.Set(mType);
            return obj;
        }

        private static GameObject CreateIssue_UltimateDecal(WebType _webT, _Issue.Issue _issue)
        {
            ResourceType rType = ResourceType.UltimateDecal;
            GameObject obj = null;
            
            if(!Prefabs.TrySet(PrefabType.UltimateDecal, out obj))
            {
                throw new System.Exception("Prefab not existed");
            }

            UltimateDecal decal = obj.GetComponent<UltimateDecal>();

            obj.name = _issue.IssueOrderCode;
            obj.transform.position = _issue.PositionVector;
            obj.transform.rotation = Quaternion.Euler(_issue.RotationVector);
            obj.AddComponent<Issue_Selectable>().Issue = _issue;

            MaterialType mType = GetMaterialType(rType, _webT);

            TextureType tType = GetTextureType(_issue.IssueCode);

            Material mat = Materials.Set(mType);
            Textures.Set(mat, tType);
            decal.material = mat;

            //UD_Manager.UpdateDecal(decal);
            return obj;
        }

        private static MaterialType GetMaterialType(ResourceType _rType, WebType _wType)
        {
            MaterialType mType;
            if(_rType == ResourceType.Cube)
            {
                switch (_wType)
                {
                    case WebType.Issue_Dmg:
                        mType = MaterialType.Issue_dmg;
                        break;

                    case WebType.Issue_Rcv:
                        mType = MaterialType.Issue_rcv;
                        break;

                    default:
                        mType = MaterialType.Issue_dmg;
                        break;
                }
            }
            else if(_rType == ResourceType.EasyDecal)
            {
                switch (_wType)
                {
                    case WebType.Issue_Dmg:
                        mType = MaterialType.EasyDecal_dmg;
                        break;

                    case WebType.Issue_Rcv:
                        mType = MaterialType.EasyDecal_rcv;
                        break;

                    default:
                        mType = MaterialType.EasyDecal_dmg;
                        break;
                }
            }
            else if(_rType == ResourceType.UltimateDecal)
            {
                switch(_wType)
                {
                    case WebType.Issue_Dmg:
                        mType = MaterialType.UltimateDecal_dmg;
                        break;

                    case WebType.Issue_Rcv:
                        mType = MaterialType.UltimateDecal_rcv;
                        break;

                    default:
                        mType = MaterialType.UltimateDecal_dmg;
                        break;
                }
            }
            else
            {
                throw new System.Exception("조건에 맞는 코드를 못받음");
            }


            return mType;
        }

        private static TextureType GetTextureType(_Issue.IssueCodes _iCode)
        {
            TextureType tType;
            switch(_iCode)
            {
                case _Issue.IssueCodes.Crack:
                    tType = TextureType.crack;
                    break;

                case _Issue.IssueCodes.Spalling:
                    tType = TextureType.bagli;
                    break;

                case _Issue.IssueCodes.Efflorescence:
                    tType = TextureType.baegtae;
                    break;

                case _Issue.IssueCodes.breakage:
                    tType = TextureType.damage;
                    break;

                default:
                    tType = TextureType.crack;
                    break;
            }

            return tType;
        }
    }
}
