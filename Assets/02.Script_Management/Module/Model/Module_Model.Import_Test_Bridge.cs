using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Model
{
	using View;
	using System.Linq;
	using Management;

	/// <summary>
	/// 템플릿
	/// </summary>
	public partial class Module_Model : AModule
	{
		[Header("Bridge")]
		[SerializeField] GameObject m_bRootObj;
		[SerializeField] GameObject m_bRoot3D;
		[SerializeField] GameObject[] m_bridgeTopParts;
		[SerializeField] GameObject[] m_bridgeBottomParts;

		public void InitializeObjectBridge(GameObject _root)
		{
			GameObject dim = null;

			m_bRootObj = _root.transform.GetChild(0).gameObject;
			m_bRoot3D = null;

			int index = m_bRootObj.transform.childCount;
			for (int i = 0; i < index; i++)
			{
				string name = m_bRootObj.transform.GetChild(i).name;
				switch (name)
				{
					case "RA":
					case "RCT":
					case "PSCI":
					case "RCS":
						m_bRoot3D = m_bRootObj.transform.GetChild(i).gameObject;
						break;

					default:
						dim = m_bRootObj.transform.GetChild(i).gameObject;
						break;
				}
			}

			// 치수선 삭제
			Destroy(dim);

			StartCoroutine(Initialize3DObject(m_bRoot3D));

			
		}

		private IEnumerator Initialize3DObject(GameObject _root3DObject)
		{
			//manager.RootObject.transform.parent.position = new Vector3(0, 0, 0);

			// 지역 리스트 초기화
			List<GameObject> bridgeTopParts = new List<GameObject>();
			List<GameObject> bridgeBottomParts = new List<GameObject>();


			// 지역 리스트에 요소 할당
			int index = _root3DObject.transform.childCount;
			for (int i = 0; i < index; i++)
			{
				// todo 0222
				if (_root3DObject.transform.GetChild(i).name.Substring(0, 2) == "SP")
				{
					bridgeTopParts.Add(_root3DObject.transform.GetChild(i).gameObject);
				}
				else if (_root3DObject.transform.GetChild(i).name.Substring(0, 2) == "AP")
				{
					bridgeBottomParts.Add(_root3DObject.transform.GetChild(i).gameObject);
				}
			}

			//// 리스트 전역 배열 변환
			m_bridgeTopParts = (from obj in bridgeTopParts orderby obj.name select obj).ToArray<GameObject>();
			m_bridgeBottomParts = (from obj in bridgeBottomParts orderby obj.name select obj).ToArray<GameObject>();

			//// 다음 메서드 실행
			SetObjectsMaterialNCollider(_root3DObject);

			yield return new WaitForEndOfFrame();

			ContentManager.Instance.SetCameraCenterPosition();

			ContentManager.Instance.CompCheck(4);

			yield break;
		}

		private void SetObjectsMaterialNCollider(GameObject _root3DObject)
		{
			#region 변수 선언부
			// 3D 객체 아래의 모든 자식객체 수집
			Transform[] objTransforms = _root3DObject.gameObject.GetComponentsInChildren<Transform>();

			// 자식객체 중에 조건에 맞는 (material을 가진 객체의 조건) 객체 수집
			Transform[] selected_objTransforms = (from obj in objTransforms where obj.name.Split(',').Length > 1 select obj).ToArray<Transform>();

			// 선택 가능한 객체 모아둠
			ModelObjects = new List<GameObject>();

			Dictionary<string, int> multiSplitIndex = new Dictionary<string, int>();
			#endregion

			#region 모든 객체의 material, collider를 할당한다.

			// Material을 할당해야 하는 객체에 Material, Collider 할당
			// [반복] material을 가진 모든 자식객체
			int index1 = selected_objTransforms.Length;
			for (int i = 0; i < index1; i++)
			{
				// 현재 index 객체의 문자열 인자값 할당
				string[] splitObjectString = selected_objTransforms[i].name.Split(',');

				string name = splitObjectString[0];

				// [조건] 분할 문자열이 2개 : (객체명 문자열 1, material 정보 1) 객체가 한 개의 Material을 필요로 하는 경우
				if (splitObjectString.Length == 2)
				{
					selected_objTransforms[i].GetComponent<MeshRenderer>().material = Materials.Set(MaterialType.Default);

					selected_objTransforms[i].gameObject.AddComponent<MeshCollider>().convex = true;
					selected_objTransforms[i].gameObject.AddComponent<Obj_Selectable>();
					ModelObjects.Add(selected_objTransforms[i].gameObject);
				}
				// [조건] 분할 문자열이 2개 이상: (객체명 문자열 1, material 정보 n) 객체가 여러 개의 Material을 필요로 하는 경우
				else
				{
					// Dictionary에 확인
					int stringIndex = 0;

					// [조건] Dictionary에 객체명 키값 검색시 키가 있는 경우 : 첫 번째 객체가 아닌 경우
					if (multiSplitIndex.TryGetValue(splitObjectString[0], out stringIndex).Equals(true))
					{
						multiSplitIndex[splitObjectString[0]]++;

						selected_objTransforms[i].GetComponent<MeshRenderer>().material = Materials.Set(MaterialType.Default);

						selected_objTransforms[i].gameObject.AddComponent<MeshCollider>().convex = true;
						selected_objTransforms[i].gameObject.AddComponent<Obj_Selectable>();
						ModelObjects.Add(selected_objTransforms[i].gameObject);
					}
					// 해당 키값이 없으면 첫 번째 검색된 객체임
					else
					{
						// Dictionary 삭제 (의도치 않은 오류가 발생해서 Dictionary 키를 한번씩 날려버리는 코드 작성)
						// 교각의 받침 02 03 라인이 2번째 교각에 있을때
						// 교각의 받침 03 04 라인이 3번째 교각에 있다. 이때 03 라인의 이름이 중복되는 문제 발생
						multiSplitIndex.Clear();

						// Dictionary에 새 키 할당
						multiSplitIndex.Add(splitObjectString[0], stringIndex);
						multiSplitIndex[splitObjectString[0]]++;

						selected_objTransforms[i].GetComponent<MeshRenderer>().material = Materials.Set(MaterialType.Default);

						selected_objTransforms[i].gameObject.AddComponent<MeshCollider>().convex = true;
						selected_objTransforms[i].gameObject.AddComponent<Obj_Selectable>();
						ModelObjects.Add(selected_objTransforms[i].gameObject);
					}
				}

				// 객체에 Meshrenderer가 존재할 경우 Meshrenderer 관리 리스트에 Meshrenderer 할당
				//MeshRenderers.Add(selected_objTransforms[i].GetComponent<MeshRenderer>());
			}

			multiSplitIndex.Clear();

			#endregion


		}
	}
}
