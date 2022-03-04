using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("IMDraw/Examples/ContactPointVisualiser"), DisallowMultipleComponent]
public class ContactPointVisualiser : MonoBehaviour
{
	public Color			m_Colour = Color.yellow;
	public float			m_Size = 0.2f;
	public float			m_Duration = 0.04f;

	void OnCollisionStay (Collision collisionInfo)
	{
		ContactPoint[] contactPoint = collisionInfo.contacts;

		for (int i = 0; i < contactPoint.Length; ++i)
		{
			DrawContactPoint(ref contactPoint[i]);
		}
	}

	private void DrawContactPoint(ref ContactPoint contactPoint)
	{
		IMDraw.Ray3D(contactPoint.point, contactPoint.normal, m_Size, m_Colour, m_Duration);
	}
}
