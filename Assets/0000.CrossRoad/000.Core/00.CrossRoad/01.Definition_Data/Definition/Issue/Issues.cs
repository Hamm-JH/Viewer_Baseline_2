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
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = _issue.IssueOrderCode;
            obj.transform.position = _issue.PositionVector;
            obj.transform.rotation = Quaternion.Euler(_issue.RotationVector);
            //obj.transform.rotation = Quaternion.Euler(new Vector3(45, 45, 0));
            obj.AddComponent<Issue_Selectable>().Issue = _issue;

            MeshRenderer render;
            if(obj.TryGetComponent<MeshRenderer>(out render))
            {
                MaterialType mType;
                switch(_webT)
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

                // TODO 0511 : 손상 변환테이블 만들고 추후 적용
                TextureType tType;
                switch(_issue.IssueCode)
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

                render.material = Materials.Set(mType);
                Textures.Set(render, tType);
            }

            return obj;
        }

        private static GameObject CreateIssue_EasyDecal(WebType _webT, _Issue.Issue _issue)
        {
            GameObject obj = null;

            if(!Prefabs.TrySet(PrefabType.EasyDecal, out obj))
            {
                throw new System.Exception("Prefab not existed");
            }

            EasyDecal decal = obj.GetComponent<EasyDecal>();

            obj.name = _issue.IssueOrderCode;
            obj.transform.position = _issue.PositionVector;
            obj.transform.rotation = Quaternion.Euler(_issue.RotationVector); // TODO 각도 초기화 코드 적용
            //obj.transform.rotation = Quaternion.Euler(new Vector3(45, 45, 45)); // TODO 각도 초기화 코드 적용
            obj.AddComponent<Issue_Selectable>().Issue = _issue;

            MaterialType mType;
            switch (_webT)
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

            TextureType tType;
            switch (_issue.IssueCode)
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

            Material mat = Materials.Set(mType);
            Textures.Set(mat, tType);
            decal.DecalMaterial = Materials.Set(mType);
            return obj;
        }

        private static GameObject CreateIssue_UltimateDecal(WebType _webT, _Issue.Issue _issue)
        {
            GameObject obj = null;
            
            if(!Prefabs.TrySet(PrefabType.UltimateDecal, out obj))
            {
                throw new System.Exception("Prefab not existed");
            }

            UltimateDecal decal = obj.GetComponent<UltimateDecal>();

            obj.name = _issue.IssueOrderCode;
            obj.transform.position = _issue.PositionVector;
            obj.transform.rotation = Quaternion.Euler(_issue.RotationVector);
            //obj.transform.rotation = Quaternion.Euler(new Vector3(45, 45, 0));
            obj.AddComponent<Issue_Selectable>().Issue = _issue;

            //MeshRenderer render;
            //if(obj.TryGetComponent<MeshRenderer>(out render))
            //{
                

            //    render.material = Materials.Set(mType);
            //    Textures.Set(render, tType);
            //}

            MaterialType mType;
            switch (_webT)
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

            // TODO 0511 : 손상 변환테이블 만들고 추후 적용
            TextureType tType;
            switch (_issue.IssueCode)
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

            Material mat = Materials.Set(mType);
            Textures.Set(mat, tType);
            decal.material = mat;
            //UD_Manager.Restart();
            UD_Manager.UpdateDecal(decal);
            return obj;
        }
    }
}
