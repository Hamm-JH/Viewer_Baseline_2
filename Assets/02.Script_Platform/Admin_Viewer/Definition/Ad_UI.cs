using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdminViewer
{
	/// <summary>
	/// enum 정의
	/// </summary>
	
	public enum Ad_UIType
	{

	}

	/// <summary>
	/// Admin Viewer 패널타입
	/// </summary>
	public enum Ad_PanelType
	{
		Null = -1,
		b1 = 0,
		b2 = 1,
		bm = 2,
		s5m1 = 3,
		s5b1 = 4,
		s5b2 = 5,
	}

	public enum Ad_TableType
	{
		dmg,
		rcv,
		rein,
	}
}
