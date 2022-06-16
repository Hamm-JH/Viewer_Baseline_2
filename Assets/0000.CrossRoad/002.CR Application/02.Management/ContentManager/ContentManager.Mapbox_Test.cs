using Module.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	public partial class ContentManager : IManager<ContentManager>
	{
		[System.Serializable]
		public class Phase1_Position_Object
        {
			/// <summary>
			/// 포인트 리스트
			/// </summary>
            [SerializeField] private List<Vector3> points;
			[SerializeField] private float distance;


            public List<Vector3> Points { get => points; set => points = value; }
            public float Distance { get => distance; set => distance = value; }
        }

		public Phase1_Position_Object mapbox_p1Position;

		/// <summary>
		/// 모델 객체로 컨트롤
		/// </summary>
		public class Phase2_CenterRotation_Object { }

		/// <summary>
		/// 모델 객체를 참조
		/// </summary>
		public class Phase3_Object_Object { }

		public void SetPhase1_StartEndPosition()
        {
			GameObject[] objs = Module<Module_Model>().m_bridgeBottomParts;
			//mapbox_p1Position = new Phase1_Position_Object();
			mapbox_p1Position.Points = new List<Vector3>();
			mapbox_p1Position.Points.Add(GetBoundsPosition(objs[0].transform));
			mapbox_p1Position.Points.Add(GetBoundsPosition(objs[objs.Length - 1].transform));

			// 거리를 구한다.
			mapbox_p1Position.Distance = Vector3.Distance(mapbox_p1Position.Points[0], mapbox_p1Position.Points[1]);

			//// 거리만큼 rootobj 위치 이동
			//Transform rootTransform = Module<Module_Model>().Model.transform;
			//Vector3 rootPos = rootTransform.position;
			//rootTransform.position = new Vector3(-mapbox_p1Position.Distance / 2, 0, 0);
		}

		/// <summary>
		/// 시작점 교대와 종료점 교대 
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		private Vector3 GetBoundsPosition(Transform target)
        {
			Vector3 result = default(Vector3);

			Vector3 min = default(Vector3);
			Vector3 max = default(Vector3);

			int index = target.childCount;
            for (int i = 0; i < index; i++)
            {
				MeshRenderer render;
				if (target.GetChild(i).TryGetComponent<MeshRenderer>(out render))
                {
					if(i == 0)
                    {
						min = render.bounds.min;
						max = render.bounds.max;
                    }
					else
                    {
						float minX, minY, minZ = 0f;
						float maxX, maxY, maxZ = 0f;

						Vector3 thisMin = render.bounds.min;
						Vector3 thisMax = render.bounds.max;

						min = new Vector3(
							min.x > thisMin.x ? thisMin.x : min.x,
							min.y > thisMin.y ? thisMin.y : min.y,
							min.z > thisMin.z ? thisMin.z : min.z);

						max = new Vector3(
							max.x < thisMax.x ? thisMax.x : max.x,
							max.y < thisMax.y ? thisMax.y : max.y,
							max.z < thisMax.z ? thisMax.z : max.z);
                    }
                }
            }

			result = (min + max) / 2;

			return result;
        }

		public void SetPhase2_CenterRotation()
        {
			// 거리만큼 rootobj 위치 이동
			Transform rootTransform = Module<Module_Model>().Model.transform;
			Vector3 rootPos = rootTransform.position;
			rootTransform.position = new Vector3(-mapbox_p1Position.Distance / 2, 0, 0);
		}

		public void SetPhase3_SetModelObject(float _mapDist, Transform _mapCenter)
        {
			// 맵 레벨에서 점 사이 길이 : 객체 시종점간 길이 비율 구함
			float scaleFactor = _mapDist / mapbox_p1Position.Distance;

			Transform model = Module<Module_Model>().Model.transform.parent;
			model.transform.position = _mapCenter.position;
			model.transform.rotation = Quaternion.Euler(_mapCenter.rotation.eulerAngles + new Vector3(0, 90, 0));
			
			model.transform.localScale = new Vector3(scaleFactor, 1, 1);
			
        }
	}
}
