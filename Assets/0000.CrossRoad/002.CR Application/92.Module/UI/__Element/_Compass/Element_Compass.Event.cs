using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using Definition.Control;
    using Management;
    using Module.Model;
    using Platform.Tunnel;
    using View;

    public partial class Element_Compass : AElement
    {
        private void Compass_prev(Interactable _setter)
        {
            // 오직 회전모드 변경
            // index == -9 첫 위치로
            // index == -1 대기 (첫 위치에 있으므로)
            // index == max 이전 위치로
            // index < max 이전 위치로
            // else :: 에러

            Transform target = null;
            PlatformCode pCode = MainManager.Instance.Platform;

            if (Platforms.IsTunnelPlatform(pCode))
            {
                //Vector3 toPos;
                //Quaternion toRot;

                //Debug.Log($"index : {m_data.index}");
                if (!TrySet_PrevIndexTarget(ref m_data.index, out target, out Vector3 toPos, out Quaternion toRot)) return;

                
                //Debug.Log("----------------- prev");
                //Debug.Log($"target : {target.name}");

                //Debug.Log($"index : {m_data.index}");
                //Debug.Log($"toPos : {toPos}");
                //Debug.Log($"toRot : {toRot}");

                //Debug.Log("-----------------");
                
                

                //Cameras.SetCameraDOTweenPosition(MainManager.Instance.MainCamera, target.gameObject);
                //Cameras.SetCameraDOTweenPosition_Compass(MainManager.Instance.MainCamera, target.gameObject);
                Cameras.SetCameraDOTweenPosition_Compass(MainManager.Instance.MainCamera, toPos, toRot);
                Cameras.SetCameraMode(CameraModes.OnlyRotate);
            }
        }

        private void Compass_next(Interactable _setter)
        {
            Transform target = null;
            PlatformCode pCode = MainManager.Instance.Platform;
            
            if (Platforms.IsTunnelPlatform(pCode))
            {
                //Debug.Log($"index : {m_data.index}");
                if (!TrySet_NextIndexTarget(ref m_data.index, out target, out Vector3 toPos, out Quaternion toRot)) return;

                //Debug.Log("----------------- next");
                //Debug.Log($"target : {target.name}");

                //Debug.Log($"index : {m_data.index}");
                //Debug.Log($"toPos : {toPos}");
                //Debug.Log($"toRot : {toRot}");

                //Debug.Log("-----------------");


                //Cameras.SetCameraDOTweenPosition(MainManager.Instance.MainCamera, target.gameObject);
                Cameras.SetCameraDOTweenPosition_Compass(MainManager.Instance.MainCamera, toPos, toRot);
                Cameras.SetCameraMode(CameraModes.OnlyRotate);
            }
        }

        private bool TrySet_PrevIndexTarget(ref int _currentIndex, out Transform _target, out Vector3 _toPos, out Quaternion _toRot)
        {
            bool result = false;

            _target = null;
            _toPos = default(Vector3);
            _toRot = default(Quaternion);

            Module_Model module_Model = ContentManager.Instance.Module<Module_Model>();
            GameObject rootObj = module_Model.Model;

            Transform first = module_Model.Trs_tunnel[0];
            Transform last = module_Model.Trs_tunnel[1];
            int maxIndex = rootObj.transform.childCount;

            //Debug.Log($"prev");


            if (_currentIndex == -9)
            {
                _currentIndex = -1;
                _target = first;
                _toPos = _target.position;
                _toRot = _target.rotation;
                result = true;
            }
            else if (_currentIndex == -1)
            {
                // 대기
                result = false;
            }
            else if (_currentIndex == 0)
            {
                _currentIndex = -1;
                _target = first;
                _toPos = _target.position;
                _toRot = _target.rotation;
                result = true;
            }
            else if (_currentIndex == maxIndex)
            {
                Debug.Log($"prev");
                // 이전 인덱스로 이동
                // 이동 개체 잡는게 다름
                _currentIndex--;

                Transform from = rootObj.transform.GetChild(_currentIndex);
                Transform to = rootObj.transform.GetChild(_currentIndex - 1);

                _target = rootObj.transform.GetChild(_currentIndex);
                _toPos = new Vector3(_target.position.x, _target.position.y + 2, _target.position.z);

                Vector3 euler = from.rotation.eulerAngles;
                _toRot = Quaternion.Euler(0, euler.y + 90, 0);
                result = true;
            }
            else if (_currentIndex < maxIndex)
            {
                Debug.Log($"4");
                // 이전 인덱스로 이동
                _currentIndex--;

                Transform from = rootObj.transform.GetChild(_currentIndex);
                Transform to = _currentIndex - 1 < 0 ? first : rootObj.transform.GetChild(_currentIndex - 1);

                _target = rootObj.transform.GetChild(_currentIndex);
                _toPos = new Vector3(_target.position.x, _target.position.y + 2, _target.position.z);

                Vector3 euler = from.rotation.eulerAngles;
                _toRot = Quaternion.Euler(0, euler.y + 90, 0);
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        private bool TrySet_NextIndexTarget(ref int _currentIndex, out Transform _target, out Vector3 _toPos, out Quaternion _toRot)
        {
            bool result = false;

            _target = null;
            _toPos = default(Vector3);
            _toRot = default(Quaternion);

            Module_Model module_Model = ContentManager.Instance.Module<Module_Model>();
            GameObject rootObj = module_Model.Model;

            Transform first = module_Model.Trs_tunnel[0];
            Transform last = module_Model.Trs_tunnel[1];
            int maxIndex = rootObj.transform.childCount;

            if (_currentIndex == -9)
            {
                _currentIndex = maxIndex;
                _target = last;
                _toPos = _target.position;
                _toRot = _target.rotation;
                result = true;
            }
            else if (_currentIndex == -1)
            {
                _currentIndex++;

                Transform from = rootObj.transform.GetChild(_currentIndex);
                Transform to = rootObj.transform.GetChild(_currentIndex + 1);

                _target = rootObj.transform.GetChild(_currentIndex);
                _toPos = new Vector3(_target.position.x, _target.position.y + 2, _target.position.z);

                Vector3 euler = from.rotation.eulerAngles;
                _toRot = Quaternion.Euler(0, euler.y - 90, 0);
                result = true;
            }
            else if (_currentIndex == maxIndex - 1)
            {
                _currentIndex++;
                _target = last;
                _toPos = _target.position;
                _toRot = _target.rotation;
                result = true;
            }
            else if (_currentIndex == maxIndex)
            {
                result = false;
            }
            else if (_currentIndex < maxIndex)
            {
                _currentIndex++;

                Transform from = rootObj.transform.GetChild(_currentIndex);
                Transform to = _currentIndex + 1 == maxIndex ? last : rootObj.transform.GetChild(_currentIndex + 1);

                _target = rootObj.transform.GetChild(_currentIndex);
                _toPos = new Vector3(_target.position.x, _target.position.y + 2, _target.position.z);

                Vector3 euler = from.rotation.eulerAngles;
                _toRot = Quaternion.Euler(0, euler.y - 90, 0);
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }
    }
}
