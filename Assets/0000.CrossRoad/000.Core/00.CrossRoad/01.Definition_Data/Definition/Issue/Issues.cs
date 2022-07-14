using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
    using Bearroll.UltimateDecals;
    using ch.sycoforge.Decal;
    using View;

    public static partial class Issues
    {
        /// <summary>
        /// 손상 표시 형태
        /// </summary>
        public enum ResourceType
        {
            Cube,
            EasyDecal,
            UltimateDecal
        }

        /// <summary>
        /// 손상정보 생성
        /// </summary>
        /// <param name="_index">1 : 손상 데칼 사용 0 : 사용하지 않음</param>
        /// <param name="_webT">웹 요청코드 분류</param>
        /// <param name="_issue">가공된 손상정보</param>
        /// <returns>생성된 손상 객체</returns>
        /// <exception cref="System.Exception">손상 인덱스 정보 불일치시 오류 발생</exception>
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

        /// <summary>
        /// 큐브 형태의 손상정보를 생성한다.
        /// </summary>
        /// <param name="_webT">웹 요청코드 분류</param>
        /// <param name="_issue">가공된 손상정보</param>
        /// <returns>생성된 손상 객체</returns>
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

        /// <summary>
        /// 데칼 형태의 손상정보를 생성한다.
        /// </summary>
        /// <param name="_webT">웹 요청코드 분류</param>
        /// <param name="_issue">가공된 손상정보</param>
        /// <returns>생성된 손상 객체</returns>
        /// <exception cref="System.Exception">필요한 프리팹 객체가 없을시 오류 발생</exception>
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
            Issue_Selectable issueSelectable = obj.AddComponent<Issue_Selectable>();
            issueSelectable.Issue = _issue;


            Items.Item_waypoint wp;
            if(obj.TryGetComponent<Items.Item_waypoint>(out wp))
            {
                // 생성된 IssueSelectable을 각 waypoint에 할당한다.
                wp.IssueWayPoint.SetIssueSelectable(issueSelectable);

                // ui 관리를 위한 waypoint 셋업
                _issue.Waypoint = wp;

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

            if(decal)
            {
                decal.DecalMaterial = Materials.Set(mType);
            }
            return obj;
        }

        /// <summary>
        /// 데칼 중에 UltimateDecal을 사용해서 손상정보를 생성한다.
        /// </summary>
        /// <param name="_webT">웹 요청코드 분류</param>
        /// <param name="_issue">가공된 손상정보</param>
        /// <returns>생성된 손상정보</returns>
        /// <exception cref="System.Exception">필요한 프리팹 객체가 없을시 오류 발생</exception>
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
            if(decal)
            {
                decal.material = mat;
            }

            //UD_Manager.UpdateDecal(decal);
            return obj;
        }

        /// <summary>
        /// 주어진 정보에서 Material 타입을 반환한다.
        /// </summary>
        /// <param name="_rType">리소스 형태 정보</param>
        /// <param name="_wType">웹 요청코드 분류</param>
        /// <returns>Material 타입</returns>
        /// <exception cref="System.Exception">조건에 맞는 코드 없을시 오류 발생</exception>
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

        /// <summary>
        /// 주어진 정보에서 Texture 타입을 반환한다.
        /// </summary>
        /// <param name="_iCode">손상정보 타입</param>
        /// <returns>텍스처 타입</returns>
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
