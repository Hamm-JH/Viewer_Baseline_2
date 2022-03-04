using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("IMDraw/Examples/IMDraw Example Scene 3"), DisallowMultipleComponent]
public class IMDrawExampleScene3 : MonoBehaviour
{
	[Header("Camera")]
	public IMDrawCameraController	m_CameraController;
	public Vector3					m_CameraPosition;
	public Vector3					m_CameraRotation;

	[Header("Demo")]
	[Multiline(10)]
	public string					m_Controls;

	public Mesh						m_ExampleMesh;

	private List<IMDrawCallTest>	m_TestList;
	private int						m_TestIndex;
	private string					m_TestTitle;

	void OnEnable()
	{
		IMDraw.Flush();
		m_CameraController.SetPosition(m_CameraPosition);
		m_CameraController.SetRotation(m_CameraRotation);

		if (m_TestList == null)
		{
			m_TestIndex = 0;
			m_TestList = new List<IMDrawCallTest>();
			m_TestList.Add(new IMDrawLine3DTest());
			m_TestList.Add(new IMDrawRay3DTest());
			m_TestList.Add(new IMDrawQuad3DTest());
			m_TestList.Add(new IMDrawGridTest());
			m_TestList.Add(new IMDrawWireBox3DTest());
			m_TestList.Add(new IMDrawBox3DTest());
			m_TestList.Add(new IMDrawWirePyramid3DTest());
			m_TestList.Add(new IMDrawPyramid3DTest());
			m_TestList.Add(new IMDrawWireRhombus3DTest());
			m_TestList.Add(new IMDrawRhombus3DTest());
			m_TestList.Add(new IMDrawWireDiscTest());
			m_TestList.Add(new IMDrawDiscTest());
			m_TestList.Add(new IMDrawWireSphereTest());
			m_TestList.Add(new IMDrawSphereTest());
			m_TestList.Add(new IMDrawWireEllipsoidTest());
			m_TestList.Add(new IMDrawEllipsoidTest());
			m_TestList.Add(new IMDrawWireCone3DTest());
			m_TestList.Add(new IMDrawCone3DTest());
			m_TestList.Add(new IMDrawWireCapsuleTest());
			m_TestList.Add(new IMDrawCapsuleTest());
			m_TestList.Add(new IMDrawWireCylinderTest());
			m_TestList.Add(new IMDrawCylinderTest());
			m_TestList.Add(new IMDrawMeshTest(m_ExampleMesh));
			m_TestList.Add(new IMDrawLabelTest());
			m_TestList.Add(new IMDrawArc3DTest());

			UpdateIndex();
        }
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Time.timeScale = Time.timeScale == 1f ? 0f : 1f;
		}

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			--m_TestIndex;

			if (m_TestIndex < 0)
			{
				m_TestIndex = m_TestList.Count - 1;
            }

			IMDraw.Flush();

			UpdateIndex();
        }

		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			++m_TestIndex;

			if (m_TestIndex >= m_TestList.Count)
			{
				m_TestIndex = 0;
			}

			IMDraw.Flush();

			UpdateIndex();
        }
	}

	void LateUpdate ()
	{ 
		IMDraw.Axis3D(Vector3.zero, Quaternion.identity, 100f, 0.5f);

		float deltaTime = Time.deltaTime;

		m_TestList[m_TestIndex].Execute(deltaTime);
		
		float screenHeight = Screen.height;

		IMDraw.LabelShadowed(Screen.width / 2f, 10f, Color.white, LabelPivot.UPPER_CENTER, LabelAlignment.CENTER, "EXAMPLE SCENE 3: API TEST");

		IMDraw.LabelShadowed(Screen.width / 2f, 24f, Color.white, LabelPivot.UPPER_CENTER, LabelAlignment.CENTER, m_TestTitle);

		IMDraw.LabelShadowed(10f, screenHeight - 10f, Color.white, LabelPivot.LOWER_LEFT, LabelAlignment.LEFT, m_Controls);
	}

	private void UpdateIndex ()
	{
		m_TestTitle = string.Format("{0} / {1} : {2}", m_TestIndex + 1, m_TestList.Count, m_TestList[m_TestIndex].GetInfoString());
    }
}

//=============================================================================================================================================================

public abstract class IMDrawCallTest
{
	public abstract void Execute (float deltaTime);

	public abstract string GetInfoString ();
}

public class IMDrawLine3DTest : IMDrawCallTest
{
	private float m_T;

	public override void Execute (float deltaTime)
	{
		IMDraw.Line3D(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 1f), Color.cyan, Color.magenta);

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			IMDraw.Line3D(new Vector3(1f, 0f, 0f), new Vector3(1f, 0f, 1f), Color.cyan, Color.magenta, 1f);
			m_T += 2f;
		}
	}

	public override string GetInfoString()
	{
		return "3D LINE";
	}
}

public class IMDrawRay3DTest : IMDrawCallTest
{
	private float m_T;

	public override void Execute(float deltaTime)
	{
		IMDraw.Ray3D(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 1f), 1f, Color.cyan, Color.magenta);

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			IMDraw.Ray3D(new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 1f), 1f, Color.cyan, Color.magenta, 1f);
			m_T += 2f;
		}
	}

	public override string GetInfoString()
	{
		return "3D RAY";
	}
}

public class IMDrawQuad3DTest : IMDrawCallTest
{
	private Vector3 m_EulerRotation;
	private int m_Axis;
	private float m_T;
	private string m_AxisString;
	
	public IMDrawQuad3DTest()
	{
		UpdateAxis();
    }

	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.Quad3D(new Vector3(0f, 0f, 0f), rotation, 0.5f, 1f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			m_Axis = (m_Axis + 1) % 3;
			UpdateAxis();

			IMDraw.Quad3D(new Vector3(1f, 0f, 0f), Quaternion.identity, 0.5f, 1f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
        }

		IMDraw.LabelShadowed(Vector3.zero, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, m_AxisString);
	}

	public override string GetInfoString()
	{
		return "3D QUAD";
	}

	private void UpdateAxis ()
	{
		m_AxisString = string.Format("AXIS: {0}", ((IMDrawAxis)m_Axis).ToString());
	}
}

public class IMDrawGridTest : IMDrawCallTest
{
	private Vector3 m_EulerRotation;
	private int m_Axis;
	private float m_T;
	private string m_AxisString;

	public IMDrawGridTest ()
	{
		UpdateAxis();
	}

	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.Grid3D(new Vector3(0f, 0f, 0f), rotation, 1f, 1f, 10, 10, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			//IMDraw.Default.Grid3D(new Vector3(0f, 0f, 0f), rotation, 1f, 1f, 10, 10, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;

			m_Axis = (m_Axis + 1) % 3;
			UpdateAxis();
		}

		IMDraw.LabelShadowed(Screen.width / 2f, Screen.height / 2f, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, m_AxisString);
	}

	private void UpdateAxis()
	{
		m_AxisString = string.Format("AXIS: {0}", ((IMDrawAxis)m_Axis).ToString());
	}

	public override string GetInfoString()
	{
		return "3D GRID";
	}
}

public class IMDrawWireBox3DTest : IMDrawCallTest
{
	private Vector3 m_EulerRotation;
	private float m_T;

	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.WireBox3D(Vector3.zero, rotation, new Vector3(0.25f, 0.25f, 0.25f), Color.white);

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			IMDraw.WireBox3D(new Vector3 (1f, 0f, 0f), Quaternion.identity, new Vector3(0.25f, 0.25f, 0.25f), Color.white, 1f);
			m_T += 2f;
		}
	}

	public override string GetInfoString()
	{
		return "3D WIREFRAME BOX";
	}
}

public class IMDrawBox3DTest : IMDrawCallTest
{
	private Vector3 m_EulerRotation;
	private float m_T;

	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.Box3D(Vector3.zero, rotation, new Vector3(0.25f, 0.25f, 0.25f), new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			IMDraw.Box3D(new Vector3(1f, 0f, 0f), Quaternion.identity, new Vector3(0.25f, 0.25f, 0.25f), new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
		}
	}

	public override string GetInfoString()
	{
		return "3D BOX";
	}
}

public class IMDrawWireDiscTest : IMDrawCallTest
{
	private Vector3 m_EulerRotation;
	private int m_Axis;
	private float m_T;
	private string m_AxisString;

	public IMDrawWireDiscTest()
	{
		UpdateAxis();
	}

	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.WireDisc3D(new Vector3(0f, 0f, 0f), rotation, 0.25f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			m_Axis = (m_Axis + 1) % 3;
			UpdateAxis();

			IMDraw.WireDisc3D(new Vector3(1f, 0f, 0f), Quaternion.identity, 0.25f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
		}

		IMDraw.LabelShadowed(Vector3.zero, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, m_AxisString);
	}

	public override string GetInfoString()
	{
		return "3D WIRE DISC";
	}

	private void UpdateAxis()
	{
		m_AxisString = string.Format("AXIS: {0}", ((IMDrawAxis)m_Axis).ToString());
	}
}

public class IMDrawDiscTest : IMDrawCallTest
{
	private Vector3 m_EulerRotation;
	private int m_Axis;
	private float m_T;
	private string m_AxisString;
	
	public IMDrawDiscTest ()
	{
		UpdateAxis();
    }

	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.Disc3D(new Vector3(0f, 0f, 0f), rotation, 0.25f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			m_Axis = (m_Axis + 1) % 3;
			UpdateAxis();

			IMDraw.Disc3D(new Vector3(1f, 0f, 0f), Quaternion.identity, 0.25f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
        }

		IMDraw.LabelShadowed(Vector3.zero, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, m_AxisString);
	}

	public override string GetInfoString()
	{
		return "3D DISC";
	}

	private void UpdateAxis ()
	{
		m_AxisString = string.Format("AXIS: {0}", ((IMDrawAxis)m_Axis).ToString());
	}
}

public class IMDrawWireSphereTest : IMDrawCallTest
{
	private Vector3 m_EulerRotation;
	private float m_T;

	public IMDrawWireSphereTest()
	{
	}

	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.WireSphere3D(new Vector3(0f, 0f, 0f), rotation, 0.25f, new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			IMDraw.WireSphere3D(new Vector3(1f, 0f, 0f), rotation, 0.25f, new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
		}
	}

	public override string GetInfoString()
	{
		return "3D WIRE SPHERE";
	}
}

public class IMDrawSphereTest : IMDrawCallTest
{
	private Vector3 m_EulerRotation;
	private float m_T;

	public IMDrawSphereTest()
	{
	}

	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.Sphere3D(new Vector3(0f, 0f, 0f), rotation, 0.25f, new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			IMDraw.Sphere3D(new Vector3(1f, 0f, 0f), rotation, 0.25f, new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
		}
	}

	public override string GetInfoString()
	{
		return "3D SPHERE";
	}
}

public class IMDrawWireEllipsoidTest : IMDrawCallTest
{
	private Vector3 m_EulerRotation;
	private float m_T;

	public IMDrawWireEllipsoidTest()
	{
	}

	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.WireEllipsoid3D(new Vector3(0f, 0f, 0f), rotation, new Vector3(0.25f,0.25f, 0.5f), new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			IMDraw.WireEllipsoid3D(new Vector3(1f, 0f, 0f), rotation, new Vector3(0.25f, 0.25f, 0.5f), new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
		}
	}

	public override string GetInfoString()
	{
		return "3D WIRE ELLIPSOID";
	}
}

public class IMDrawEllipsoidTest : IMDrawCallTest
{
	private Vector3 m_EulerRotation;
	private float m_T;

	public IMDrawEllipsoidTest()
	{
	}

	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.Ellipsoid3D(new Vector3(0f, 0f, 0f), rotation, new Vector3(0.25f, 0.25f, 0.5f), new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			IMDraw.Ellipsoid3D(new Vector3(1f, 0f, 0f), rotation, new Vector3(0.25f, 0.25f, 0.5f), new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
		}
	}

	public override string GetInfoString()
	{
		return "3D ELLIPSOID";
	}
}

public class IMDrawWireCapsuleTest : IMDrawCallTest
{
	private Vector3 m_EulerRotation;
	private int m_Axis;
	private float m_T;
	private string m_AxisString;

	public IMDrawWireCapsuleTest ()
	{
		UpdateAxis();
	}

	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.WireCapsule3D(new Vector3(0f, 0f, 0f), rotation, 0.5f, 0.125f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			m_Axis = (m_Axis + 1) % 3;
			UpdateAxis();

			IMDraw.WireCapsule3D(new Vector3(1f, 0f, 0f), Quaternion.identity, 0.5f, 0.125f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
		}

		IMDraw.LabelShadowed(Vector3.zero, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, m_AxisString);
	}

	public override string GetInfoString()
	{
		return "3D WIRE CAPSULE";
	}

	private void UpdateAxis()
	{
		m_AxisString = string.Format("AXIS: {0}", ((IMDrawAxis)m_Axis).ToString());
	}
}

public class IMDrawCapsuleTest : IMDrawCallTest
{
	private Vector3 m_EulerRotation;
	private int m_Axis;
	private float m_T;
	private string m_AxisString;

	public IMDrawCapsuleTest()
	{
		UpdateAxis();
	}

	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.Capsule3D(new Vector3(0f, 0f, 0f), rotation, 0.5f, 0.125f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			m_Axis = (m_Axis + 1) % 3;
			UpdateAxis();

			IMDraw.Capsule3D(new Vector3(1f, 0f, 0f), Quaternion.identity, 0.5f, 0.125f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
		}

		IMDraw.LabelShadowed(Vector3.zero, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, m_AxisString);
	}

	public override string GetInfoString()
	{
		return "3D CAPSULE";
	}

	private void UpdateAxis()
	{
		m_AxisString = string.Format("AXIS: {0}", ((IMDrawAxis)m_Axis).ToString());
	}
}

public class IMDrawWireCylinderTest : IMDrawCallTest
{
	private Vector3 m_EulerRotation;
	private int m_Axis;
	private float m_T;
	private string m_AxisString;

	public IMDrawWireCylinderTest()
	{
		UpdateAxis();
	}

	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.WireCylinder3D(new Vector3(0f, 0f, 0f), rotation, 0.5f, 0.125f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			m_Axis = (m_Axis + 1) % 3;
			UpdateAxis();

			IMDraw.WireCylinder3D(new Vector3(1f, 0f, 0f), Quaternion.identity, 0.5f, 0.125f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
		}

		IMDraw.LabelShadowed(Vector3.zero, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, m_AxisString);
	}

	public override string GetInfoString()
	{
		return "3D WIRE CYLINDER";
	}

	private void UpdateAxis()
	{
		m_AxisString = string.Format("AXIS: {0}", ((IMDrawAxis)m_Axis).ToString());
	}
}

public class IMDrawCylinderTest : IMDrawCallTest
{
	private Vector3 m_EulerRotation;
	private int m_Axis;
	private float m_T;
	private string m_AxisString;

	public IMDrawCylinderTest()
	{
		UpdateAxis();
	}

	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.Cylinder3D(new Vector3(0f, 0f, 0f), rotation, 0.5f, 0.125f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			m_Axis = (m_Axis + 1) % 3;
			UpdateAxis();

			IMDraw.Cylinder3D(new Vector3(1f, 0f, 0f), Quaternion.identity, 0.5f, 0.125f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
		}

		IMDraw.LabelShadowed(Vector3.zero, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, m_AxisString);
	}

	public override string GetInfoString()
	{
		return "3D CYLINDER";
	}

	private void UpdateAxis()
	{
		m_AxisString = string.Format("AXIS: {0}", ((IMDrawAxis)m_Axis).ToString());
	}
}

public class IMDrawMeshTest : IMDrawCallTest
{
	public Mesh m_Mesh;
	private Vector3 m_EulerRotation;
	private float m_T;

	public IMDrawMeshTest(Mesh mesh)
	{
		m_Mesh = mesh;
    }

	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);
		Vector3 size = new Vector3(0.25f, 0.25f, 0.25f);

		IMDraw.Mesh(m_Mesh, new Vector3(0f, 0f, 0f), rotation, size, new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			IMDraw.Mesh(m_Mesh, new Vector3(1f, 0f, 0f), Quaternion.identity, size, new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
		}
	}

	public override string GetInfoString()
	{
		return "3D MESH";
	}
}

public class IMDrawLabelTest : IMDrawCallTest
{
	private float m_T;

	public override void Execute(float deltaTime)
	{
		float screenX = Mathf.Round((float)Screen.width * 0.75f);
		float screenY = Mathf.Round((float)Screen.height * 0.75f);

		IMDraw.LabelShadowed(screenX, screenY, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, string.Format("LABEL AT SCREEN POSITION\n{0}, {1}", screenX, screenY));

		IMDraw.LabelShadowed(Vector3.zero, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "LABEL AT WORLD POSITION\n0, 0, 0");

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			screenY += 40f;
			IMDraw.LabelShadowed(screenX, screenY, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, string.Format("LABEL AT SCREEN POSITION\n{0}, {1}", screenX, screenY), 1f);

			IMDraw.LabelShadowed(new Vector3(0f, 0.2f, 0f), Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, "LABEL AT WORLD POSITION\n0, 0.2, 0", 1f);
			m_T += 2f;
		}
	}

	public override string GetInfoString()
	{
		return "LABEL TEST";
	}
}

public abstract class IMDrawAxialPrimitiveTest : IMDrawCallTest
{
	protected Vector3 m_EulerRotation;
	protected int m_Axis;
	protected float m_T;
	protected string m_AxisString;

	public IMDrawAxialPrimitiveTest()
	{
		UpdateAxis();
	}

	protected void UpdateAxis()
	{
		m_AxisString = string.Format("AXIS: {0}", ((IMDrawAxis)m_Axis).ToString());
	}
}

public class IMDrawWirePyramid3DTest : IMDrawAxialPrimitiveTest
{
	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.WirePyramid3D(new Vector3(0f, 0f, 0f), rotation, 0.5f, 0.25f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			m_Axis = (m_Axis + 1) % 3;
			UpdateAxis();

			IMDraw.WirePyramid3D(new Vector3(1f, 0f, 0f), Quaternion.identity, 0.5f, 0.25f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
		}

		IMDraw.LabelShadowed(Vector3.zero, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, m_AxisString);
	}

	public override string GetInfoString()
	{
		return "3D WIREFRAME PYRAMID";
	}
}

public class IMDrawPyramid3DTest : IMDrawAxialPrimitiveTest
{
	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.Pyramid3D(new Vector3(0f, 0f, 0f), rotation, 0.5f, 0.25f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			m_Axis = (m_Axis + 1) % 3;
			UpdateAxis();

			IMDraw.Pyramid3D(new Vector3(1f, 0f, 0f), Quaternion.identity, 0.5f, 0.25f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
		}

		IMDraw.LabelShadowed(Vector3.zero, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, m_AxisString);
	}

	public override string GetInfoString()
	{
		return "3D PYRAMID";
	}
}

public class IMDrawWireRhombus3DTest : IMDrawAxialPrimitiveTest
{
	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.WireRhombus3D(new Vector3(0f, 0f, 0f), rotation, 0.5f, 0.25f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			m_Axis = (m_Axis + 1) % 3;
			UpdateAxis();

			IMDraw.WireRhombus3D(new Vector3(1f, 0f, 0f), Quaternion.identity, 0.5f, 0.25f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
		}

		IMDraw.LabelShadowed(Vector3.zero, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, m_AxisString);
	}

	public override string GetInfoString()
	{
		return "3D WIREFRAME RHOMBUS";
	}
}

public class IMDrawRhombus3DTest : IMDrawAxialPrimitiveTest
{
	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.Rhombus3D(new Vector3(0f, 0f, 0f), rotation, 0.5f, 0.25f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			m_Axis = (m_Axis + 1) % 3;
			UpdateAxis();

			IMDraw.Rhombus3D(new Vector3(1f, 0f, 0f), Quaternion.identity, 0.5f, 0.25f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
		}

		IMDraw.LabelShadowed(Vector3.zero, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, m_AxisString);
	}

	public override string GetInfoString()
	{
		return "3D RHOMBUS";
	}
}

public class IMDrawWireCone3DTest : IMDrawAxialPrimitiveTest
{
	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.WireCone3D(new Vector3(0f, 0f, 0f), rotation, 0.5f, 0.25f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			m_Axis = (m_Axis + 1) % 3;
			UpdateAxis();

			IMDraw.WireCone3D(new Vector3(1f, 0f, 0f), Quaternion.identity, 0.5f, 0.25f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
		}

		IMDraw.LabelShadowed(Vector3.zero, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, m_AxisString);
	}

	public override string GetInfoString()
	{
		return "3D WIREFRAME CONE";
	}
}

public class IMDrawCone3DTest : IMDrawAxialPrimitiveTest
{
	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);
		m_EulerRotation.z = m_EulerRotation.y = m_EulerRotation.x;

		Quaternion rotation = Quaternion.Euler(m_EulerRotation);

		IMDraw.Cone3D(new Vector3(0f, 0f, 0f), rotation, 0.5f, 0.25f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f));

		m_T -= deltaTime;

		if (m_T < 0f)
		{
			m_Axis = (m_Axis + 1) % 3;
			UpdateAxis();

			IMDraw.Cone3D(new Vector3(1f, 0f, 0f), Quaternion.identity, 0.5f, 0.25f, (IMDrawAxis)m_Axis, new Color(1f, 1f, 1f, 0.5f), 1f);
			m_T += 2f;
		}

		IMDraw.LabelShadowed(Vector3.zero, Color.white, LabelPivot.MIDDLE_CENTER, LabelAlignment.CENTER, m_AxisString);
	}

	public override string GetInfoString()
	{
		return "3D CONE";
	}
}


public class IMDrawArc3DTest : IMDrawAxialPrimitiveTest
{
	public override void Execute(float deltaTime)
	{
		m_EulerRotation.x = Mathf.Repeat(m_EulerRotation.x + (90f * deltaTime), 360f);

		m_T -= deltaTime;

		IMDraw.Arc3D(Vector3.zero, Quaternion.identity, 0.5f, m_EulerRotation.x, Color.red);

		m_EulerRotation.y = Mathf.Repeat(m_EulerRotation.y + (90f * deltaTime), 360f);

		IMDraw.Arc3D(Vector3.zero, Quaternion.identity, 1f, 1.5f, m_EulerRotation.y, 45f, Color.green);

		if (m_T < 0f)
		{ 
			IMDraw.Arc3D(Vector3.zero, Quaternion.identity, 2f, 2.5f, 0f, 180f, Color.blue, 1f);

			m_T += 2f;
		}
	}

	public override string GetInfoString()
	{
		return "3D ARC";
	}

}