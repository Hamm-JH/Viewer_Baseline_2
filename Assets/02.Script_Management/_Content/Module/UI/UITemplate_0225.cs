using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
	using UnityEngine.UI;

	public class UITemplate_0225 : AUI
	{
		[SerializeField] Text m_segment;
		[SerializeField] Text m_line;
		[SerializeField] Text m_description;
		[SerializeField] List<GameObject> childElements_1;

		public override void OnStart()
		{
			Debug.LogWarning("UITemplate OnStart");
		}

		public override void SetObjectData_Tunnel(GameObject selected)
		{
			if (selected == null) return;

			Debug.LogError($"selected name : {selected.name}");
			string seg = "";
			string line = "";

			SetTunnelData(selected.name, out seg, out line);

			m_segment.text = seg;
			m_line.text = line;
		}

		public override void TogglePanelList(int _index, GameObject _exclusive)
		{
			if(_index == 1)
			{
				childElements_1.ForEach(x => x.SetActive( (x != _exclusive) ? false : x.activeSelf));
			}
		}

		private void SetTunnelData(string name, out string _seg, out string _line)
		{
			_seg = "";
			_line = "";

			string[] spl = name.Split(',');

			_seg = spl[0];
			_line = spl[1];
		}
	}
}
