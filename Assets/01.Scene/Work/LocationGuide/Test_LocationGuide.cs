using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
	using Definition;
	using Items;

	public class Test_LocationGuide : MonoBehaviour
	{
		public GameObject target;

		public GameObject guide;

		//public ViewRotations vRot;
		public UIEventType uType;

		// Start is called before the first frame update
		void Start()
		{

			SetCamera(target, target.transform.rotation.eulerAngles, uType);
			SetGuide(target, target.transform.rotation.eulerAngles, uType);
		}

		public void SetMode(UIEventType _uType)
		{
			uType = _uType;

			SetCamera(target, target.transform.rotation.eulerAngles, uType);
			SetGuide(target, target.transform.rotation.eulerAngles, uType);
		}

		/// <summary>
		/// 개선된 카메라 할당코드
		/// </summary>
		/// <param name="_obj"></param>
		/// <param name="_baseAngle"></param>
		/// <param name="_uType"></param>
		public void SetCamera(GameObject _obj, Vector3 _baseAngle, UIEventType _uType)
		{
			Vector3 center = default(Vector3);
			Camera cam = Camera.main;
			Bounds bound = default(Bounds);

			MeshRenderer render;
			if(_obj.TryGetComponent<MeshRenderer>(out render))
			{
				bound = render.bounds;
			}

			center = bound.center;

			cam.orthographic = true;

			cam.transform.position = center;
			cam.transform.rotation = Quaternion.Euler(_baseAngle);
			cam.transform.Rotate(Angle.Set(_uType));

			cam.transform.Translate(Vector3.back * 5f);
		}

		/// <summary>
		/// LocationGuide 할당코드
		/// </summary>
		/// <param name="_obj"></param>
		/// <param name="_baseAngle"></param>
		/// <param name="_uType"></param>
		public void SetGuide(GameObject _obj, Vector3 _baseAngle, UIEventType _uType)
		{
			Vector3 targetPos = target.transform.position;
			Vector3 targetScale = target.transform.localScale;


			guide.transform.position = targetPos;
			guide.transform.rotation = Quaternion.Euler( _baseAngle );
			guide.transform.Rotate(Angle.Set(_uType));
			guide.transform.Translate(Positions.SetLocal(_obj, _uType));
			


			Vector3 setScale = Scales.SetQuad(target, uType);
			guide.transform.localScale = setScale;

			Controller_LocationGuide _lg = guide.GetComponent<Controller_LocationGuide>();
			_lg.SetCubeLine(setScale);

		}
	}
}
