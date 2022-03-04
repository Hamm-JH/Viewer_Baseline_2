using UnityEngine;
using System;
using System.Collections;

[AddComponentMenu("IMDraw/Examples/IMDraw Camera Controller"), DisallowMultipleComponent]
public class IMDrawCameraController : MonoBehaviour
{
	public enum CameraMode
	{
		FIRST_PERSON_SHOOTER,
		FULL_3D,
	}

	public enum MouseButton
	{
		LEFT = 0,
		RIGHT = 1,
		MIDDLE = 2,
	}

	public Camera			m_Camera;

	[Header("Settings")]

	public float			m_MovementSpeed = 1.0f; // Units per second
	public float			m_FastMovementSpeed = 2.0f; // Units per second
	public float			m_RollSpeed = 90.0f; // Degrees per second
	public float			m_MouseXAxisSpeed = 3.0f;
	public float			m_MouseYAxisSpeed = 3.0f;
	public bool				m_InvertY = false;
	public CameraMode		m_CameraMode = CameraMode.FIRST_PERSON_SHOOTER;
	public bool				m_ShowFPS = true;
	public bool				m_ForceVsync = false;

	[Header("Key bindings")]

	public MouseButton		m_CameraControlMouseButton = MouseButton.RIGHT; // 0 = left, 1 = right, 2 = middle
	public KeyCode			m_KeyboardControlToggle = KeyCode.None;
	public KeyCode			m_KeyForward = KeyCode.W;
	public KeyCode			m_KeyBack = KeyCode.S;
	public KeyCode			m_KeyLeft = KeyCode.A;
	public KeyCode			m_KeyRight = KeyCode.D;
	public KeyCode			m_KeyUp = KeyCode.R;
	public KeyCode			m_KeyDown = KeyCode.F;
	public KeyCode			m_KeyRollLeft = KeyCode.Q;
	public KeyCode			m_KeyRollRight = KeyCode.E;
	public KeyCode			m_KeyScreenshot = KeyCode.F12;

	private Transform		m_CameraTransform;
	private float			m_FPSUpdateInterval = 1f;
	private float			m_FPSAccum   = 0; // FPS accumulated over the interval
	private int				m_FPSFrames  = 0; // Frames drawn over the interval
	private float			m_FPSTimeleft; // Left time for current interval
	private string			m_FPSString;
	private GUIStyle		m_GUIStyle = null;
	private Vector3			m_EulerRotation;
	private bool			m_CursorLocked;

	private bool			m_ForceCameraUpdate;

	public void SetPosition (Vector3 position)
	{
		m_CameraTransform.position = position;
		m_ForceCameraUpdate = true;
    }

	public void SetRotation (Vector3 eulerRotation)
	{
		m_EulerRotation = eulerRotation;
		
		m_CameraTransform.rotation = Quaternion.Euler(m_EulerRotation);
		m_ForceCameraUpdate = true;
    }

	void Awake ()
	{
		if (m_Camera != null)
		{
			m_CameraTransform = m_Camera.transform;
			
			m_EulerRotation = m_CameraTransform.rotation.eulerAngles;
			
			if (m_EulerRotation.x >= 180.0f)
				m_EulerRotation.x -= 360.0f;

			m_EulerRotation.x = Mathf.Clamp(m_EulerRotation.x, -90f, 90f);			

			Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware); // This was originally added because of a horrible Unity issues with mouse cursor and the editor

			m_FPSTimeleft = m_FPSUpdateInterval;

			Application.runInBackground = true;

			if (m_ForceVsync)
			{
				//Application.targetFrameRate = 60;
				QualitySettings.vSyncCount = 1;
			}

			m_ForceCameraUpdate = true;
        }
		else
		{
			Debug.LogWarning("IMDrawCameraController: Warning! Camera reference missing!");
		}
	}

	void Update ()
	{
		if (m_Camera == null)
			return;

		if (m_KeyboardControlToggle != KeyCode.None)
		{
			if (Input.GetKeyDown(m_KeyboardControlToggle))
			{
				m_CursorLocked = !m_CursorLocked;
				Cursor.visible = !m_CursorLocked;
				Cursor.lockState = m_CursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
			}

			Input.GetMouseButton((int)m_CameraControlMouseButton);
		}
		else
		{
			m_CursorLocked = Input.GetMouseButton((int)m_CameraControlMouseButton);

			Cursor.visible = !m_CursorLocked;
			Cursor.lockState = m_CursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
		}
		
		if (!Cursor.visible || m_ForceCameraUpdate)
		{
			m_ForceCameraUpdate = false;
			UpdateCamera();
		}

		if (m_KeyScreenshot != KeyCode.None && Input.GetKey(m_KeyScreenshot)) // Note: KeyCode.Print does not appear to work at all
		{
			DateTime dateTime = System.DateTime.Now;

			string path = string.Format("Screenshot_{0}-{1:00}-{2:00}_{3:00}-{4:00}-{5:00}.png",
										 dateTime.Year,
										 dateTime.Month,
										 dateTime.Day,
										 dateTime.Hour,
										 dateTime.Minute,
										 dateTime.Second);

			#if UNITY_2017_1_OR_NEWER
			ScreenCapture.CaptureScreenshot(path);
			#else
			Application.CaptureScreenshot(path);
			#endif
		}
	}

	void LateUpdate ()
	{
		if (m_ShowFPS)
		{
			float deltaTime = Time.unscaledDeltaTime;

			m_FPSTimeleft -= deltaTime;
			m_FPSAccum += 1.0f / deltaTime;
			++m_FPSFrames;
			
			// Interval ended - update GUI text and start new interval
			if (m_FPSTimeleft <= 0.0)
			{
				float fps = m_FPSAccum / m_FPSFrames;

				if (m_GUIStyle != null)
				{
					if (fps < 30)
						m_GUIStyle.normal.textColor = Color.yellow;
					else 
						if (fps < 10)
							m_GUIStyle.normal.textColor = Color.red;
					else
						m_GUIStyle.normal.textColor = Color.green;
				}

				m_FPSTimeleft += m_FPSUpdateInterval;

				if (m_FPSTimeleft <= 0f)
				{
					m_FPSTimeleft = m_FPSUpdateInterval;
                }

				m_FPSAccum = 0.0F;
				m_FPSFrames = 0;

				m_FPSString = System.String.Format("{0} FPS", System.Math.Round(fps, 2));
			} 

			IMDraw.LabelShadowed(Screen.width - 10, 10f, Color.white, LabelPivot.UPPER_RIGHT, LabelAlignment.RIGHT, m_FPSString);
		}
	}

	/*
	void OnGUI ()
	{
		if (m_ShowFPS)
		{
			if (m_GUIStyle == null)
			{
				m_GUIStyle = new GUIStyle(GUI.skin.label);
				m_GUIStyle.clipping = TextClipping.Overflow;
				m_GUIStyle.fontSize = 12;
			}

			GUI.Label(new Rect(10.0f,10.0f,100.0f,100.0f),m_FPS,m_GUIStyle);
		}
	}
	*/

	private void UpdateCamera()
	{
		switch (m_CameraMode)
		{
			case CameraMode.FIRST_PERSON_SHOOTER:
				UpdateFPSCamera();
				break;

			default:
				UpdateFull3DCamera();
				break;
		}
	}

	private void UpdateFPSCamera()
	{
		m_EulerRotation.y = Mathf.Repeat(m_EulerRotation.y + (Input.GetAxis ("Mouse X") * m_MouseXAxisSpeed), 360f);
		m_EulerRotation.x = Mathf.Clamp(m_EulerRotation.x + Input.GetAxis("Mouse Y") * m_MouseYAxisSpeed * (m_InvertY ? 1.0f : -1.0f), -90f, 90f);
		m_EulerRotation.z = 0f;

		m_CameraTransform.rotation = Quaternion.Euler(m_EulerRotation);

		// Update camera position
		Vector3 position = m_CameraTransform.position;
		Vector3 forward = m_CameraTransform.forward;
		Vector3 right = m_CameraTransform.right;
		//Vector3 up = m_CameraTransform.up;

		float deltaTime = Time.unscaledDeltaTime;
		float speed = Input.GetKey(KeyCode.LeftShift) ? m_FastMovementSpeed * deltaTime : m_MovementSpeed * deltaTime;

		if (Input.GetKey(m_KeyForward))
		{
			position += forward * speed;
		}
		if (Input.GetKey(m_KeyBack))
		{
			position -= forward * speed;
		}
		if (Input.GetKey(m_KeyLeft))
		{
			position -= right * speed;
		}
		if (Input.GetKey(m_KeyRight))
		{
			position += right * speed;
		}

		if (Input.GetKey(m_KeyUp))
		{
			position.y += speed;
		}
		if (Input.GetKey(m_KeyDown))
		{
			position.y -= speed;
		}

		m_CameraTransform.position = position;
	}

	private void UpdateFull3DCamera()
	{
		float deltaTime = Time.unscaledDeltaTime;

		// Update camera rotation
		Quaternion rotation = m_CameraTransform.rotation;
		
		rotation *= Quaternion.AngleAxis(Input.GetAxis ("Mouse X") * m_MouseXAxisSpeed, Vector3.up);
		rotation *= Quaternion.AngleAxis(Input.GetAxis ("Mouse Y") * m_MouseYAxisSpeed * (m_InvertY ? 1.0f : -1.0f), Vector3.right);

		if (Input.GetKey(m_KeyRollLeft))
		{
			rotation *= Quaternion.AngleAxis(m_RollSpeed * deltaTime,Vector3.forward);
		}
		if (Input.GetKey(m_KeyRollRight))
		{
			rotation *= Quaternion.AngleAxis(-m_RollSpeed * deltaTime,Vector3.forward);
		}
		
		NormaliseQuat(ref rotation);

		m_CameraTransform.rotation = rotation;

		m_EulerRotation = m_CameraTransform.rotation.eulerAngles;
		
		// Update camera position
		Vector3 position = m_CameraTransform.position;
		Vector3 forward = m_CameraTransform.forward;
		Vector3 right = m_CameraTransform.right;
		Vector3 up = m_CameraTransform.up;

		float speed = Input.GetKey(KeyCode.LeftShift) ? m_FastMovementSpeed * deltaTime : m_MovementSpeed * deltaTime;

		if (Input.GetKey(m_KeyForward))
		{
			position += forward * speed;
		}
		if (Input.GetKey(m_KeyBack))
		{
			position -= forward * speed;
		}
		if (Input.GetKey(m_KeyLeft))
		{
			position -= right * speed;
		}
		if (Input.GetKey(m_KeyRight))
		{
			position += right * speed;
		}

		if (Input.GetKey(m_KeyUp))
		{
			position += up * speed;
		}
		if (Input.GetKey(m_KeyDown))
		{
			position -= up * speed;
		}

		m_CameraTransform.position = position;
	}

	public static void NormaliseQuat (ref Quaternion q)
	{
		float k = (float)(1.0 / Math.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w));

		q.x *= k;
		q.y *= k;
		q.z *= k;
		q.w *= k;
	}
}
