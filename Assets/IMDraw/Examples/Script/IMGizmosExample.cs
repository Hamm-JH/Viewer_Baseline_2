using UnityEngine;
using System.Collections;

[AddComponentMenu("IMDraw/Examples/IMGizmos Example"), DisallowMultipleComponent]
public class IMGizmosExample : MonoBehaviour
{
	public bool m_Enable;

	[Header("Line test")]

	public Vector3 m_LineStart;
	public Vector3 m_LineEnd;
	public Color m_LineColor = Color.white;

	[Header("Quad test")]

	public Vector3 m_QuadPosition;
	public Vector3 m_QuadRotation;
	public Vector2 m_QuadSize;
	public GizmoDrawAxis m_QuadAxis;
	public Color m_QuadSolidColor = Color.white;
	public Color m_QuadWireframeColor = Color.white;

	[Header("Disc test")]

	public Vector3 m_DiscPosition;
	public Vector3 m_DiscRotation;
	public float m_DiscRadius;
	public GizmoDrawAxis m_DiscAxis;
	public Color m_DiscSolidColor = Color.white;
	public Color m_DiscWireframeColor = Color.white;

	[Header("Box test")]

	public Vector3 m_BoxPosition;
	public Vector3 m_BoxRotation;
	public Vector3 m_BoxSize;
	public Color m_BoxSolidColor = Color.white;
	public Color m_BoxWireframeColor = Color.white;

	[Header("Pyramid test")]

	public Vector3 m_PyramidPosition;
	public Vector3 m_PyramidRotation;
	public Vector2 m_PyramidSize;
	public GizmoDrawAxis m_PyramidAxis;
	public Color m_PyramidSolidColor = Color.white;
	public Color m_PyramidWireframeColor = Color.white;

	[Header("Rhombus test")]

	public Vector3 m_RhombusPosition;
	public Vector3 m_RhombusRotation;
	public Vector2 m_RhombusSize;
	public GizmoDrawAxis m_RhombusAxis;
	public Color m_RhombusSolidColor = Color.white;
	public Color m_RhombusWireframeColor = Color.white;

	[Header("Sphere test")]

	public Vector3 m_SpherePosition;
	public Vector3 m_SphereRotation;
	public float m_SphereRadius;
	public Color m_SphereSolidColor = Color.white;
	public Color m_SphereWireframeColor = Color.white;

	[Header("Ellipsoid test")]

	public Vector3 m_EllipsoidPosition;
	public Vector3 m_EllipsoidRotation;
	public Vector3 m_EllipsoidSize;
	public Color m_EllipsoidSolidColor = Color.white;
	public Color m_EllipsoidWireframeColor = Color.white;

	[Header("Cone test")]

	public Vector3 m_ConePosition;
	public Vector3 m_ConeRotation;
	public Vector2 m_ConeSize;
	public GizmoDrawAxis m_ConeAxis;
	public Color m_ConeSolidColor = Color.white;
	public Color m_ConeWireframeColor = Color.white;

	[Header("Capsule test")]

	public Vector3 m_CapsulePosition;
	public Vector3 m_CapsuleRotation;
	public Vector2 m_CapsuleSize;
	public GizmoDrawAxis m_CapsuleAxis;
	public Color m_CapsuleSolidColor = Color.white;
	public Color m_CapsuleWireframeColor = Color.white;

	[Header("Cylinder test")]

	public Vector3 m_CylinderPosition;
	public Vector3 m_CylinderRotation;
	public Vector2 m_CylinderSize;
	public GizmoDrawAxis m_CylinderAxis;
	public Color m_CylinderSolidColor = Color.white;
	public Color m_CylinderWireframeColor = Color.white;

	[Header("Mesh test")]
	public Mesh m_Mesh;
	public Vector3 m_MeshPosition;
	public Vector3 m_MeshRotation;
	public Vector3 m_MeshScale;
	public Color m_MeshSolidColor = Color.white;
	public Color m_MeshWireframeColor = Color.white;

	[Header("Arc test")]
	public Vector3 m_ArcPosition;
	public Vector3 m_ArcRotation;
	public float m_ArcInnerRadius;
	public float m_ArcOuterRadius;
	public float m_ArcDirectionAngle;
	public float m_ArcSectorAngle;
	public Color m_ArcColor;

	[Header("Axis test")]
	public Vector3 m_AxisPosition;
	public Vector3 m_AxisRotation;
	public float m_AxisLength = 8f;
	public float m_AxisAlpha = 1f;

	[Header("Grid test")]

	public Vector3 m_GridPosition;
	public Vector3 m_GridRotation;
	public Vector2 m_GridSize;
	public int m_GridCellsX;
	public int m_GridCellsY;
	public GizmoDrawAxis m_GridAxis;
	public Color m_GridColor = Color.white;

	[Header("2D label test")]

	public Vector2 m_2DLabelAnchor;
	public Vector2 m_2DLabelPosition;
	public Color m_2DLabelColor = Color.white;
	public LabelPivot m_2DLabelPivot;
	public LabelAlignment m_2DLabelAlignment;
	public int m_2DLabelFontSize = 16;

	[TextArea]
	public string m_2DLabelString = "This is a 2D label";

	[Header("2D image test")]

	public Texture2D m_ImageTexture;
	public Rect m_ImageTextureRect = new Rect (10f,10f, 100f, 100f);
	public Color m_ImageColor = Color.white;

	[Header("Bounds test")]
	public Renderer m_BoundsRenderer;
	public Color m_BoundsColor = Color.green;

	void OnDrawGizmos()
	{
		if (!m_Enable)
			return;

		Vector3 origin = transform.position;
		Quaternion rotation;

		float spacing = 1.5f;

		IMGizmos.Label(origin + (m_LineStart + m_LineEnd) * 0.5f, m_LineColor, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "LINE", 100f);

		IMGizmos.Line3D(origin + m_LineStart, origin + m_LineEnd, m_LineColor);
		IMGizmos.Ray3D(new Ray(origin + m_LineStart + new Vector3(-0.1f, 0f, 0f), new Vector3(0f, 0f, 1f)), 9f, m_LineColor);
		IMGizmos.Ray3D(origin + m_LineStart + new Vector3(-0.2f, 0f, 0f), new Vector3(0f, 0f, 1f), 9f, m_LineColor);
		IMGizmos.Ray3D(origin + m_LineStart + new Vector3(-0.3f, 0f, 0f), Quaternion.LookRotation(new Vector3(0f, 0f, 1f)), 9f, m_LineColor);

		IMGizmos.Label(origin + m_DiscPosition, m_DiscWireframeColor, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "DISC", 100f);

		IMGizmos.WireDisc3D(origin + m_DiscPosition, Quaternion.Euler(m_DiscRotation), m_DiscRadius, m_DiscAxis, m_DiscWireframeColor);
		IMGizmos.Disc3D(origin + m_DiscPosition + new Vector3(spacing * 1f, 0f, 0f), Quaternion.Euler(m_DiscRotation), m_DiscRadius, m_DiscAxis, m_DiscSolidColor);
		IMGizmos.Disc3D(origin + m_DiscPosition + new Vector3(spacing * 2f, 0f, 0f), Quaternion.Euler(m_DiscRotation), m_DiscRadius, m_DiscAxis, m_DiscSolidColor, m_DiscWireframeColor);

		IMGizmos.Label(origin + m_QuadPosition, m_QuadWireframeColor, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "QUAD", 100f);

		IMGizmos.WireQuad3D(origin + m_QuadPosition, Quaternion.Euler(m_QuadRotation), m_QuadSize.x, m_QuadSize.y, m_QuadAxis, m_QuadWireframeColor);
		IMGizmos.Quad3D(origin + m_QuadPosition + new Vector3(spacing * 1f, 0f, 0f), Quaternion.Euler(m_QuadRotation), m_QuadSize.x, m_QuadSize.y, m_QuadAxis, m_QuadSolidColor);
		IMGizmos.Quad3D(origin + m_QuadPosition + new Vector3(spacing * 2f, 0f, 0f), Quaternion.Euler(m_QuadRotation), m_QuadSize.x, m_QuadSize.y, m_QuadAxis, m_QuadSolidColor, m_QuadWireframeColor);

		IMGizmos.Label(origin + m_BoxPosition, m_BoxWireframeColor, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "BOX", 100f);

		IMGizmos.WireBox3D(origin + m_BoxPosition, Quaternion.Euler(m_BoxRotation), m_BoxSize, m_BoxWireframeColor);
		IMGizmos.Box3D(origin + m_BoxPosition + new Vector3(spacing * 1f, 0f, 0f), Quaternion.Euler(m_BoxRotation), m_BoxSize, m_BoxSolidColor);
		IMGizmos.Box3D(origin + m_BoxPosition + new Vector3(spacing * 2f, 0f, 0f), Quaternion.Euler(m_BoxRotation), m_BoxSize, m_BoxSolidColor, m_BoxWireframeColor);

		IMGizmos.Label(origin + m_PyramidPosition, m_PyramidWireframeColor, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "PYRAMID", 100f);

		IMGizmos.WirePyramid3D(origin + m_PyramidPosition, Quaternion.Euler(m_PyramidRotation), m_PyramidSize.y, m_PyramidSize.x, m_PyramidAxis, m_PyramidWireframeColor);
		IMGizmos.Pyramid3D(origin + m_PyramidPosition + new Vector3(spacing * 1f, 0f, 0f), Quaternion.Euler(m_PyramidRotation), m_PyramidSize.y, m_PyramidSize.x, m_PyramidAxis, m_PyramidSolidColor);
		IMGizmos.Pyramid3D(origin + m_PyramidPosition + new Vector3(spacing * 2f, 0f, 0f), Quaternion.Euler(m_PyramidRotation), m_PyramidSize.y, m_PyramidSize.x, m_PyramidAxis, m_PyramidSolidColor, m_PyramidWireframeColor);

		IMGizmos.Label(origin + m_RhombusPosition, m_RhombusWireframeColor, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "RHOMBUS", 100f);

		IMGizmos.WireRhombus3D(origin + m_RhombusPosition, Quaternion.Euler(m_RhombusRotation), m_RhombusSize.y, m_RhombusSize.x, m_RhombusAxis, m_RhombusWireframeColor);
		IMGizmos.Rhombus3D(origin + m_RhombusPosition + new Vector3(spacing * 1f, 0f, 0f), Quaternion.Euler(m_RhombusRotation), m_RhombusSize.y, m_RhombusSize.x, m_RhombusAxis, m_RhombusSolidColor);
		IMGizmos.Rhombus3D(origin + m_RhombusPosition + new Vector3(spacing * 2f, 0f, 0f), Quaternion.Euler(m_RhombusRotation), m_RhombusSize.y, m_RhombusSize.x, m_RhombusAxis, m_RhombusSolidColor, m_RhombusWireframeColor);

		IMGizmos.Label(origin + m_SpherePosition, m_SphereWireframeColor, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "SPHERE", 100f);

		IMGizmos.WireSphere3D(origin + m_SpherePosition, Quaternion.Euler(m_SphereRotation), m_SphereRadius, m_SphereWireframeColor);
		IMGizmos.Sphere3D(origin + m_SpherePosition + new Vector3(spacing * 1f, 0f, 0f), Quaternion.Euler(m_SphereRotation), m_SphereRadius, m_SphereSolidColor);
		IMGizmos.Sphere3D(origin + m_SpherePosition + new Vector3(spacing * 2f, 0f, 0f), Quaternion.Euler(m_SphereRotation), m_SphereRadius, m_SphereSolidColor, m_SphereWireframeColor);

		IMGizmos.Label(origin + m_EllipsoidPosition, m_EllipsoidWireframeColor, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "ELLIPSOID", 100f);

		IMGizmos.WireEllipsoid3D(origin + m_EllipsoidPosition, Quaternion.Euler(m_EllipsoidRotation), m_EllipsoidSize, m_EllipsoidWireframeColor);
		IMGizmos.Ellipsoid3D(origin + m_EllipsoidPosition + new Vector3(spacing * 1f, 0f, 0f), Quaternion.Euler(m_EllipsoidRotation), m_EllipsoidSize, m_EllipsoidSolidColor);
		IMGizmos.Ellipsoid3D(origin + m_EllipsoidPosition + new Vector3(spacing * 2f, 0f, 0f), Quaternion.Euler(m_EllipsoidRotation), m_EllipsoidSize, m_EllipsoidSolidColor, m_EllipsoidWireframeColor);

		IMGizmos.Label(origin + m_ConePosition, m_ConeWireframeColor, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "CONE", 100f);

		IMGizmos.WireCone3D(origin + m_ConePosition, Quaternion.Euler(m_ConeRotation), m_ConeSize.y, m_ConeSize.x, m_ConeAxis, m_ConeWireframeColor);
		IMGizmos.Cone3D(origin + m_ConePosition + new Vector3(spacing * 1f, 0f, 0f), Quaternion.Euler(m_ConeRotation), m_ConeSize.y, m_ConeSize.x, m_ConeAxis, m_ConeSolidColor);
		IMGizmos.Cone3D(origin + m_ConePosition + new Vector3(spacing * 2f, 0f, 0f), Quaternion.Euler(m_ConeRotation), m_ConeSize.y, m_ConeSize.x, m_ConeAxis, m_ConeSolidColor, m_ConeWireframeColor);

		IMGizmos.Label(origin + m_CapsulePosition, m_CapsuleWireframeColor, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "CAPSULE", 100f);

		IMGizmos.WireCapsule3D(origin + m_CapsulePosition, Quaternion.Euler(m_CapsuleRotation), m_CapsuleSize.y, m_CapsuleSize.x, m_CapsuleAxis, m_CapsuleWireframeColor);
		IMGizmos.Capsule3D(origin + m_CapsulePosition + new Vector3(spacing * 1f, 0f, 0f), Quaternion.Euler(m_CapsuleRotation), m_CapsuleSize.y, m_CapsuleSize.x, m_CapsuleAxis, m_CapsuleSolidColor);
		IMGizmos.Capsule3D(origin + m_CapsulePosition + new Vector3(spacing * 2f, 0f, 0f), Quaternion.Euler(m_CapsuleRotation), m_CapsuleSize.y, m_CapsuleSize.x, m_CapsuleAxis, m_CapsuleSolidColor, m_CapsuleWireframeColor);

		IMGizmos.Label(origin + m_CylinderPosition, m_CylinderWireframeColor, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "CYLINDER", 100f);

		IMGizmos.WireCylinder3D(origin + m_CylinderPosition, Quaternion.Euler(m_CylinderRotation), m_CylinderSize.y, m_CylinderSize.x, m_CylinderAxis, m_CylinderWireframeColor);
		IMGizmos.Cylinder3D(origin + m_CylinderPosition + new Vector3(spacing * 1f, 0f, 0f), Quaternion.Euler(m_CylinderRotation), m_CylinderSize.y, m_CylinderSize.x, m_CylinderAxis, m_CylinderSolidColor);
		IMGizmos.Cylinder3D(origin + m_CylinderPosition + new Vector3(spacing * 2f, 0f, 0f), Quaternion.Euler(m_CylinderRotation), m_CylinderSize.y, m_CylinderSize.x, m_CylinderAxis, m_CylinderSolidColor, m_CylinderWireframeColor);

		rotation = Quaternion.Euler(m_MeshRotation);
		IMGizmos.WireMesh(m_Mesh, origin + m_MeshPosition, rotation, m_MeshScale, m_MeshWireframeColor);
		IMGizmos.Mesh(m_Mesh, origin + m_MeshPosition + new Vector3(0f, 0f, 7f) , rotation, m_MeshScale, m_MeshSolidColor);
		IMGizmos.Mesh(m_Mesh, origin + m_MeshPosition + new Vector3(0f, 0f, 14f), rotation, m_MeshScale, m_MeshSolidColor, m_MeshWireframeColor);

		IMGizmos.Arc3D(origin + m_ArcPosition, Quaternion.Euler(m_ArcRotation), m_ArcInnerRadius, m_ArcOuterRadius, m_ArcDirectionAngle, m_ArcSectorAngle, m_ArcColor);
		IMGizmos.Label(origin + m_ArcPosition, m_ArcColor, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "ARC", 100f);

		IMGizmos.Label(origin + m_MeshPosition + new Vector3(0f, 0f, 7f), Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "MESH", 100f);

		IMGizmos.Label(origin + m_AxisPosition, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "AXIS", 100f);

		IMGizmos.Axis3D(origin + m_AxisPosition, Quaternion.Euler(m_AxisRotation), m_AxisLength, m_AxisAlpha);

		IMGizmos.Label(origin + m_GridPosition, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "GRID", 100f);

		IMGizmos.Grid3D(origin + m_GridPosition, Quaternion.Euler(m_GridRotation), m_GridSize.x, m_GridSize.y, m_GridCellsX, m_GridCellsY, m_GridAxis, m_GridColor);

		IMGizmos.Label(m_2DLabelPosition.x + ((float)Screen.width * m_2DLabelAnchor.x), m_2DLabelPosition.y + ((float)Screen.height * m_2DLabelAnchor.y), m_2DLabelColor, m_2DLabelPivot, m_2DLabelAlignment, m_2DLabelString, m_2DLabelFontSize);

		IMGizmos.Image(m_ImageTextureRect, m_ImageColor, m_ImageTexture);

		if (m_BoundsRenderer != null)
		{
			IMGizmos.Label(m_BoundsRenderer.bounds.center, m_BoundsColor, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "RENDERER\nBOUNDS", 100f);

			IMGizmos.Bounds(m_BoundsRenderer, m_BoundsColor);
		}
	}
}
