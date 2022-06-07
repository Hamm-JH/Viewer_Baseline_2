using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using Definition.Control;
    using Management;
    using Platform.Tunnel;
    using View;

    public partial class Element_Compass : AElement
    {
        private void Compass_prev(Interactable _setter)
        {
            // 오직 회전모드 변경

            GameObject obj = EventManager.Instance._SelectedObject;
            PlatformCode pCode = MainManager.Instance.Platform;
            int segmentIndex = 0;
            int totalCount = 0;

            if(Platforms.IsTunnelPlatform(pCode))
            {
                Transform segment = obj.transform.parent.parent;
                Transform root = segment.parent;
                totalCount = root.childCount;

                int _index = root.childCount;
                for (int i = 0; i < _index; i++)
                {
                    if(root.GetChild(i) == segment)
                    {
                        segmentIndex = i;
                        break;
                    }
                }

                // 처음
                if(segmentIndex == 0)
                {

                }
                // 마지막
                else if(segmentIndex == totalCount-1)
                {

                }
                // 처음 위치 또는 마지막 위치
                else
                {

                }

                // 찾은 위치 기반으로 이전인 경우 이전으로 (최종 이전이면 끝)
                // 찾은 위치 기반으로 이후인 경우 이후로 (최종 이후이면 끝)
            }
            
            int index = obj.transform.childCount;


            if (obj != null)
            {
                Cameras.SetCameraDOTweenPosition(MainManager.Instance.MainCamera, obj);
                Cameras.SetCameraMode(CameraModes.OnlyRotate);
            }
        }

        private void Compass_next(Interactable _setter)
        {

        }
    }
}
