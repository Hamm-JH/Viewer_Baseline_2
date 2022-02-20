using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definition.Data
{
	using Platform.Feature._Input;

	public partial class CoreManagement : IData
	{
		[Header("Resource :: Input")]
		/// <summary>
		/// 생성된 입력 인스턴스 리스트
		/// </summary>
		[SerializeField] List<IInput> m_inputsource;

		public List<IInput> Inputsource { get => m_inputsource; set => m_inputsource=value; }

		/// <summary>
		/// 입력 리소스를 입력 root 아래에 할당
		/// </summary>
		/// <param name="_source"> GameObject </param>
		/// <param name="_code"> Code::Input </param>
		public void SetInputResource(GameObject _source, IInput _code, Management.Events.InputEvents _events)
		{
			if (m_rootInput == null)
			{
				Debug.LogError("rootInput is null");
				return;
			}
			if (_source == null || _code == null)
			{
				Debug.LogError("_source or _code is null");
				return;
			}

			_source.transform.SetParent(m_rootInput.transform);

			m_inputsource.Add(_code);

			_code.OnStart(ref _events);
		}

	}
}
