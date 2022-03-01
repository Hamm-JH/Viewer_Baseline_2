using Definition;
using Kino;
using Management;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

public enum ReceiveRequestCode
{
    Null,

    ResetIssue,         // 손상/보강 리셋

    SelectObject,       // 3D 객체 선택
    SelectIssue,        // 손상/보강 선택
    SelectObject6Shape, // 6면 객체 선택
    SelectSurfaceLocation,  // 9면 선택
    InformationWidthChange, // 정보창 폭 변경 수신

    SetIssueStatus,         // 손상 / 보강 상태 변경
    ChangePinMode,          // PinMode 설정 변경
    InitializeRegisterMode, // 등록 모드 시작
    FinishRegisterMode,     // 등록 단계 종료

}

public partial class WebManager : MonoBehaviour
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
                {
                    ContentManager.Instance.InitCamPosition();
                    //ContentManager.Instance.InitCamPosition(ContentManager.Instance.Root3DObject);

                    ContentManager.Instance.SetIssueObject();
                }   break;

            // W
            case ReceiveRequestCode.SelectObject:            // 3D 객체 선택
                {


                    GameObject selectedObject = GameObject.Find(arguments[1]);
                    ContentManager.Instance.Select3DObject(selectedObject.transform);  // 웹에서 받은 객체명 기준 3D 객체선택 단계 실행

                    Debug.Log($"WM ReceiveRequest SelectObject : objectName {selectedObject.name}");
                }   break;

            // E
            case ReceiveRequestCode.SelectIssue:
                {
                    GameObject damageCode = GameObject.Find(arguments[1]);

                    ContentManager.Instance.SelectIssueEntity(damageCode.transform, false);

                    Debug.Log($"WM ReceiveRequest SelectIssue : issueName {damageCode.name}");
                }   break;

            // R
            case ReceiveRequestCode.SelectObject6Shape:     // 6면 객체 선택
                {
                    //return;
                    Transform selectObj = ContentManager.Instance.SelectedObject;
                    Vector3 _angle = selectObj.parent.parent.rotation.eulerAngles;

                    string shapeCode = arguments[1];

                    Debug.Log($"WM ReceiveRequest SelectObject6Shape : shapeCode {shapeCode}");


                    // 시점변환 코드 실행
                    switch (shapeCode)
                    {
                        case "Top":
                        case "TO":
                            ContentManager.Instance.DirectionAngle(0, _angle);
                            ContentManager.Instance.DirectionAngle(0, _angle);
                            //MainCameraController.DirectionAngle(0, _angle);
                            //ViewCubeCameraController.DirectionAngle(1, _angle);
                            //ContentManager.Instance.InputManager.SelectionController.CacheIssueEntity.issueData.DcMemberSurface = "Top";
                            ContentManager.Instance.DcMemberSurface = "Top";
                            ContentManager.Instance.SetInspectionImage(1);
                            break;

                        case "Bottom":
                        case "BO":
                            ContentManager.Instance.DirectionAngle(1, _angle);
                            ContentManager.Instance.DirectionAngle(1, _angle);
                            ContentManager.Instance.DcMemberSurface = "Bottom";
                            ContentManager.Instance.SetInspectionImage(2);
                            break;

                        case "Front":
                        case "FR":
                            ContentManager.Instance.DirectionAngle(2, _angle);
                            ContentManager.Instance.DirectionAngle(2, _angle);
                            ContentManager.Instance.DcMemberSurface = "Front";
                            ContentManager.Instance.SetInspectionImage(3);
                            break;

                        case "Back":
                        case "BA":
                            ContentManager.Instance.DirectionAngle(3, _angle);
                            ContentManager.Instance.DirectionAngle(3, _angle);
                            ContentManager.Instance.DcMemberSurface = "Back";
                            ContentManager.Instance.SetInspectionImage(4);
                            break;

                        case "Left":
                        case "LE":
                            ContentManager.Instance.DirectionAngle(4, _angle);
                            ContentManager.Instance.DirectionAngle(4, _angle);
                            ContentManager.Instance.DcMemberSurface = "Left";
                            ContentManager.Instance.SetInspectionImage(5);
                            break;

                        case "Right":
                        case "RI":
                            ContentManager.Instance.DirectionAngle(5, _angle);
                            ContentManager.Instance.DirectionAngle(5, _angle);
                            ContentManager.Instance.DcMemberSurface = "Right";
                            ContentManager.Instance.SetInspectionImage(6);
                            break;
                    }

                    SendRequest(SendRequestCode.SelectObject6Shape, shapeCode);
                }
                break;

            // T
            case ReceiveRequestCode.InformationWidthChange: // 정보창 폭 변경 수신
                {
                    string widthText = (string)arguments[1];
                    float width = float.Parse(widthText);

                    ContentManager.Instance.GetRightWebUIWidth(width);

                    Debug.Log($"WM ReceiveRequest InformationWidthChange : width {width}");
                }
                break;

            #endregion

            #region Register step

            // TODO : delete 점검
            // Y
            case ReceiveRequestCode.SetIssueStatus: // 손상 / 보강 상태 변경
                {
                    string issueCode = (string)arguments[1];
                    string issueIndex = (string)arguments[2];

                    // TODO : issueCode, issueIndex 확인 필요
                    ContentManager.Instance.SetIssueCode(issueCode, issueIndex);

                    Debug.Log($"WM ReceiveRequest SetIssueStatus : issueCode {issueCode} , issueIndex {issueIndex}");
                }
                break;

            // I
            case ReceiveRequestCode.ChangePinMode:  // PinMode 설정 변경
                {
                    Issue_Selectable issueEntity = ContentManager.Instance.CacheIssueEntity;

                    Transform currentSelectedObj = ContentManager.Instance.SelectedObject;
                    MeshRenderer meshRenderer;

                    if (ContentManager.Instance.ViewSceneStatus == SceneStatus.Register)
                    {
                        ContentManager.Instance.ViewSceneStatus = SceneStatus.Register_PinMode;
                        //manager.IsPinMode = true;

                        // skybox 변경
                        RenderSettings.skybox = ContentManager.Instance.PinModeSkyboxMaterial;

                        // GuideLine On
                        ContentManager.Instance.SwitchGuideCubeOnOff(true);

                        //Debug.Log(issueEntity.issueData.DcMemberSurface);
                        //Debug.Log(currentSelectedObj.GetComponent<Obj_Selectable>().Bound);

                        Bounds nB = new Bounds();

                        if (ContentManager.Instance.AppUseCase == UseCase.Bridge
                            || ContentManager.Instance.AppUseCase == UseCase.Tunnel)
						{
                            nB = currentSelectedObj.GetComponent<Obj_Selectable>().Bounds;
                        }

                        Debug.Log($"_____ {currentSelectedObj.name}");
                        
                        bool isEtc = false;
                        string side = GetSurfaceSide(issueEntity.Issue, out isEtc);
                        ContentManager.Instance.ChangeCameraDirection(side, nB, currentSelectedObj.parent.parent.localRotation.eulerAngles, isEtc);
						//manager.UISubManager.GuideLineController.ChangeCameraDirection(issueEntity.issueData.DcMemberSurface, nB, currentSelectedObj.parent.parent.localRotation.eulerAngles);

						// 선택객체 Mat White
						if (currentSelectedObj.TryGetComponent<MeshRenderer>(out meshRenderer))
                        {
                            meshRenderer.material = Materials.Set(MaterialType.White);
                        }
                        for (int i = 0; i < currentSelectedObj.childCount; i++)
                        {
                            if (currentSelectedObj.GetChild(i).TryGetComponent<MeshRenderer>(out meshRenderer))
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
                        side = GetSurfaceSide(issueEntity.Issue, out isEtc);
                        switch (side)
                        {
                            case "Top":
                                vr = ViewRotations.Top;
                                cubeDir = 0;
                                imgIndex = 1;
                                break;

                            case "Bottom":
                                vr = ViewRotations.Bottom;
                                cubeDir = 1;
                                imgIndex = 2;
                                break;

                            case "Front":
                                vr = ViewRotations.Front;
                                cubeDir = 2;
                                imgIndex = 3;
                                break;

                            case "Back":
                                vr = ViewRotations.Back;
                                cubeDir = 3;
                                imgIndex = 4;
                                break;

                            case "Left":
                                vr = ViewRotations.Left;
                                cubeDir = 4;
                                imgIndex = 5;
                                break;

                            case "Right":
                                vr = ViewRotations.Right;
                                cubeDir = 5;
                                imgIndex = 6;
                                break;
                        }

                        Transform selectObj = ContentManager.Instance.SelectedObject;
                        Vector3 _angle = selectObj.parent.parent.rotation.eulerAngles;

                        ContentManager.Instance.DirectionAngle(cubeDir, _angle); // 4 left
                        ContentManager.Instance.DirectionAngle(cubeDir, _angle); // 4
                    }
                    else if (ContentManager.Instance.ViewSceneStatus == SceneStatus.Register_PinMode)
                    {
                        ContentManager.Instance.ViewSceneStatus = SceneStatus.Register;
                        //manager.IsPinMode = false;

                        // skybox 변경
                        RenderSettings.skybox = ContentManager.Instance.DefaultSkyboxMaterial;

                        // GuideLine Off
                        ContentManager.Instance.SwitchGuideCubeOnOff(false);
                        // 위치 초기화
                        //ContentManager.Instance.UISubManager.GuideLineController.transform.position = new Vector3(0, 0, 0);

                        // 선택객체 Mat 객체선택으로 변경
                        if (currentSelectedObj.TryGetComponent<MeshRenderer>(out meshRenderer))
                        {
                            meshRenderer.material = Materials.Set(MaterialType.White);
                        }
                        for (int i = 0; i < currentSelectedObj.childCount; i++)
                        {
                            if (currentSelectedObj.GetChild(i).TryGetComponent<MeshRenderer>(out meshRenderer))
                            {
                                meshRenderer.material = Materials.Set(MaterialType.White);
                            }
                        }

                        ViewRotations vr = ViewRotations.Top;
                        int cubeDir = 0;
                        int imgIndex = 0;

                        bool isEtc = false;
                        string side = GetSurfaceSide(issueEntity.Issue, out isEtc);
                        switch (side)
                        {
                            case "Top":
                                vr = ViewRotations.Top;
                                cubeDir = 0;
                                imgIndex = 1;
                                break;

                            case "Bottom":
                                vr = ViewRotations.Bottom;
                                cubeDir = 1;
                                imgIndex = 2;
                                break;

                            case "Front":
                                vr = ViewRotations.Front;
                                cubeDir = 2;
                                imgIndex = 3;
                                break;

                            case "Back":
                                vr = ViewRotations.Back;
                                cubeDir = 3;
                                imgIndex = 4;
                                break;

                            case "Left":
                                vr = ViewRotations.Left;
                                cubeDir = 4;
                                imgIndex = 5;
                                break;

                            case "Right":
                                vr = ViewRotations.Right;
                                cubeDir = 5;
                                imgIndex = 6;
                                break;
                        }

                        Transform selectObj = ContentManager.Instance.SelectedObject;
                        Vector3 _angle = selectObj.parent.parent.rotation.eulerAngles;

                        ContentManager.Instance.DirectionAngle(cubeDir, _angle); // 4 left
                        ContentManager.Instance.DirectionAngle(cubeDir, _angle); // 4

                        //viewmanager.DimSet(manager.InputManager.SelectionController.SelectedObject, vr, true);
                    }
                    else
                    {
                        Debug.Log($"Invalid access");
                    }
                }
                break;

            // O
            case ReceiveRequestCode.InitializeRegisterMode: // 등록 모드 시작
                {
                    Transform objectTransform = ContentManager.Instance.SelectedObject;
                    Contour contour;

                    if (ContentManager.Instance.ViewSceneStatus == SceneStatus.Ready)
                    {
                        if (objectTransform != null)
                        {
                            Debug.Log($"***** WM ReceiveRequest InitializeRegisterMode : objectTransform.name {objectTransform.name}");

                            ContentManager.Instance.ViewSceneStatus = SceneStatus.Register;



                            // TODO 0104 카메라 세팅 변경
                            ContentManager.Instance.SetCameraMode(1);
                            // 위치 줌

                            StartCoroutine(InitRequestMode(objectTransform));
                        }
                        else
                        {
                            Debug.Log("Object not selected");
                        }
                    }
                }
                break;

            case ReceiveRequestCode.FinishRegisterMode: // 등록 단계 종료
                {
                    ContentManager.Instance.ViewSceneStatus = SceneStatus.Ready;
                    Contour contour;

                    string issueID = "";

                    float gizmoScale = 0f;

                    // TODO 0104 카메라 세팅 변경
                    ContentManager.Instance.SetCameraMode(2);

                    // Dim 비활성화
                    ContentManager.Instance.Toggle_Dimension(false);

                    for (int i = 0; i < arguments.Length; i++)
                    {
                        Debug.Log($"arguments {i} : {(string)arguments[i]}");
                    }

                    // 변경 이전 버전
                    if (isUnderUpdateVersion)
                    {
                        //========================================

                        ReceiveRequest("ResetIssue");

                        Camera.main.orthographic = false;        // 메인 카메라 3D 변경

                        ContentManager.Instance.SetInspectionImage(0);     // 우측 6면 표시 슬라이드 이미지

                        ContentManager.Instance.Toggle_Issues(IssueVisualizeOption.All_ON);
                        if (Camera.main.TryGetComponent<Contour>(out contour))
                        {
                            contour.enabled = true;
                        }

                        Debug.Log($"WM ReceiveRequest FinishRegisterMode : issueID {issueID}");

                    }
                    // 변경 이후 버전 
                    else
                    {
                        BoolValue boolArg;
                        if (!Enum.TryParse<BoolValue>(arguments[1], out boolArg))
                        {
                            Debug.Log($"arguments 2 isnt valid : value -> {(string)arguments[1]}");
                            return;
                        }

                        if (boolArg == BoolValue.True)
                        {
                            ReceiveRequest("ResetIssue");

                            // 모든 이슈 가시화
                            ContentManager.Instance.Toggle_Issues(IssueVisualizeOption.All_ON);

                            if (Camera.main.TryGetComponent<Contour>(out contour))
                            {
                                contour.enabled = true;
                            }

                            //Debug.Log($"WM ReceiveRequest FinishRegisterMode : issueOrderCode {ContentManager.Instance.InputManager.SelectionController.CacheIssueEntity.issueData.IssueOrderCode}");
                        }
                        else
                        {
                            // 등록 취소
                            Debug.Log($"WM ReceiveRequest FinishRegisterMode : register canceled");
                        }
                    }

                    ContentManager.Instance.SetInspectionImage(0);     // 우측 6면 표시 슬라이드 이미지

                    // 2D -> 3D 변환
                    Camera.main.orthographic = false;        // 메인 카메라 2D 변경

                    // UI 리셋
                    //ContentManager.Instance.UIManager.OnClickReset();       // TODO : UI 객체 상태 리셋 ?

                    // 캐시정보 리셋
                    //ContentManager.Instance.InputManager.SelectionController.CacheIssueEntity.Reset();

                    //ContentManager.Instance.SetModel_StartEndPos();

                }
                break;

                #endregion
        }
    }


    private string GetSurfaceSide(Definition._Issue.Issue _data, out bool isEtc)
	{
        string result = "";
        isEtc = false;

        if(_data.CdBridgeParts.Contains("Etc"))
		{
            isEtc = true;
            if(_data.CdBridgeParts.Contains("_R"))
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

    private IEnumerator InitRequestMode(Transform objectTransform)
	{
        yield return null;

        MeshRenderer renderer;
        if (objectTransform.TryGetComponent<MeshRenderer>(out renderer))
        {
            Bounds b = renderer.bounds;
            Vector3 centerVector = b.center;
            float maxValue = Mathf.Max(Mathf.Max(b.size.x, b.size.y), b.size.z);

            // 카메라 거리변경 전달
            //MainManager.Instance.MainCameraController.targetDistance = maxValue + maxValue / 10 - 2;

            ContentManager.Instance.ZoomCamera(centerVector, maxValue, false, true);

            Camera.main.orthographic = true;        // 메인 카메라 2D 변경
            Camera.main.orthographicSize = maxValue;
        }

        // 표면값 전달
        List<string> codes = new List<string>{ "Top" };
        //List<string> codes = Dim.DimScript.Instance.RequestAvailableSurface();
        string code = "";
        int cubeDir = 0;
        int imgIndex = 0;

        Debug.Log($"***** Dim codes :: ");
        for (int i = 0; i < codes.Count; i++)
        {
            Debug.Log($"Dim {i} : {codes[i]}");
        }
        Debug.Log($"***** Code end");

        Debug.Log($"obj Transform : {objectTransform.name}"); // 객체명
        Debug.Log($"obj Transform parent : {objectTransform.parent.name}"); // 32,1
        Debug.Log($"obj Transform parent parent : {objectTransform.parent.parent.name}"); // Segment
        Debug.Log($"obj Transform parent parent angle : {objectTransform.parent.parent.rotation.eulerAngles}"); // Segment

        Vector3 _angle = objectTransform.parent.parent.rotation.eulerAngles;

        if (codes.Count != 0)
        {
            code = codes[0];

            switch (code)
            {
                case "Top":
                    {
                        cubeDir = 0;
                        imgIndex = 1;
                    }
                    break;

                case "Bottom":
                    {
                        cubeDir = 1;
                        imgIndex = 2;
                    }
                    break;
                case "Front":
                    {
                        cubeDir = 2;
                        imgIndex = 3;
                    }
                    break;
                case "Back":
                    {
                        cubeDir = 3;
                        imgIndex = 4;
                    }
                    break;
                case "Left":
                    {
                        cubeDir = 4;
                        imgIndex = 5;
                    }
                    break;
                case "Right":
                    {
                        cubeDir = 5;
                        imgIndex = 6;
                    }
                    break;
            }
        }
        else
        {
            code = "Left";
            cubeDir = 4;
            imgIndex = 4;
        }

        ContentManager.Instance.DirectionAngle(cubeDir, _angle); // 4 left
        ContentManager.Instance.DirectionAngle(cubeDir, _angle); // 4

        ContentManager.Instance.CacheIssueEntity.Issue.CdBridge = "model code";
        //if (ContentManager.Instance.AppUseCase == UseCase.Bridge)
        //{
        //    ContentManager.Instance.CacheIssueEntity.issueData.CdBridge = ContentManager.Instance.RootObject.transform.name;
        //}
        //else if (ContentManager.Instance.AppUseCase == UseCase.Tunnel)
        //{
        //    ContentManager.Instance.CacheIssueEntity.issueData.CdBridge = Data.Viewer.Cache.Instance.models.ModelName.name;
        //}


        ContentManager.Instance.CacheIssueEntity.Issue.CdBridgeParts = objectTransform.name;
        ContentManager.Instance.CacheIssueEntity.Issue.DcMemberSurface = code; // Left

        ContentManager.Instance.SetInspectionImage(imgIndex);     // 우측 6면 표시 슬라이드 이미지
        //ContentManager.Instance.UIManager.OnClickDamagedRegist();       // 카메라 등록전 상태 호출

        objectTransform.gameObject.SetActive(false);

        // 선택된 객체에 해당하는 손상/보강 객체만 표시하기
        ContentManager.Instance.Toggle_Issues(IssueVisualizeOption.SelectedTarget);


        yield break;
	}
}
    