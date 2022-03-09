using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
	using Test;

	[RequireComponent(typeof(MeshRenderer))]
	public class LocationElement : MonoBehaviour
	{
		[SerializeField] Controller_LocationGuide m_controller;

		Renderer m_render;
		[SerializeField] int m_index;

		public int Index { get => m_index; }

		public void SetCubeScale(Vector3 _scale)
		{
			float scX = _scale.x >= 1.13f ? 0.33f : 0.32f;
			float scY = _scale.y >= 1.13f ? 0.33f : 0.32f;

			transform.localScale = new Vector3(scX, scY, 1);
		}

		private void Awake()
		{
			m_render = GetComponent<Renderer>();
			//m_render.enabled = true;
			m_render.material.SetColor("_BaseColor", m_controller.DefaultColor);
		}

		private void OnEnable()
		{
			m_render = GetComponent<Renderer>();
			//m_render.enabled = true;
			m_render.material.SetColor("_BaseColor", m_controller.DefaultColor);
		}

		private void OnMouseEnter()
		{
			//m_render.enabled = true;
			m_render.material.SetColor("_BaseColor", m_controller.HoverColors[Index]);
		}

		private void OnMouseExit()
		{
			//m_render.enabled = false;
			m_render.material.SetColor("_BaseColor", m_controller.DefaultColor);
		}
	}
}
