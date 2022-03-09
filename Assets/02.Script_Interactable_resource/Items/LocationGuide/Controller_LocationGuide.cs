using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{

	public class Controller_LocationGuide : MonoBehaviour
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
