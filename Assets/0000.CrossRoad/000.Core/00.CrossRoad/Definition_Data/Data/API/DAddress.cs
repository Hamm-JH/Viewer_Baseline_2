using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.API
{
	public class DAddress : AAPI
	{
		/// <summary>
		/// 터널 형태 (아치, 박스)
		/// </summary>
		public string fgUF001;

		/// <summary>
		/// 터널 재질 (콘크리트)
		/// </summary>
		public string fgLM001;

		/// <summary>
		/// 터널 주소
		/// </summary>
		public string nmAddress;

		/// <summary>
		/// 터널 이름
		/// </summary>
		public string nmTunnel;

		/// <summary>
		/// 주 사진 id
		/// </summary>
		public string mp_fid;
		/// <summary>
		/// 주 사진 ftype
		/// </summary>
		public string mp_ftype;
		/// <summary>
		/// 주 사진 그룹
		/// </summary>
		public string mp_fgroup;

	}
}
