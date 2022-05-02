using Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
	using Definition;

	public class Controller_LocationGuide : AItem
	{
		[SerializeField] List<LocationElement> m_elements;

		[SerializeField] Material m_default;
		[SerializeField] Color m_defaultColor;
		[SerializeField] List<Color> m_hoverColors;

		public Material MDefault { get => m_default; set => m_default=value; }
		public Color DefaultColor { get => m_defaultColor; set => m_defaultColor=value; }
		public List<Color> HoverColors { get => m_hoverColors; set => m_hoverColors=value; }

		private void Start()
		{
			m_elements.ForEach(x => x.GetComponent<MeshRenderer>().material = m_default);

			UpdateState(EventManager.Instance._ModuleList);
		}

		public override void UpdateState(List<Definition.ModuleCode> _mList)
		{
			// 이 아이템은 PinMode일때 동작함
			gameObject.SetActive(_mList.Contains(ModuleCode.Work_Pinmode));
		}

		/// <summary>
		/// 가이드 큐브 업 아이템 활성화
		/// </summary>
		/// <param name="_target"></param>
		/// <param name="_baseAngle"></param>
		/// <param name="_uType"></param>
		public override void SetGuide(GameObject _target, Vector3 _baseAngle, UIEventType _uType)
		{
			Bounds bound = _target.GetComponent<MeshRenderer>().bounds;

			Vector3 targetPos = bound.center;
			Vector3 targetScale = bound.size;
			//Vector3 targetPos = _target.transform.position;
			//Vector3 targetScale = _target.transform.localScale;

			transform.position = targetPos;
			transform.rotation = Quaternion.Euler(_baseAngle);
			transform.Rotate(Angle.Set(_uType));
			transform.Translate(Positions.SetLocal(_target, _uType));

			Vector3 setScale = Scales.SetQuad(_target, _uType);
			transform.localScale = setScale;

			SetCubeLine(setScale);
		}

		/// <summary>
		/// 개별 영역의 크기를 조절한다.
		/// </summary>
		/// <param name="_scale"></param>
		public void SetCubeLine(Vector3 _scale)
		{
			//1.13f
			m_elements.ForEach(x => x.SetCubeScale(_scale));
		}
	}
}
