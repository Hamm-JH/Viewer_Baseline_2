using UnityEngine;
using System.Collections;

[AddComponentMenu("IMDraw/Examples/IMDraw Example Scene 2"), DisallowMultipleComponent]
public class IMDrawExampleScene2 : MonoBehaviour
{
	[Header("Camera")]
	public IMDrawCameraController	m_CameraController;
	public Vector3					m_CameraPosition;
	public Vector3					m_CameraRotation;

	[Header("Demo")]
	public Transform				m_RotatingTransform;
	private float					m_RotationY;
	public Rigidbody[]				m_RigidBodies;
	public float					m_Acceleration;
	public Vector3					m_Position;

	[Multiline(10)]
	public string					m_Controls;

	void OnEnable ()
	{
		IMDraw.Flush();
		m_CameraController.SetPosition(m_CameraPosition);
		m_CameraController.SetRotation(m_CameraRotation);
	}
	
	void FixedUpdate ()
	{
		m_RotationY = Mathf.Repeat(m_RotationY + 45f * Time.fixedDeltaTime, 360f);
		m_RotatingTransform.localRotation = Quaternion.Euler(0f, m_RotationY, 0f);

		Vector3 force;

		for (int i = 0; i < m_RigidBodies.Length; ++i)
		{
			force = m_Position - m_RigidBodies[i].position;
			force.Normalize();
			force *= m_Acceleration;

			m_RigidBodies[i].AddForce(force, ForceMode.Acceleration);
		}
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Time.timeScale = Time.timeScale == 1f ? 0f : 1f;
		}
	}

	void LateUpdate ()
	{ 
		IMDraw.Axis3D(Vector3.zero, Quaternion.identity, 1000f, 0.5f);

		IMDraw.LabelShadowed(Screen.width / 2f, 10f, Color.white, LabelPivot.UPPER_CENTER, LabelAlignment.CENTER, "EXAMPLE SCENE 2: COLLIDERS & CONTACT POINTS");

		IMDraw.LabelShadowed(10f, Screen.height - 10f, Color.white, LabelPivot.LOWER_LEFT, LabelAlignment.LEFT, m_Controls);
	}
}
