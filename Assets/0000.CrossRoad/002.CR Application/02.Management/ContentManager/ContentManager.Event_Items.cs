using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Management
{
	using Definition;
	using Items;
    using Module.Item;

    public partial class ContentManager : IManager<ContentManager>
	{
		// 테스트 코드의 LocGuide 코드 가져오기

		public GameObject target;

		public GameObject guide;

		public GameObject compass;

		//public ViewRotations vRot;
		public UIEventType uType;

		// Start is called before the first frame update
		//void Start()
		//{

		//	SetCamera(target, target.transform.rotation.eulerAngles, uType);
		//	SetGuide(target, target.transform.rotation.eulerAngles, uType);
		//}

		public void SetMode(UIEventType _uType)
		{
			uType = _uType;

			SetCamera(target, target.transform.rotation.eulerAngles, uType);
			SetGuide(target, target.transform.rotation.eulerAngles, uType);
		}

		public void SetCamera(GameObject _obj, Vector3 _baseAngle, UIEventType _uType)
		{
			Vector3 center = default(Vector3);
			Camera cam = Camera.main;
			Bounds bound = default(Bounds);

			MeshRenderer render;
			if (_obj.TryGetComponent<MeshRenderer>(out render))
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

		public void SetGuide(GameObject _obj, Vector3 _baseAngle, UIEventType _uType)
		{
			Vector3 targetPos = target.transform.position;
			Vector3 targetScale = target.transform.localScale;


			guide.transform.position = targetPos;
			guide.transform.rotation = Quaternion.Euler(_baseAngle);
			guide.transform.Rotate(Angle.Set(_uType));
			guide.transform.Translate(Positions.SetLocal(_obj, _uType));



			Vector3 setScale = Scales.SetQuad(target, uType);
			guide.transform.localScale = setScale;

			Controller_LocationGuide _lg = guide.GetComponent<Controller_LocationGuide>();
			_lg.SetCubeLine(setScale);

		}

		/// <summary>
		/// compass 토글
		/// </summary>
		/// <param name="_isOn"></param>
		public void ToggleCompass(bool _isOn)
        {
			if (compass == null)
            {
				compass = Module<Module_Items>().Compass.CompassUIRoot;
            }

			compass.SetActive(_isOn);
        }

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.A))
        //    {
		//		ToggleCompass(true);
        //    }
		//	else if (Input.GetKeyDown(KeyCode.S))
        //    {
		//		ToggleCompass(false);
        //    }
        //}
    }
}
