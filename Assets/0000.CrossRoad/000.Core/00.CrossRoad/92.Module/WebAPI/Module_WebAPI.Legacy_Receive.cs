using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.WebAPI
{
    using Definition;
    using Definition.Control;
    using Management;
    using Module.Interaction;
    using Module.Model;
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

                case ReceiveRequestCode.ChangeTab:
                    ChangeTab(arguments[1]);
                    break;

                // W
                case ReceiveRequestCode.SelectObject:   // 3D 객체 선택 (Issue 포함)
                    Func_SelectObject(arguments[1]);
                    break;

                // E
                case ReceiveRequestCode.SelectIssue:    // Issue 객체 선택
                    // TODO :: CHECK :: 웹 뷰어에서 Issue 상세보기 선택시 접근하는 루트
                    Func_SelectIssue(arguments[1]);
                    break;

                // R
                case ReceiveRequestCode.SelectObject6Shape:     // 6면 객체 선택 // 사용자가 6면중 하나를 선택할때 발생
                    Debug.Log($"arg0 : {arguments[0]}");
                    Debug.Log($"arg1 : {arguments[1]}");
                    Func_Receive_SelectObject6Shape(arguments[1]);
                    break;

                // T
                case ReceiveRequestCode.InformationWidthChange: // 정보창 폭 변경 수신
                    if(arguments.Length >= 2)
					{
                        Func_InformationWidthChange(Parsers.Parse<float>(arguments[1]));
					}
                    else
					{
                        Func_InformationWidthChange();
                    }
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
            _obj = ContentManager.Instance._IssueObjects.Find(x => x.name == _code);
            //_obj = ContentManager.Instance._IssueObjects.Find(x => x == GameObject.Find(_code));  // 터널에서 쓴 코드

            //Debug.Log($"code : {_code}");
            //List<GameObject> issues = ContentManager.Instance._IssueObjects;
            //for (int i = 0; i < issues.Count; i++)
            //{
            //    Debug.Log($"issue : {issues[i].name}");
            //}

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

        #region ChangeTab

        public void ChangeTab(string _value)
        {
            GameObject selected = EventManager.Instance._SelectedObject;
            Module_Model model = ContentManager.Instance.Module<Module_Model>();
            string selectedName = selected ? selected.name : null;

            List<Definition._Issue.Issue> dmgs = model.DmgData;
            List<Definition._Issue.Issue> rcvs = model.RcvData;
            List<Definition._Issue.Issue> all = model.AllIssues;

            PlatformCode pCode = MainManager.Instance.Platform;
            bool isOnTarget = false;
            if (Platforms.IsDemoWebViewer(pCode))
            {
                isOnTarget = true;
            }

            // Web에서 탭을 누르는 시점에 선택한 개체가 없는 경우도 존재한다.
            if (_value == "DMG")
            {
                if(isOnTarget)
                {
                    Issues.WP_Setup_Dmgs_WithTarget(dmgs, rcvs, all, selectedName);
                }
                else
                {
                    Issues.WP_Setup_Dmgs_WithTarget(dmgs, rcvs, all, null);
                }
            }
            else if(_value == "RCV")
            {
                if(isOnTarget)
                {
                    Issues.WP_Setup_Rcvs_WithTarget(dmgs, rcvs, all, selectedName);
                }
                else
                {
                    Issues.WP_Setup_Rcvs_WithTarget(dmgs, rcvs, all, null);
                }
            }
            else if(_value == "ALL")
            {
                Issues.WP_Setup_ALL(dmgs, rcvs, all);
            }
            else
            {
                Debug.LogError("unknown argument");
            }
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
                ContentManager.Instance.SelectIssue(objIssue);
                Debug.Log($"[Receive.SelectIssue] : objectName {objIssue.name}");
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
                ContentManager.Instance.SelectIssue(issue);
            }
        }

        private void Func_Receive_SelectObject6Shape(string _code)
        {
            // 현재 선택된 객체 가져오기
            GameObject selectedObj = EventManager.Instance._CacheObject;

            //// 객체가 선택되지 않았으면 실행되지 않음
            //if (selectedObj == null)
			//{
            //    Debug.LogError("6Shape :: Object not set");
            //    return;
			//}

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

        private void Func_InformationWidthChange()
        {
            //float width = _value;

            //ContentManager.Instance.GetRightWebUIWidth(width);

            Debug.Log($"[Receive.InformationWidthChange] : width {0}");
        }

        private void Func_InformationWidthChange(float _value)
        {
            float width = _value;

            ContentManager.Instance.GetRightWebUIWidth(width);

            Debug.Log($"[Receive.InformationWidthChange] : width {width}");
        }

		#endregion

		#region PinMode

		private void Func_ChangePinMode()
        {
            var moduleList = EventManager.Instance._ModuleList;

            if (moduleList.Contains(ModuleCode.WorkQueue))
            {
                if(!moduleList.Contains(ModuleCode.Work_Pinmode))
				{
                    moduleList.Add(ModuleCode.Work_Pinmode);

                    Toggle_ChangePinMode(true);
				}
                else
				{
                    moduleList.Remove(ModuleCode.Work_Pinmode);

                    Toggle_ChangePinMode(false);
                }
            }
            else
			{
                Debug.Log($"Invalid access");
            }
        }

        /// <summary>
        /// 핀모드 OnOff
        /// </summary>
        /// <param name="isMode"></param>
        private void Toggle_ChangePinMode(bool isMode)
		{
            var moduleList = EventManager.Instance._ModuleList;

            GameObject obj = ContentManager.Instance._SelectedObj;

            List<string> codes = new List<string>();
            string _fCode = "";

            if (obj != null)
            {
                //List<string> codes = ContentManager.Instance.RequestAvailableSurface();
                codes = GetSurfaceStrings(obj);
                _fCode = codes.First();
                //int index = codes.Count;
                //for (int i = 0; i < index; i++)
                //{
                //    _fCode += $"{codes[i]}";
                //    if (i != index - 1)
                //    {
                //        _fCode += ",";
                //    }
                //}
            }

            //List<string> codes = new List<string> { "Top" };
            UIEventType uType = Parsers.OnParse(_fCode);

            if (isMode)
			{
                //GameObject obj = ContentManager.Instance._SelectedObj;
                Vector3 angle = ContentManager.Instance._SelectedAngle;

                ContentManager.Instance._Items.SetGuide(obj, angle, uType);
			}

            ContentManager.Instance._Items.OnUpdateState(moduleList);
		}

		#endregion

		#region InitializeRegisterMode

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

        private IEnumerator InitRequestMode(GameObject _obj)
        {
            // 카메라 뷰 Orthogonal
            ContentManager.Instance.Function_ToggleOrthoView(true);

            yield return null;

            // 카메라 설정 조정
            Bounds bound = Parsers.Calculate(_obj);
            MainManager.Instance.ResetCamdata_targetOffset();
            MainManager.Instance.UpdateCamData_maxOffset(bound.size.magnitude);

            List<string> codes = GetSurfaceStrings(_obj);
            //List<string> codes = new List<string> { "Top", "Bottom", "Front", "Back", "Left", "Right" };
            //List<string> codes = Dim.DimScript.Instance.RequestAvailableSurface();

            string _fCode = codes.First();

            Cameras.SetCamera(_fCode);

            // 선택 객체 제외하고 모두 끄기 (Isolate)
            ContentManager.Instance.Cache_SelectedObject();
            ContentManager.Instance.Reset_ModelObject();
            //ContentManager.Instance.
            ContentManager.Instance.Toggle_ModelObject(UIEventType.Mode_Isolate_Off, ToggleType.Isolate);

            // 버튼 바를 아래로 내린다.
            ContentManager.Instance.ButtonBar_Toggle(false);

            // 선택된 객체에 해당하는 손상/보강 객체만 표시하기
            ContentManager.Instance.Toggle_Issues(IssueVisualizeOption.SelectedTarget);

            // 핀모드 Off
            Toggle_ChangePinMode(false);

            yield break;
        }

        /// <summary>
        /// 초기화 해야하는 객체의 표시가능 표면을 
        /// </summary>
        /// <param name="_target"></param>
        /// <returns></returns>
        private List<string> GetSurfaceStrings(GameObject _target)
        {
            List<string> result = new List<string>();

            PlatformCode pCode = MainManager.Instance.Platform;
            if(Platforms.IsBridgePlatform(pCode))
            {
                result = Platform.Bridge.Bridges.GetPartSurfaces(_target);
            }
            else if(Platforms.IsTunnelPlatform(pCode))
            {
                result = Platform.Tunnel.Tunnels.GetPartSurfaces(_target);
            }

            return result;
        }

        #endregion

        #region FinishRegisterMode

        private void Func_FinishRegisterMode()
        {
            var moduleList = EventManager.Instance._ModuleList;

            if(moduleList.Contains(ModuleCode.WorkQueue))
			{
                moduleList.Remove(ModuleCode.WorkQueue);
                moduleList.Remove(ModuleCode.Work_Pinmode);

                // 핀모드 off
                Toggle_ChangePinMode(false);

                StartCoroutine(FinishRequestMode());
            }
        }

        private IEnumerator FinishRequestMode()
        {
            ContentManager.Instance.Function_ToggleOrthoView(false);

            yield return null;

            Bounds bound = ContentManager.Instance._Model.CenterBounds;
            MainManager.Instance.ResetCamdata_targetOffset();
            MainManager.Instance.UpdateCamData_maxOffset(bound.size.magnitude);
            Cameras.SetCameraMode(CameraModes.BIM_ISO);

            ContentManager.Instance.SetCameraCenterPosition();
            ContentManager.Instance.Reset_ModelObject();

            // 버튼 바 올리기
            ContentManager.Instance.ButtonBar_Toggle(true);

            ContentManager.Instance.Remove_Cache();

            // 이슈 정보 다시 로드
            ReceiveRequest("ResetIssue");



            yield break;
        }

        #endregion

        #endregion
    }
}
