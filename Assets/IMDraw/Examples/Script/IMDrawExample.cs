using UnityEngine;
using System.Collections;

[AddComponentMenu("IMDraw/Examples/IMDraw Example"), DisallowMultipleComponent]
public class IMDrawExample : MonoBehaviour
{
	[Header("Title")]
	public Color m_TitleBG;
	public Color m_TitleBGOutline;

	[Header("Logo")]
	public Texture2D m_LogoTexture;
	public Color m_LogoColor;
	public float m_LogoSize;

	[Header("Demo scenes")]
	public GameObject [] m_Scene;

	private string m_Title;
	private int m_CurrentScene = 0;

	void Start ()
	{
		m_Title = string.Format("IMMEDIATE MODE DRAW v{0} DEMO\nUNITY v{1}", IMDraw.VERSION, Application.unityVersion);

		UpdateActiveScene();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			--m_CurrentScene;

			if (m_CurrentScene < 0)
				m_CurrentScene = m_Scene.Length - 1;

			UpdateActiveScene();
		}

		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			++m_CurrentScene;

			if (m_CurrentScene >= m_Scene.Length)
				m_CurrentScene = 0;

			UpdateActiveScene();
		}
	}

	void LateUpdate ()
	{
		float screenWidth = (float)Screen.width;
		float screenHeight = (float)Screen.height;

		Vector2 titleSize = IMDraw.GetLabelSize(m_Title);
		IMDraw.Rectangle2D(new Rect (10f - 4f, 10f - 4f, titleSize.x + 8f, titleSize.y + 8f), m_TitleBG, m_TitleBGOutline);
		IMDraw.LabelShadowed(10f, 10f, Color.white, LabelPivot.UPPER_LEFT, LabelAlignment.LEFT, m_Title);

		Rect imageRect = new Rect(screenWidth - m_LogoSize - 10f, screenHeight - m_LogoSize - 10f, m_LogoSize, m_LogoSize);

		IMDraw.Image2D(m_LogoTexture, imageRect, m_LogoColor);
		IMDraw.RectangleOutline2D(imageRect, Color.black);
	}

	private void UpdateActiveScene()
	{
		for (int i = 0; i < m_Scene.Length; ++i)
		{
			if (m_Scene[i] != null)
			{
				m_Scene[i].SetActive(i == m_CurrentScene);
			}
		}
	}
}
