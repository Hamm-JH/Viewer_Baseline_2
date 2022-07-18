using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
    using Bearroll.UltimateDecals;
    using ch.sycoforge.Decal;
    using Items;

    /// <summary>
    /// 어떤 아이템인지 파악하는 코드
    /// </summary>
    public static class _Items
    {
        /// <summary>
        /// 이 아이템은 아이템 요소인가?
        /// </summary>
        /// <param name="_item"></param>
        /// <returns></returns>
        public static bool IsLocationElement(GameObject _item)
        {
            LocationElement tg;
            if(_item.TryGetComponent<LocationElement>(out tg))
            {
                return true;
            }

            return false;
        }
        
        // 임시 변수를 생성하고 할당한다.
        public static GameObject CreateCachePin(RaycastHit _hit, bool _isDecal)
        {
            GameObject result = null;

            if(_isDecal)
            {
                result = CreateCachePin_EasyDecal(_hit, _isDecal);
                //result = CreateCachePin_UltimateDecal(_hit, _isDecal);

                //if (!Prefabs.TrySet(PrefabType.UltimateDecal, out result))
                //{
                //    throw new System.Exception("Prefab not existed");
                //}

                //UltimateDecal decal = result.GetComponent<UltimateDecal>();

                //result.transform.position = _hit.point;
                //result.transform.rotation = Quaternion.FromToRotation(result.transform.up, _hit.normal) * _hit.collider.gameObject.transform.rotation;
                ////decal.material = Materials.Set(MaterialType.Decal);
                ////result.GetComponentInParent<MeshRenderer>().material = Materials.Set(MaterialType.Decal);
                //result.GetComponent<Collider>().enabled = false;
                ////UD_Manager.AddDecal(decal);
                //UD_Manager.UpdateDecal(decal);
            }
            else
            {
                result = CreateCachePin_Cube(_hit, _isDecal);
                //result = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //result.transform.position = _hit.point;
                //result.transform.rotation = Quaternion.Euler(new Vector3(45, 45, 45));
                //result.GetComponent<MeshRenderer>().material = Materials.Set(MaterialType.Issue_dmg);
                //result.GetComponent<Collider>().enabled = false;
            }

            return result;
        }

        /// <summary>
        /// POI객체를 EasyDecal 모드로 생성
        /// </summary>
        /// <param name="_hit">충돌한 객체</param>
        /// <param name="_isDecal">데칼 포함여부</param>
        /// <returns></returns>
        private static GameObject CreateCachePin_EasyDecal(RaycastHit _hit, bool _isDecal)
        {
            GameObject result = null;

            if(!Prefabs.TrySet(PrefabType.EasyDecal, out result))
            {
                throw new System.Exception("Prefab not existed");
            }

            EasyDecal decal = result.GetComponent<EasyDecal>();

            result.transform.position = _hit.point;
            result.transform.rotation = Quaternion.FromToRotation(Vector3.up, _hit.normal) * _hit.collider.gameObject.transform.rotation;
            result.GetComponent<Collider>().enabled = false;
            result.transform.Translate(Vector3.up * 0.01f);

            return result;
        }

        /// <summary>
        /// POI 객체를 UltimateDecal 모드로 생성
        /// </summary>
        /// <param name="_hit">충돌한 객체</param>
        /// <param name="_isDecal">데칼 포함여부</param>
        /// <returns></returns>
        private static GameObject CreateCachePin_UltimateDecal(RaycastHit _hit, bool _isDecal)
        {
            GameObject result = null;

            if (!Prefabs.TrySet(PrefabType.UltimateDecal, out result))
            {
                throw new System.Exception("Prefab not existed");
            }

            UltimateDecal decal = result.GetComponent<UltimateDecal>();

            result.transform.position = _hit.point;
            result.transform.rotation = Quaternion.FromToRotation(result.transform.up, _hit.normal) * _hit.collider.gameObject.transform.rotation;
            result.GetComponent<Collider>().enabled = false;
            UD_Manager.UpdateDecal(decal);

            return result;
        }

        /// <summary>
        /// POI 객체를 일반 정육면체 형태로 생성
        /// </summary>
        /// <param name="_hit">충돌한 객체</param>
        /// <param name="_isDecal">데칼 포함여부</param>
        /// <returns></returns>
        private static GameObject CreateCachePin_Cube(RaycastHit _hit, bool _isDecal)
        {
            GameObject result = null;

            result = GameObject.CreatePrimitive(PrimitiveType.Cube);
            result.transform.position = _hit.point;
            result.transform.rotation = Quaternion.Euler(new Vector3(45, 45, 45));
            result.GetComponent<MeshRenderer>().material = Materials.Set(MaterialType.Issue_dmg);
            result.GetComponent<Collider>().enabled = false;

            return result;
        }

        /// <summary>
        /// POI 객체를 이동한다.
        /// </summary>
        /// <param name="_obj">선택된 객체</param>
        /// <param name="_hit">마우스 클릭 위치</param>
        /// <param name="_isDecal">데칼 요소인가?</param>
        public static void MoveCachePin(GameObject _obj, RaycastHit _hit, bool _isDecal)
        {
            if(_isDecal)
            {
                _obj.transform.position = _hit.point;
                _obj.transform.rotation = Quaternion.FromToRotation(_obj.transform.up, _hit.normal) * _hit.collider.gameObject.transform.rotation;
            }
            else
            {
                _obj.transform.position = _hit.point;
            }
        }
    }
}
