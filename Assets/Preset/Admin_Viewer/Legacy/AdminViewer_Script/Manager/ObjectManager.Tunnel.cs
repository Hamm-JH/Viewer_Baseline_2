using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data.Viewer;
using Cache = Data.Viewer.Cache;
using System.Linq;

namespace Manager
{
	public partial class ObjectManager : MonoBehaviour
	{
		[Header("_Tunnel")]
		public GameObject trashCan;

		public void InitializeObjectTunnel(GameObject _root)
		{
			StartCoroutine(ModelInitialize(_root));
		}

		private IEnumerator ModelInitialize(GameObject root)
		{
			//yield break;
			yield return null;

			// 1. 모델의 의미없는 메쉬 삭제
			// 2. 분할 서브메쉬의 경우 메이커에서 서브메쉬 가지고 있는 모델 가지고 데이터 까보기
			// 3. 서브메쉬 할당 필요한 모델을 메쉬병합
			// 모델 캐시데이터 할당, 라인 객체 생성, 개별 객체 라인객체에 배치
			SetModelCache(root.transform);

			//yield break;
			yield return new WaitForEndOfFrame();

			ControlSubMesh(Cache.Instance.models.Segments);

			//yield break;
			yield return new WaitForEndOfFrame();

			yield return new WaitForEndOfFrame();

			RuntimeData.RootContainer.Instance.isObjectRoutineEnd = true;
			//Debug.LogError("1103 치수선 객체 배치 준비");
			RuntimeData.RootContainer.Instance.isIssueRoutineEnd = true;

			MainManager.Instance.InitializeRoutineCheck();
			//yield break;
			//RemoveTrashCan();

			//yield break;
			//RemoveTrashCan();

		}

		#region 1-1. 모델 캐시데이터 할당

		/// <summary>
		/// 모델 캐시 데이터 할당
		/// </summary>
		/// <param name="root"></param>
		private void SetModelCache(Transform root)
		{
			//Debug.Log(root.transform.name); // TunnelRoot
			Cache.Instance.models.Gltf_root = root.gameObject;

			int index = root.childCount;
			for (int i = 0; i < index; i++)
			{
				//Debug.Log(root.GetChild(i).name); // Tunnel CodeName :: Ex) 20211118-00000001
				SetSingleTunnel(root.GetChild(i));
			}
		}

		/// <summary>
		/// 모델 루트 캐시 데이터 할당
		/// </summary>
		/// <param name="rootModel"></param>
		private void SetSingleTunnel(Transform rootModel)
		{
			//Debug.Log(rootModel.name); // Tunnel CodeName :: Ex) 20211118-00000001
			Cache.Instance.models.ModelName = rootModel.gameObject;

			int index = rootModel.childCount;
			for (int i = 0; i < index; i++)
			{
				//Debug.Log(rootModel.GetChild(i).name); // Segment 1 2 3...

				SetLines(rootModel.GetChild(i), i+1);
			}
		}

		/// <summary>
		/// 세그먼트 레벨 캐시 데이터 할당 // 라인 객체 재생성 // 객체 레벨 재배치
		/// </summary>
		/// <param name="tunnel"></param>
		/// <param name="tunnelIndex"></param>
		private void SetLines(Transform tunnel, int tunnelIndex)
		{
			//Debug.Log(tunnel.name); // Segment 1 2 3...
			Cache.Instance.models.Segments.Add(tunnel.gameObject);
			string segmentCount = tunnelIndex.ToString();
			//string segmentCount = GetSegmentCount(tunnel);

			//Debug.Log(segmentCount);

			// 단위 라인 인덱스를 모아두는 리스트
			List<int> indexList = new List<int>();
			Dictionary<int, Transform> lineList = new Dictionary<int, Transform>();

			// 단위 객체를 모아두는 변수
			Dictionary<int, List<Transform>> lineObjs = new Dictionary<int, List<Transform>>();

			#region 1 Set indexList
			// 1-1 라인 인덱스를 모두 수집한다.
			int index = tunnel.childCount;
			for (int i = 0; i < index; i++)
			{
				int _index = GetObjLineIndex(tunnel.GetChild(i));
				if(_index != 0)
				{
					indexList.Add(GetObjLineIndex(tunnel.GetChild(i)));
				}
			}

			// 1-2 중복 제거로 단위 라인 인덱스 리스트 생성
			indexList = indexList.Distinct().ToList();
			#endregion

			#region 2 line별 객체 선별
			// 2-1 라인 인덱스 기반으로 같이 넘어온 라인 객체를 수집한다.
			index = indexList.Count;
			for (int i = 0; i < index; i++)
			{
				string lineName = string.Format("{0},{1}", segmentCount, indexList[i]);
				//Debug.Log($"lineName : {lineName}");

				Transform toFind = tunnel.Find(lineName);
				toFind.localScale = new Vector3(1, 1, 1);
				Destroy(toFind.GetComponent<MeshRenderer>());
				Destroy(toFind.GetComponent<MeshFilter>());
				//Debug.Log($"found line name : {toFind.name}");	// 라인 객체 이름 1,1 2,1 ...

				lineList.Add(indexList[i], toFind);
			}

			// 2-2 lineObjs 사전 변수 설정
			index = tunnel.childCount;
			for (int i = 0; i < index; i++)
			{
				//Debug.Log(tunnel.GetChild(i).name); // 개별 객체 (line 객체 [1,1 1,2...] 포함)
				int lineIndex = GetObjLineIndex(tunnel.GetChild(i));    // 라인 카운트 할당

				string lineName = string.Format("{0},{1}", segmentCount, lineIndex);

				if (tunnel.GetChild(i).name != lineName)
				{
					//Debug.Log("not correct");	// 라인 객체가 아님 (모델 객체들임)
					Data.Viewer.Cache.Instance.models.Objects.Add(tunnel.GetChild(i).gameObject);   // On/Off용 리스트에 배치

					// lineIndex key가 이미 할당된 경우
					if (lineObjs.ContainsKey(lineIndex))
					{
						lineObjs[lineIndex].Add(tunnel.GetChild(i));
					}
					// lineIndex key가 없는 경우
					else
					{
						List<Transform> list = new List<Transform>();
						list.Add(tunnel.GetChild(i));

						lineObjs.Add(lineIndex, list);
					}
				}
				else
				{
					//Debug.Log("correct");	// 라인 객체임
				}
			}
			#endregion

			#region 3 라인 객체와 라인 단위 객체 순회하면서 단위 객체 할당
			// indexList를 순회하면서 
			foreach (int key in lineList.Keys)
			{
				//Debug.Log($"line key : {key}");     // 라인 키값
													//lineList[key];	// 라인 객체

				// 라인 컨트롤 리스트 배치
				Data.Viewer.Cache.Instance.models.Lines.Add(lineList[key].gameObject);

				// 라인 키값 단위의 개별 객체 순회
				foreach (Transform obj in lineObjs[key])
				{
					// obj;		// 라인 내부 개별객체

					obj.SetParent(lineList[key]);   // 개별 객체 부모할당
				}
			}
			#endregion

			{
				//#region Obj 레벨 Line 레벨 부모 할당
				//foreach (int key in lineObjs.Keys)
				//{
				//	// tunnelIndex, lineIndex 조합해 Line 객체 생성
				//	// 부모객체 tunnel로 설정
				//	// 같은 키 안에 있는 객체들 부모 생성한 Line 객체로 설정

				//	string lineName = $"{tunnelIndex},{key}";

				//	GameObject newLine = new GameObject(lineName);
				//	newLine.transform.position = tunnel.position;       // 터널 위치 동기화
				//	newLine.transform.localScale = tunnel.localScale;   // 터널 스케일 동기화
				//	newLine.transform.SetParent(tunnel);

				//	Data.Viewer.Cache.Instance.models.Lines.Add(newLine);

				//	List<Transform> _line = lineObjs[key];

				//	int _index = _line.Count;
				//	for (int i = 0; i < _index; i++)
				//	{
				//		_line[i].SetParent(newLine.transform);
				//	}
				//}
				//#endregion
			}
		}

		/// <summary>
		/// 객체의 라인 카운트 반환
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		private int GetObjLineIndex(Transform obj)
		{
			string name = obj.name;

			string[] spl = name.Split(',');

			//Debug.Log($"0 : {spl[0]}, 1 : {spl[1]}, 2 : {spl[2]}");

			if (spl.Length > 1)
			{
				int lineIndex;// = int.Parse(spl[1]);
				if (int.TryParse(spl[1], out lineIndex))
				{
					return lineIndex;
				}
				else
				{
					throw new System.Exception($"GetObjLineIndex : lineIndex ({spl[1]}) 정수변환 실패");
				}
			}
			else
			{
				return 0;
			}
		}

		#endregion

		#region 1-2. 모델 메시관리

		private void ControlSubMesh(List<GameObject> segments)
		{
			int index = segments.Count;
			for (int i = 0; i < index; i++)
			{
				//Debug.Log(segments[i].name); // segment 1 2 3...
				ControlSubMesh_perSegment(segments[i].transform);
			}
		}

		private void ControlSubMesh_perSegment(Transform segment)
		{
			int index = segment.childCount;
			for (int i = 0; i < index; i++)
			{
				//Debug.Log(segment.GetChild(i).name); // (Line) 1,1 1,2 2,1...
				ControlSubMesh_perLine(segment.GetChild(i));
			}
		}

		private void ControlSubMesh_perLine(Transform line)
		{
			int index = line.childCount;
			for (int i = 0; i < index; i++)
			{
				//Debug.Log(line.GetChild(i).name);	// 개별 객체 이름...
				ControlSubMesh_perObject(line.GetChild(i));
			}
		}

		private void ControlSubMesh_perObject(Transform obj)
		{
			// 서브메쉬가 정의된 객체인지 확인하는 변수
			bool isInSubMesh = false;

			// TODO : 1014 현재 기타 객체의 경우에만 SubMesh가 정의되어있음
			if (obj.name.Contains("Etc_"))
			{
				isInSubMesh = true;
			}

			int index = obj.childCount;
			for (int i = index-1; i >= 0; i--)
			{
				// 일반 정의되지 않은 객체에 서브메쉬가 존재하는 경우
				if (!isInSubMesh)
				{
					obj.GetChild(i).SetParent(trashCan.transform);
				}
				else
				{
					// 서브메쉬를 처리하는 메서드
					SetSubmeshObject(obj);
				}
			}

			SetMaterial_Collider(obj);
		}

		/// <summary>
		/// TODO 1014 : SubMesh 처리구간 메서드 처리방법이 정해지는대로 여기에 작성한다.
		/// </summary>
		/// <param name="obj"></param>
		private void SetSubmeshObject(Transform obj)
		{
			// 서브메쉬 할당 구간
			List<MeshFilter> filters = new List<MeshFilter>();

			MeshFilter meshfilter;
			if (obj.TryGetComponent<MeshFilter>(out meshfilter))
			{
				filters.Add(meshfilter);
			}

			int index = obj.childCount;
			for (int i = 0; i < index; i++)
			{
				if (obj.GetChild(i).TryGetComponent<MeshFilter>(out meshfilter))
				{
					filters.Add(meshfilter);
				}
			}

			Mesh result = CombineMeshes(filters.ToArray());

			// TODO 1014 : 결과물 메쉬를 기반으로 obj 객체의 Mesh를 교체한다.
			// 자식 객체들을 trashCan으로 보낸다.
		}

		/// <summary>
		/// 객체의 Material, Collider 할당
		/// </summary>
		/// <param name="obj"></param>
		private void SetMaterial_Collider(Transform obj)
		{
			MeshRenderer render;

			if (obj.TryGetComponent<MeshRenderer>(out render))
			{
				//Debug.Log(obj.name);
				//Datas.ObjectType param = Utilities.NameParameter.GetMatParameter(obj.name, 1);

				//Utilities.ReturnMaterial.SetMaterials(render, param);

				//if(!obj.name.Contains("_SwO"))
				//{
				//	obj.gameObject.AddComponent<MeshCollider>().convex = false;
				//}
			}
		}

		/// <summary>
		/// 객체의 Material 할당
		/// </summary>
		/// <param name="obj"></param>
		public void SetMaterial(Transform obj, int mode = 0)
		{
			MeshRenderer render;

			if (obj.TryGetComponent<MeshRenderer>(out render))
			{
				// 기본 Material
				//if(mode == 0)
				//{
				//	//Debug.Log(obj.name);
				//	Datas.ObjectType param = Utilities.NameParameter.GetMatParameter(obj.name, 1);

				//	Utilities.ReturnMaterial.SetMaterials(render, param);
				//}
				//// 반투명 Material
				//else if(mode == 1)
				//{
				//	// 일단 반투명 하나만 주기
				//	render.material = Resources.Load<Material>("Material/MAT_Transparent");
				//}
			}
		}

		#endregion

		//----------------------------------------

		public Mesh CombineMeshes(MeshFilter[] meshes)
		{
			// Key : shared mesh instance ID, Value : arguments to combine meshes
			var helper = new Dictionary<int, List<CombineInstance>>();

			// Build combine instances for each type of mesh
			foreach (var m in meshes)
			{
				List<CombineInstance> tmp;
				if (!helper.TryGetValue(m.sharedMesh.GetInstanceID(), out tmp))
				{
					tmp = new List<CombineInstance>();
					helper.Add(m.sharedMesh.GetInstanceID(), tmp);
				}

				var ci = new CombineInstance();
				ci.mesh = m.sharedMesh;
				ci.transform = m.transform.localToWorldMatrix;
				tmp.Add(ci);
			}

			// Combine meshes and build combine instance for combined meshes
			var list = new List<CombineInstance>();
			foreach (var e in helper)
			{
				var m = new Mesh();
				m.CombineMeshes(e.Value.ToArray());
				var ci = new CombineInstance();
				ci.mesh = m;
				list.Add(ci);
			}

			// And now combine everything
			var result = new Mesh();
			result.CombineMeshes(list.ToArray(), false, false);

			// It is a good idea to clean unused meshes now
			foreach (var m in list)
			{
				Destroy(m.mesh);
			}

			return result;
		}
	}
}
