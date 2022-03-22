using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Viewer
{
	using System.Linq;

	[System.Serializable]
	public struct Model
	{
		[SerializeField] private GameObject gltf_root;		// Root GLTF
		[SerializeField] private GameObject modelName;      // 모델 이름
		[SerializeField] private List<GameObject> segments;     // 세그먼트들
		[SerializeField] private List<GameObject> lines;
		[SerializeField] private List<GameObject> objects;
		[SerializeField] public Bounds rootBound;

		public GameObject Gltf_root { get => gltf_root; set => gltf_root=value; }
		public GameObject ModelName
		{
			get => modelName;
			set
			{
				modelName = value;
				SetBoundary(modelName, out rootBound);
			}
		}
		//{ get => modelName; set => modelName=value; }
		public List<GameObject> Segments { get => segments; set => segments=value; }
		public List<GameObject> Lines { get => lines; set => lines=value; }
		public List<GameObject> Objects { get => objects; set => objects=value; }

		private void SetBoundary(GameObject root, out Bounds result)
		{
			result = new Bounds();

			MeshRenderer renderer;
			Transform[] trs = root.GetComponentsInChildren<Transform>();
			Transform[] renders = (from obj in trs where obj.TryGetComponent(out renderer) select obj).ToArray();

			float? minX = null;
			float? minY = null;
			float? minZ = null;

			float? maxX = null;
			float? maxY = null;
			float? maxZ = null;

			Bounds _b;
			foreach(Transform _tr in renders)
			{
				renderer = _tr.GetComponent<MeshRenderer>();

				_b = renderer.bounds;

				if (minX != null)
				{
					minX = Mathf.Min((float)minX, _b.min.x);
				}
				else
				{
					minX = _b.min.x;
				}

				if (minY != null)
				{
					minY = Mathf.Min((float)minY, _b.min.x);
				}
				else
				{
					minY = _b.min.y;
				}

				if (minZ != null)
				{
					minZ = Mathf.Min((float)minZ, _b.min.x);
				}
				else
				{
					minZ = _b.min.z;
				}


				if (maxX != null)
				{
					maxX = Mathf.Max((float)maxX, _b.min.x);
				}
				else
				{
					maxX = _b.min.x;
				}

				if (maxY != null)
				{
					maxY = Mathf.Max((float)maxY, _b.min.x);
				}
				else
				{
					maxY = _b.min.y;
				}

				if (maxZ != null)
				{
					maxZ = Mathf.Max((float)maxZ, _b.min.x);
				}
				else
				{
					maxZ = _b.min.z;
				}
			}

			Vector3 _min = new Vector3((float)minX, (float)minY, (float)minZ);
			Vector3 _max = new Vector3((float)maxX, (float)maxY, (float)maxZ);

			Vector3 center = (_min + _max) / 2;
			Vector3 size = (_max - _min);
			result.center = center;
			result.size = size;
		}
	}

	public partial class Cache : MonoBehaviour
	{
		public Model models;
	}
}

