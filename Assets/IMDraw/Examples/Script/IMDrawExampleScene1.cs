using UnityEngine;
using System.Collections;

[AddComponentMenu("IMDraw/Examples/IMDraw Example Scene 1"), DisallowMultipleComponent]
public class IMDrawExampleScene1 : MonoBehaviour
{
	[Header("Camera")]
	public IMDrawCameraController	m_CameraController;
	public Vector3					m_CameraPosition;
	public Vector3					m_CameraRotation;

	[Header("Demo")]
	public ParticleSystem			m_TestParticleSystem;
	public IMDrawAxis				m_TestAxis;

	[Multiline(10)]
	public string					m_Controls;

	[Header("Colors")]
	public Color					m_ColorQuad;
	public Color					m_ColorBox;
	public Color					m_ColorPyramid;
	public Color					m_ColorRhombus;
	public Color					m_ColorDisc;
	public Color					m_ColorSphere;
	public Color					m_ColorEllipsoid;
	public Color					m_ColorCone;
	public Color					m_ColorCapsule;
	public Color					m_ColorCylinder;
	public Color					m_ColorArc;

	private Vector3					m_EulerRotation;
	private float					m_RotationY;
	private ParticleSystemRenderer	m_TestParticleSystemRenderer;
	private float					m_TextMeshRotation;

	void OnEnable()
	{
		IMDraw.Flush();
		m_CameraController.SetPosition(m_CameraPosition);
		m_CameraController.SetRotation(m_CameraRotation);

		if (m_TestParticleSystemRenderer == null)
		{
			m_TestParticleSystemRenderer = gameObject.GetComponent<ParticleSystemRenderer>();
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Time.timeScale = Time.timeScale == 1f ? 0f : 1f;
		}
	}

	void LateUpdate ()
	{ 
		//float screenWidth = Screen.width;
		float screenHeight = Screen.height;
		float deltaTime = Time.deltaTime;

		m_EulerRotation.x += deltaTime * 90f;
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x, 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;
		m_RotationY = Mathf.Repeat(m_RotationY + (deltaTime * 45f), 360f);

		Vector3 origin = Vector3.zero;
		Vector3 position;
		Quaternion objectRotation = Quaternion.Euler(m_EulerRotation);

		Quaternion rotation = Quaternion.Euler(0f, m_RotationY, 0f);

		IMDraw.Axis3D(Vector3.zero, Quaternion.identity, 100f, 0.5f);

		IMDraw.Grid3D(Vector3.zero, Quaternion.identity, 5f, 5f, 10, 10, new Color(1f, 1f, 1f, 0.5f));

		position = origin + (rotation * new Vector3(-2f, 2f, 0f));
		IMDraw.Quad3D(position, objectRotation, 1f, 1f, m_TestAxis, ToColor(m_ColorQuad, 0.25f));
		IMDraw.Grid3D(position, objectRotation, 1f, 1f, 1, 1, m_TestAxis, m_ColorQuad);
		IMDraw.LabelShadowed(position, m_ColorQuad, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "QUAD");

		position = origin + (rotation * new Vector3(0f, 2f, 0f));
		IMDraw.Box3D(position, objectRotation, new Vector3(1f, 1f, 1f), ToColor(m_ColorBox, 0.25f));
		IMDraw.WireBox3D(position, objectRotation, new Vector3(1f, 1f, 1f), m_ColorBox);
		IMDraw.LabelShadowed(position, m_ColorBox, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "BOX");

		position = origin + (rotation * new Vector3(2f, 2f, 0f));
		float pyramidHeight = 1f, pyramidWidth = 0.5f;
		IMDraw.Pyramid3D(position, objectRotation, pyramidHeight, pyramidWidth, m_TestAxis, ToColor(m_ColorPyramid, 0.25f));
		IMDraw.WirePyramid3D(position, objectRotation, pyramidHeight, pyramidWidth, m_TestAxis, m_ColorPyramid);
		IMDraw.LabelShadowed(position, m_ColorPyramid, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "PYRAMID");

		position = origin + (rotation * new Vector3(0f, 2f, 2f));
		float rhombusHeight = 2f, rhombusWidth = 0.75f;
		IMDraw.Rhombus3D(position, objectRotation, rhombusHeight, rhombusWidth, m_TestAxis, ToColor(m_ColorRhombus, 0.25f));
		IMDraw.WireRhombus3D(position, objectRotation, rhombusHeight, rhombusWidth, m_TestAxis, m_ColorRhombus);
		IMDraw.LabelShadowed(position, m_ColorRhombus, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "RHOMBUS");

		position = origin + (rotation * new Vector3(0f, 2f, -2f));
		IMDraw.Disc3D(position, objectRotation, 0.5f, m_TestAxis, ToColor(m_ColorDisc, 0.25f));
		IMDraw.WireDisc3D(position, objectRotation, 0.5f, m_TestAxis, m_ColorDisc);
		IMDraw.LabelShadowed(position, m_ColorDisc, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "DISC");


		position = origin + (rotation * new Vector3(0f, -2f, 0f));
		IMDraw.WireSphere3D(position, objectRotation, 0.5f, m_ColorSphere);
		IMDraw.Sphere3D(position, objectRotation, 0.5f, ToColor(m_ColorSphere, 0.25f));
		IMDraw.LabelShadowed(position, m_ColorSphere, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "SPHERE");

		position = origin + (rotation * new Vector3(2f, -2f, 0f));
		IMDraw.Cylinder3D(position, objectRotation, 2f, 0.25f, IMDrawAxis.X, ToColor(m_ColorCylinder, 0.25f));
		IMDraw.WireCylinder3D(position, objectRotation, 2f, 0.25f, IMDrawAxis.X, m_ColorCylinder);
		IMDraw.LabelShadowed(position, m_ColorCylinder, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "CYLINDER");

		position = origin + (rotation * new Vector3(-2f, -2f, 0f));
		IMDraw.Capsule3D(position, objectRotation, 2f, 0.25f, IMDrawAxis.Y, ToColor(m_ColorCapsule, 0.25f));
		IMDraw.WireCapsule3D(position, objectRotation, 2f, 0.25f, IMDrawAxis.Y, m_ColorCapsule);
		IMDraw.LabelShadowed(position, m_ColorCapsule, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "CAPSULE");

		position = origin + (rotation * new Vector3(0f, -2f, 2f));
		Vector3 ellipsoidSize = new Vector3(2f, 1f, 1f);
		IMDraw.Ellipsoid3D(position, objectRotation, ellipsoidSize, ToColor(m_ColorEllipsoid, 0.25f));
		IMDraw.WireEllipsoid3D(position, objectRotation, ellipsoidSize, m_ColorEllipsoid);
		IMDraw.LabelShadowed(position, m_ColorEllipsoid, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "ELLIPSOID");

		position = origin + (rotation * new Vector3(0f, -2f, -2f));
		IMDraw.Cone3D(position, objectRotation, 1f, 0.5f, m_TestAxis, ToColor(m_ColorCone, 0.25f));
		IMDraw.WireCone3D(position, objectRotation, 1f, 0.5f, m_TestAxis, m_ColorCone);
		IMDraw.LabelShadowed(position, m_ColorCone, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "CONE");

		IMDraw.Arc3D(Vector3.zero, Quaternion.identity, 4f, 5f, m_RotationY, 45f, m_ColorArc );
		IMDraw.Arc3D(Vector3.zero, Quaternion.identity, 2f, 3f, 360f - m_RotationY, 45f, m_ColorArc);

		if (m_TestParticleSystemRenderer != null)
		{
			IMDraw.Bounds(m_TestParticleSystemRenderer.bounds, Color.green);
		}

		IMDraw.LabelShadowed(Screen.width / 2f, 10f, Color.white, 20, LabelPivot.UPPER_CENTER, LabelAlignment.CENTER, "EXAMPLE SCENE 1: PRIMITIVES & LABELS");

		IMDraw.LabelShadowed(10f, screenHeight - 10f, Color.white, LabelPivot.LOWER_LEFT, LabelAlignment.LEFT, m_Controls);

		UpdateTextMesh(deltaTime);
	}

	private Color ToColor (Color input, float alpha)
	{
		return new Color(input.r, input.g, input.b, alpha);
	}

	private void UpdateTextMesh (float deltaTime)
	{
		m_TextMeshRotation += deltaTime * 45f;

		if (m_TextMeshRotation > 360f)
			m_TextMeshRotation -= 360f;

		Quaternion rotation = Quaternion.Euler(0f, m_TextMeshRotation, 0f);

		IMDraw.TextMesh(rotation * new Vector3(0f, 0.4f, -4f), rotation, 2f, 2f, Color.white, TextAlignment.Center, TextAnchor.MiddleCenter, "TEXT\nMESH");

		Quaternion rotation2 = Quaternion.Euler(0f, 360f - m_TextMeshRotation, 0f);

		IMDraw.TextMesh(rotation2 * new Vector3(0f, -0.4f, -4f), rotation2, 2f, 2f, Color.white, TextAlignment.Center, TextAnchor.MiddleCenter, "TEXT\nMESH");
	}
}
