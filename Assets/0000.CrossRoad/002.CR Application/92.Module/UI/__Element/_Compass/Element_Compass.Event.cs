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
            // ���� ȸ����� ����

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

                // ó��
                if(segmentIndex == 0)
                {

                }
                // ������
                else if(segmentIndex == totalCount-1)
                {

                }
                // ó�� ��ġ �Ǵ� ������ ��ġ
                else
                {

                }

                // ã�� ��ġ ������� ������ ��� �������� (���� �����̸� ��)
                // ã�� ��ġ ������� ������ ��� ���ķ� (���� �����̸� ��)
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
