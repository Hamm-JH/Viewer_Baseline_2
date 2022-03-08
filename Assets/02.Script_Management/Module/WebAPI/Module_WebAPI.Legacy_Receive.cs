using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.WebAPI
{
    using Definition;
    using Management;
	using System;
	using System.Linq;
	using Utilities;
	using View;

	public partial class Module_WebAPI : AModule
	{
        public void ReceiveRequest(string argument)
        {

            string[] arguments = argument.Split('/');
            string requestString = (string)arguments[0];
            ReceiveRequestCode requestCode = (ReceiveRequestCode)Enum.Parse(typeof(ReceiveRequestCode), requestString);

            if (!Enum.TryParse<ReceiveRequestCode>(requestString, out requestCode))
            {
                Debug.Log($"request code not validate");
                return;
            }

            switch (requestCode)
            {
                #region Default Selection step

                // Q
                case ReceiveRequestCode.ResetIssue:
                    Func_ResetIssue();
                    break;

                // W
                case ReceiveRequestCode.SelectObject:   // 3D 객체 선택 (Issue 포함)
                    Func_SelectObject(arguments[1]);
                    break;

                // E
                case ReceiveRequestCode.SelectIssue:    // Issue 객체 선택
                    Func_SelectIssue(arguments[1]);
                    break;

                // R
                case ReceiveRequestCode.SelectObject6Shape:     // 6면 객체 선택
                    Func_Receive_SelectObject6Shape(arguments[1]);
                    break;

                // T
                case ReceiveRequestCode.InformationWidthChange: // 정보창 폭 변경 수신
                    Func_InformationWidthChange(Parsers.Parse<float>(arguments[1]));
                    break;

                #endregion

                #region Register step

                // I
                case ReceiveRequestCode.ChangePinMode:  // PinMode 설정 변경
                    Func_ChangePinMode();
                    break;

                // O
                case ReceiveRequestCode.InitializeRegisterMode: // 등록 모드 시작
                    Func_InitializeRegisterMode();
                    break;

                case ReceiveRequestCode.FinishRegisterMode: // 등록 단계 종료
                    Func_FinishRegisterMode();
                    break;

                    #endregion
            }
        }

		#region Functions

		#region 3DObject
		private void Find_3DObject(string _name, out GameObject _obj)
        {
            _obj = ContentManager.Instance._ModelObjects.Find(
                x => x == GameObject.Find(_name));
        }
		#endregion

		#region IssueObject

		private void Find_IssueObject(string _code, out GameObject _obj)
        {
            _obj = ContentManager.Instance._IssueObjects.Find(
                x => x == GameObject.Find(_code));
        }

		#endregion

		#region ResetIssue

		private void Func_ResetIssue()
        {
            //ContentManager.Instance.InitCamPosition();

            // Issue 보기상태 복구
            ContentManager.Instance.Reset_IssueObject();
        }

		#endregion

		#region SelectObject

		private void Func_SelectObject(string _name)
        {
            GameObject obj3D;
            Find_3DObject(_name, out obj3D);

            GameObject objIssue;
            Find_IssueObject(_name, out objIssue);

            // 선택 객체 :: 3D 객체인 경우
            if (obj3D)
            {
                // 객체 선택 이벤트를 전달
                ContentManager.Instance.Select3DObject(obj3D);
                Debug.Log($"[Receive.SelectObject] : objectName {obj3D.name}");
            }
            // 선택 객체 :: Issue 객체인 경우
            else if (objIssue)
            {
                // 점검정보 선택 이벤트를 전달
                Debug.LogError("TODO");
            }

        }

		#endregion

		#region SelectIssue

		private void Func_SelectIssue(string _name)
        {
            GameObject issue;
            Find_IssueObject(_name, out issue);

            if (issue)
            {
                ContentManager.Instance.SelectIssue(issue, false);
            }
        }

        private void Func_Receive_SelectObject6Shape(string _code)
        {
            // 현재 선택된 객체 가져오기
            GameObject selectedObj = ContentManager.Instance._SelectedObj;

            // 객체가 선택되지 않았으면 실행되지 않음
            if (selectedObj == null)
			{
                Debug.LogError("6Shape :: Object not set");
                return;
			}

            Cameras.SetCamera(_code);

            Send6ShapeRequest(_code);
        }

        private void Send6ShapeRequest(string _code)
		{
            ViewRotations vCode = ViewRotations.Null;

            // 시점변환 코드 수집
            vCode = Parsers.GetStringToViewCode(_code);

            // 코드 변환 결과가 null이 아닐 경우에만 실행
            if (vCode != ViewRotations.Null)
            {
                SendRequest(SendRequestCode.SelectObject6Shape, _code);
                Debug.Log($"[Receive.SelectObject6Shape] :: Arg {vCode.ToString()}");
            }
        }

		#endregion

		#region InformationWidthChange

		private void Func_InformationWidthChange(float _value)
        {
            float width = _value;

            ContentManager.Instance.GetRightWebUIWidth(width);

            Debug.Log($"[Receive.InformationWidthChange] : width {width}");
        }

		#endregion

		private void Func_ChangePinMode()
        {
            GameObject obj = ContentManager.Instance._SelectedObj;

            //Transform currentSelectedObj = ContentManager.Instance.SelectedObject;
            MeshRenderer meshRenderer;

            var moduleList = EventManager.Instance._ModuleList;

            if (moduleList.Contains(ModuleCode.WorkQueue))
            {
                if(!moduleList.Contains(ModuleCode.Work_Pinmode))
				{
                    moduleList.Add(ModuleCode.Work_Pinmode);

                    // TODO 0308 Module.Graphic에서 스카이박스 변경 코드 작성
                    // skybox 변경
                    RenderSettings.skybox = ContentManager.Instance.PinModeSkyboxMaterial;

                    // TODO 0308 GuideCube 기존 코드에서 가져와서 구현하기
                    // GuideLine On
                    ContentManager.Instance.SwitchGuideCubeOnOff(true);

                    // TODO 0308 아래 코드 최적화
                    Bounds nB = new Bounds();
                    if (ContentManager.Instance.AppUseCase == UseCase.Bridge
                        || ContentManager.Instance.AppUseCase == UseCase.Tunnel)
                    {
                        nB = obj.GetComponent<Obj_Selectable>().Bounds;
                    }

                    Debug.Log($"_____ {obj.name}");

                    bool isEtc = false;

                    // TODO do remove
                    //string side = GetSurfaceSide(issueEntity.Issue, out isEtc);
                    //ContentManager.Instance.ChangeCameraDirection(side, nB, currentSelectedObj.parent.parent.localRotation.eulerAngles, isEtc);
                    //manager.UISubManager.GuideLineController.ChangeCameraDirection(issueEntity.issueData.DcMemberSurface, nB, currentSelectedObj.parent.parent.localRotation.eulerAngles);

                    // 선택객체 Mat White
                    if (obj.TryGetComponent<MeshRenderer>(out meshRenderer))
                    {
                        meshRenderer.material = Materials.Set(MaterialType.White);
                    }
                    for (int i = 0; i < obj.transform.childCount; i++)
                    {
                        if (obj.transform.GetChild(i).TryGetComponent<MeshRenderer>(out meshRenderer))
                        {
                            meshRenderer.material = Materials.Set(MaterialType.White);
                        }
                    }

                    // DimViewManager 설정 (선택한 객체만 보이기)
                    ContentManager.Instance.Toggle_ModelObject(UIEventType.Mode_Isolate, ToggleType.Isolate);

                    // __////////////////

                    ViewRotations vr = ViewRotations.Top;
                    int cubeDir = 0;
                    int imgIndex = 0;

                    isEtc = false;
                    // TODO do remove
                    //side = GetSurfaceSide(issueEntity.Issue, out isEtc);
                    //switch (side)
                    //{
                    //    case "Top":
                    //        vr = ViewRotations.Top;
                    //        cubeDir = 0;
                    //        imgIndex = 1;
                    //        break;

                    //    case "Bottom":
                    //        vr = ViewRotations.Bottom;
                    //        cubeDir = 1;
                    //        imgIndex = 2;
                    //        break;

                    //    case "Front":
                    //        vr = ViewRotations.Front;
                    //        cubeDir = 2;
                    //        imgIndex = 3;
                    //        break;

                    //    case "Back":
                    //        vr = ViewRotations.Back;
                    //        cubeDir = 3;
                    //        imgIndex = 4;
                    //        break;

                    //    case "Left":
                    //        vr = ViewRotations.Left;
                    //        cubeDir = 4;
                    //        imgIndex = 5;
                    //        break;

                    //    case "Right":
                    //        vr = ViewRotations.Right;
                    //        cubeDir = 5;
                    //        imgIndex = 6;
                    //        break;
                    //}

                    Transform selectObj = ContentManager.Instance.SelectedObject;
                    Vector3 _angle = selectObj.parent.parent.rotation.eulerAngles;

                    ContentManager.Instance.DirectionAngle(cubeDir, _angle); // 4 left
                    ContentManager.Instance.DirectionAngle(cubeDir, _angle); // 4
				}
                else
				{
                    moduleList.Remove(ModuleCode.Work_Pinmode);

                    //ContentManager.Instance.ViewSceneStatus = SceneStatus.Register;
                    //manager.IsPinMode = false;

                    // skybox 변경
                    RenderSettings.skybox = ContentManager.Instance.DefaultSkyboxMaterial;

                    // GuideLine Off
                    ContentManager.Instance.SwitchGuideCubeOnOff(false);
                    // 위치 초기화
                    //ContentManager.Instance.UISubManager.GuideLineController.transform.position = new Vector3(0, 0, 0);

                    // 선택객체 Mat 객체선택으로 변경
                    if (obj.TryGetComponent<MeshRenderer>(out meshRenderer))
                    {
                        meshRenderer.material = Materials.Set(MaterialType.White);
                    }
                    for (int i = 0; i < obj.transform.childCount; i++)
                    {
                        if (obj.transform.GetChild(i).TryGetComponent<MeshRenderer>(out meshRenderer))
                        {
                            meshRenderer.material = Materials.Set(MaterialType.White);
                        }
                    }

                    ViewRotations vr = ViewRotations.Top;
                    int cubeDir = 0;
                    int imgIndex = 0;

                    bool isEtc = false;
                    // TODO do remove
                    //string side = GetSurfaceSide(issueEntity.Issue, out isEtc);
                    //switch (side)
                    //{
                    //    case "Top":
                    //        vr = ViewRotations.Top;
                    //        cubeDir = 0;
                    //        imgIndex = 1;
                    //        break;

                    //    case "Bottom":
                    //        vr = ViewRotations.Bottom;
                    //        cubeDir = 1;
                    //        imgIndex = 2;
                    //        break;

                    //    case "Front":
                    //        vr = ViewRotations.Front;
                    //        cubeDir = 2;
                    //        imgIndex = 3;
                    //        break;

                    //    case "Back":
                    //        vr = ViewRotations.Back;
                    //        cubeDir = 3;
                    //        imgIndex = 4;
                    //        break;

                    //    case "Left":
                    //        vr = ViewRotations.Left;
                    //        cubeDir = 4;
                    //        imgIndex = 5;
                    //        break;

                    //    case "Right":
                    //        vr = ViewRotations.Right;
                    //        cubeDir = 5;
                    //        imgIndex = 6;
                    //        break;
                    //}

                    Transform selectObj = ContentManager.Instance.SelectedObject;
                    Vector3 _angle = selectObj.parent.parent.rotation.eulerAngles;

                    ContentManager.Instance.DirectionAngle(cubeDir, _angle); // 4 left
                    ContentManager.Instance.DirectionAngle(cubeDir, _angle); // 4

                    //viewmanager.DimSet(manager.InputManager.SelectionController.SelectedObject, vr, true);
                }
            }
            else
			{
                Debug.Log($"Invalid access");
            }

			{
                //else if (ContentManager.Instance.ViewSceneStatus == SceneStatus.Register_PinMode)
                //{
                //    //ContentManager.Instance.ViewSceneStatus = SceneStatus.Register;
                //    ////manager.IsPinMode = false;

                //    //// skybox 변경
                //    //RenderSettings.skybox = ContentManager.Instance.DefaultSkyboxMaterial;

                //    //// GuideLine Off
                //    //ContentManager.Instance.SwitchGuideCubeOnOff(false);
                //    //// 위치 초기화
                //    ////ContentManager.Instance.UISubManager.GuideLineController.transform.position = new Vector3(0, 0, 0);

                //    //// 선택객체 Mat 객체선택으로 변경
                //    //if (currentSelectedObj.TryGetComponent<MeshRenderer>(out meshRenderer))
                //    //{
                //    //    meshRenderer.material = Materials.Set(MaterialType.White);
                //    //}
                //    //for (int i = 0; i < currentSelectedObj.childCount; i++)
                //    //{
                //    //    if (currentSelectedObj.GetChild(i).TryGetComponent<MeshRenderer>(out meshRenderer))
                //    //    {
                //    //        meshRenderer.material = Materials.Set(MaterialType.White);
                //    //    }
                //    //}

                //    //ViewRotations vr = ViewRotations.Top;
                //    //int cubeDir = 0;
                //    //int imgIndex = 0;

                //    //bool isEtc = false;
                //    //string side = GetSurfaceSide(issueEntity.Issue, out isEtc);
                //    //switch (side)
                //    //{
                //    //    case "Top":
                //    //        vr = ViewRotations.Top;
                //    //        cubeDir = 0;
                //    //        imgIndex = 1;
                //    //        break;

                //    //    case "Bottom":
                //    //        vr = ViewRotations.Bottom;
                //    //        cubeDir = 1;
                //    //        imgIndex = 2;
                //    //        break;

                //    //    case "Front":
                //    //        vr = ViewRotations.Front;
                //    //        cubeDir = 2;
                //    //        imgIndex = 3;
                //    //        break;

                //    //    case "Back":
                //    //        vr = ViewRotations.Back;
                //    //        cubeDir = 3;
                //    //        imgIndex = 4;
                //    //        break;

                //    //    case "Left":
                //    //        vr = ViewRotations.Left;
                //    //        cubeDir = 4;
                //    //        imgIndex = 5;
                //    //        break;

                //    //    case "Right":
                //    //        vr = ViewRotations.Right;
                //    //        cubeDir = 5;
                //    //        imgIndex = 6;
                //    //        break;
                //    //}

                //    //Transform selectObj = ContentManager.Instance.SelectedObject;
                //    //Vector3 _angle = selectObj.parent.parent.rotation.eulerAngles;

                //    //ContentManager.Instance.DirectionAngle(cubeDir, _angle); // 4 left
                //    //ContentManager.Instance.DirectionAngle(cubeDir, _angle); // 4

                //    ////viewmanager.DimSet(manager.InputManager.SelectionController.SelectedObject, vr, true);
                //}
                //else
                //{
                //    Debug.Log($"Invalid access");
                //}
			}
        }

        private void Func_InitializeRegisterMode()
        {
            GameObject obj = ContentManager.Instance._SelectedObj;
            //Transform objectTransform = ContentManager.Instance.SelectedObject;

            var moduleList = EventManager.Instance._ModuleList;

            if(!moduleList.Contains(ModuleCode.WorkQueue))
			{
                if(obj != null)
				{
                    Debug.Log($"[Receive.InitializeRegisterMode] : obj name {obj.name}");

                    moduleList.Add(ModuleCode.WorkQueue);

                    StartCoroutine(InitRequestMode(obj));
                }
                else
				{
                    Debug.LogError("Object not selected");
				}
			}
        }

        private void Func_FinishRegisterMode()
        {
            var moduleList = EventManager.Instance._ModuleList;

            if(moduleList.Contains(ModuleCode.WorkQueue))
			{
                moduleList.Remove(ModuleCode.WorkQueue);
                moduleList.Remove(ModuleCode.Work_Pinmode);

                StartCoroutine(FinishRequestMode());
            }
        }

        #endregion



        private string GetSurfaceSide(Definition._Issue.Issue _data, out bool isEtc)
        {
            string result = "";
            isEtc = false;

            if (_data.CdBridgeParts.Contains("Etc"))
            {
                isEtc = true;
                if (_data.CdBridgeParts.Contains("_R"))
                {
                    result = "Back";
                }
                // _L로 봄
                else
                {
                    result = "Front";
                }
            }
            else
            {
                result = _data.DcMemberSurface;
            }

            return result;
        }

        private IEnumerator InitRequestMode(GameObject _obj)
        {
            ContentManager.Instance.Function_ToggleOrthoView(true);

            yield return null;

			{
                //MeshRenderer renderer;
                //if (objectTransform.TryGetComponent<MeshRenderer>(out renderer))
                //{

                //    Bounds _b = renderer.bounds;
                //    Canvas _canvas = ContentManager.Instance._Canvas;

                //    Vector3 centerVector = _b.center;
                //    float maxValue = Mathf.Max(Mathf.Max(_b.size.x, _b.size.y), _b.size.z);

                //    //MainManager.Instance.SetCameraPosition(_b, _canvas, UIEventType.Viewport_ViewMode_TOP, )
                //    // 카메라 거리변경 전달
                //    //MainManager.Instance.MainCameraController.targetDistance = maxValue + maxValue / 10 - 2;

                //    ContentManager.Instance.ZoomCamera(centerVector, maxValue, false, true);

                //    Camera.main.orthographic = true;        // 메인 카메라 2D 변경
                //    Camera.main.orthographicSize = maxValue;
                //}
			}

            Bounds bound = Parsers.Calculate(_obj);
            MainManager.Instance.ResetCamdata_targetOffset();
            MainManager.Instance.UpdateCamData_maxOffset(bound.size.magnitude);

            // TODO 0308 표면값 추출
            List<string> codes = new List<string> { "Top" };
            //List<string> codes = Dim.DimScript.Instance.RequestAvailableSurface();

            string _fCode = codes.First();

            Cameras.SetCamera(_fCode);

            // 선택 객체 제외하고 모두 끄기 (Isolate)
            ContentManager.Instance.Toggle_ModelObject(UIEventType.Mode_Isolate_Off, ToggleType.Isolate);

            // 선택된 객체에 해당하는 손상/보강 객체만 표시하기
            ContentManager.Instance.Toggle_Issues(IssueVisualizeOption.SelectedTarget);

            yield break;

			{
                //string code = "";
                //int cubeDir = 0;
                //int imgIndex = 0;

                //Debug.Log($"***** Dim codes :: ");
                //for (int i = 0; i < codes.Count; i++)
                //{
                //    Debug.Log($"Dim {i} : {codes[i]}");
                //}
                //Debug.Log($"***** Code end");

                //Debug.Log($"obj Transform : {objectTransform.name}"); // 객체명
                //Debug.Log($"obj Transform parent : {objectTransform.parent.name}"); // 32,1
                //Debug.Log($"obj Transform parent parent : {objectTransform.parent.parent.name}"); // Segment
                //Debug.Log($"obj Transform parent parent angle : {objectTransform.parent.parent.rotation.eulerAngles}"); // Segment

                //Vector3 _angle = objectTransform.parent.parent.rotation.eulerAngles;

                //if (codes.Count != 0)
                //{
                //    code = codes[0];

                //    switch (code)
                //    {
                //        case "Top":
                //            {
                //                cubeDir = 0;
                //                imgIndex = 1;
                //            }
                //            break;

                //        case "Bottom":
                //            {
                //                cubeDir = 1;
                //                imgIndex = 2;
                //            }
                //            break;
                //        case "Front":
                //            {
                //                cubeDir = 2;
                //                imgIndex = 3;
                //            }
                //            break;
                //        case "Back":
                //            {
                //                cubeDir = 3;
                //                imgIndex = 4;
                //            }
                //            break;
                //        case "Left":
                //            {
                //                cubeDir = 4;
                //                imgIndex = 5;
                //            }
                //            break;
                //        case "Right":
                //            {
                //                cubeDir = 5;
                //                imgIndex = 6;
                //            }
                //            break;
                //    }
                //}
                //else
                //{
                //    code = "Left";
                //    cubeDir = 4;
                //    imgIndex = 4;
                //}
                //ContentManager.Instance.CacheIssueEntity.Issue.CdBridge = "model code";
                //ContentManager.Instance.CacheIssueEntity.Issue.CdBridgeParts = objectTransform.name;
                //ContentManager.Instance.CacheIssueEntity.Issue.DcMemberSurface = code; // Left



                //ContentManager.Instance.DirectionAngle(cubeDir, _angle); // 4 left
                //ContentManager.Instance.DirectionAngle(cubeDir, _angle); // 4

                //if (ContentManager.Instance.AppUseCase == UseCase.Bridge)
                //{
                //    ContentManager.Instance.CacheIssueEntity.issueData.CdBridge = ContentManager.Instance.RootObject.transform.name;
                //}
                //else if (ContentManager.Instance.AppUseCase == UseCase.Tunnel)
                //{
                //    ContentManager.Instance.CacheIssueEntity.issueData.CdBridge = Data.Viewer.Cache.Instance.models.ModelName.name;
                //}



                //ContentManager.Instance.SetInspectionImage(imgIndex);     // 우측 6면 표시 슬라이드 이미지
                                                                          //ContentManager.Instance.UIManager.OnClickDamagedRegist();       // 카메라 등록전 상태 호출

                //objectTransform.gameObject.SetActive(false);

                //yield break;
			}
        }

        private IEnumerator FinishRequestMode()
		{
            // TODO 0307 카메라 세팅 변경 (기존 참조)
            ContentManager.Instance.Function_ToggleOrthoView(false);

            yield return null;

            Bounds bound = ContentManager.Instance._Model.CenterBounds;
            MainManager.Instance.ResetCamdata_targetOffset();
            MainManager.Instance.UpdateCamData_maxOffset(bound.size.magnitude);

            ContentManager.Instance.SetCameraCenterPosition();
            ContentManager.Instance.Reset_ModelObject();

            // TODO 0307 Dim 비활성화
            //ContentManager.Instance.Toggle_Dimension(false);

            // 이슈 정보 다시 로드
            ReceiveRequest("ResetIssue");



            yield break;
		}


        
    }
}
