using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition
{
    /// <summary>
    /// � ���������� �ľ��ϴ� �ڵ�
    /// </summary>
    public static class _Items
    {
        /// <summary>
        /// �� �������� ������ ����ΰ�?
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
        
        // �ӽ� ������ �����ϰ� �Ҵ��Ѵ�.
        public static GameObject CreateCachePin(RaycastHit _hit)
        {
            GameObject result = GameObject.CreatePrimitive(PrimitiveType.Cube);
            result.transform.position = _hit.point;
            result.transform.rotation = Quaternion.Euler(new Vector3(45, 45, 45));
            result.GetComponent<MeshRenderer>().material = Materials.Set(MaterialType.Issue_dmg);
            result.GetComponent<Collider>().enabled = false;

            return result;
        }

        public static void MoveCachePin(GameObject _obj, RaycastHit _hit)
        {
            _obj.transform.position = _hit.point;
        }
    }
}
