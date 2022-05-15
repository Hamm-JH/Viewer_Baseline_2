using Bearroll.UltimateDecals;
using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
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
                if(!Prefabs.TrySet(PrefabType.Decal, out result))
                {
                    throw new System.Exception("Prefab not existed");
                }

                UltimateDecal decal = result.GetComponent<UltimateDecal>();

                result.transform.position = _hit.point;
                result.transform.rotation = Quaternion.FromToRotation(result.transform.up, _hit.normal) * _hit.collider.gameObject.transform.rotation;
                //decal.material = Materials.Set(MaterialType.Decal);
                //result.GetComponentInParent<MeshRenderer>().material = Materials.Set(MaterialType.Decal);
                result.GetComponent<Collider>().enabled = false;
                //UD_Manager.AddDecal(decal);
                UD_Manager.UpdateDecal(decal);
            }
            else
            {
                result = GameObject.CreatePrimitive(PrimitiveType.Cube);
                result.transform.position = _hit.point;
                result.transform.rotation = Quaternion.Euler(new Vector3(45, 45, 45));
                result.GetComponent<MeshRenderer>().material = Materials.Set(MaterialType.Issue_dmg);
                result.GetComponent<Collider>().enabled = false;
            }

            return result;
        }

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
