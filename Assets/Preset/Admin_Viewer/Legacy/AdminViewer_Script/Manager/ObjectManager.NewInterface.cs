using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
	using MGS.UCamera;
	//using UnityTemplateProjects;

	public partial class ObjectManager : MonoBehaviour
	{
		#region Set Material
		private void SetObjMaterial(ViewSceneStatus _stat)
		{
			if(_stat == ViewSceneStatus.Ready)
			{
				// 1 준비상태
				SetTemplateToMaterial(mode: 0);
			}
			else if(_stat == ViewSceneStatus.ViewAllDamage || _stat == ViewSceneStatus.ViewPartDamage || _stat == ViewSceneStatus.ViewPart2R || _stat == ViewSceneStatus.ViewMaintainance)
			{
				// 2, 3, 4, 5 상태
				SetTemplateToMaterial(mode: 1);
			}
		}

		private void SetTemplateToMaterial(int mode)
		{
			List<GameObject> objs = Data.Viewer.Cache.Instance.models.Objects;

			foreach(GameObject _o in objs)
			{
				SetMaterial(_o.transform, mode);
			}
		}
		#endregion

		//private void SetSubCamera(ViewSceneStatus _stat, 
		//	AroundAlignCamera orbitCam, Transform orbitTarget,
		//	SimpleCameraController freeCam ,Camera camTarget, bool isStatRecursived = false)
		//{
		//	switch(_stat)
		//	{
		//		case ViewSceneStatus.Ready:				// 상태 1
		//		case ViewSceneStatus.ViewAllDamage:		// 상태 2
		//		case ViewSceneStatus.ViewMaintainance:  // 상태 5
		//			{
		//				// 상태 1, 2, 5는 키맵을 사용하지 않으므로 초기화 진행
		//				orbitCam.enabled = true;
		//				freeCam.enabled = false;
		//				MainManager.Instance.isRecursived = false;	// 재귀확인 초기화
		//			}
		//			break;

		//		case ViewSceneStatus.ViewPartDamage:	// 상태 3
		//		case ViewSceneStatus.ViewPart2R:        // 상태 4
		//			{
		//				// 현재 선택된 객체
		//				Transform tr = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform;

		//				// 상태 3, 4는 키맵 사용, recursived 조건에 따라 초기화 수행
		//				StartCoroutine(DoKeymapSetting(target: tr, orbitCam: orbitCam, orbitTarget: orbitTarget,
		//					freeCam: freeCam, camTarget: camTarget, isStatRecursived: isStatRecursived));
		//			}
		//			break;
		//	}
		//}


		//private IEnumerator DoKeymapSetting(Transform target, AroundAlignCamera orbitCam, Transform orbitTarget, 
		//	SimpleCameraController freeCam, Camera camTarget, bool isStatRecursived = false)
		//{
		//	// 재귀안됨 또는 리셋버튼 클릭 -> 궤도 카메라 이동
		//	if(isStatRecursived == false)
		//	{
		//		// 궤도카메라 위치점 할당
		//		Vector3 targetPos = default(Vector3);
		//		// 중심점 수집용 경계변수
		//		Bounds _bound = new Bounds();

		//		MeshRenderer renderer;
		//		if(target.TryGetComponent<MeshRenderer>(out renderer))
		//		{
		//			targetPos = MainManager.Instance.rootBounds.center;
		//			_bound = Data.Viewer.Cache.Instance.models.rootBound;
		//		}

		//		orbitCam.enabled = true;        // 궤도캠 활성화
		//		freeCam.enabled = false;        // 자유캠 비활성화

		//		yield return null;

		//		orbitTarget.position = targetPos;   // 1. 궤도캠의 줌심 위치 이동

		//		float maxValue = Mathf.Max(Mathf.Max(_bound.size.x, _bound.size.y), _bound.size.z);

		//		orbitCam.targetDistance = maxValue / 1.8f; // 2. 궤도캠의 거리 이동
		//	}
		//	// 재귀 상태 -> 자유캠 이동
		//	else
		//	{
		//		orbitCam.enabled = false;
		//		freeCam.enabled = false;

		//		// 목표 위치점 변수 선언
		//		Vector3 targetVector = default(Vector3);

		//		// 0. 선택 객체의 부모부모객체 수집 (Segment)
		//		Transform segment = target.parent.parent;

		//		// 1. 선택 객체의 부모객체 수집 (Line)
		//		Transform line = target.parent;

		//		// 객체 타입 확인
		//		Datas.ObjectType _objType = Utilities.NameParameter.GetMatParameter(target.name, 1);
		//		// 확인된 객체 타입에서 측면벽에 해당하면 아래 값을 변경
		//		float sideWallFactor = Utilities.TunnelObject.SideWallRange(_objType);
		//		// 시작부재인지 아닌지 확인해서 합산값의 +- 부호를 지정
		//		int isStart = Utilities.TunnelObject.IsStartSegment(target.name);

		//		yield return null;

		//		// 2. 부모 객체에서 일정 거리만큼 이동위치 수집
		//		camTarget.transform.position = 
		//			line.position + new Vector3(0, 1.6f, 0) + 
		//			line.TransformDirection(Vector3.right) * -5 +
		//			line.TransformDirection(Vector3.right) * isStart * sideWallFactor;

		//		//Debug.Log($"recursived, segment name : {segment.name}");
		//		//Debug.Log($"recursived, line name : {line.name}");

		//		// 모든 작업 시행 후 변수 초기화
		//		MainManager.Instance.isRecursived = false;  // 재귀확인 초기화

		//		orbitCam.enabled = false;
		//		freeCam.enabled = true;
		//	}
		//	yield break;
		//}
	}
}