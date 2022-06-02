using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Module.UI
{
    using Definition;
    using View;
    using Management;

    public partial class Element_BottomBar : AElement
    {
        private void Zoom_in(Interactable _setter)
        {
            Zoom(1);
        }

        private void Zoom_out(Interactable _setter)
        {
            Zoom(-1);
        }

        private void Zoom(int index)
        {
            // index :: 1 줌인
            // index ;; -1 줌아웃

            float maxDistance = ContentManager.Instance.generalSettings.MaxCameraDistance;

            GameObject selected = EventManager.Instance._SelectedObject;
            Transform camTransform = MainManager.Instance.MainCamera.transform;
            Vector3 camPos = camTransform.position;
            Vector3 camAngle = camTransform.rotation.eulerAngles;

            Vector3 targetPos = default(Vector3);
            if (selected != null)
            {
                targetPos = selected.transform.position;
            }
            else
            {
                targetPos = default(Vector3);
            }

            //float camToTargetDistance = Vector3.Distance(camPos, targetPos);

            //Vector3 directionNormal = Vector3.Cross(camPos, targetPos);

            Vector3 heading = targetPos - camPos;
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;

            //direction = direction * camAngle;

            // 이동거리 총 길이 / 10
            float range = (float)(maxDistance / 10);

            // 현재 거리에서 근접시 0 아래로 내려가면
            if (distance - (range * index) <= 0)
            {
                range = 0;
            }

            // 거리 값에서 이동거리를 index에 따라 +-
            // 거리값을 최소 최대값에 보간한다.
            //range = Mathf.Clamp(distance + index * range, 0, maxDistance);

            //DG.Tweening.DOTween

            Vector3 targetVector = direction * (range * index);

            Vector3 targetPosition = new Vector3(
                camTransform.position.x + targetVector.x,
                camTransform.position.y + targetVector.y,
                camTransform.position.z + targetVector.z
                );

            camTransform.DOMove(targetPosition, 0.5f);
            //camTransform.Translate(direction * (range * index), Space.World);
        }
    }
}
