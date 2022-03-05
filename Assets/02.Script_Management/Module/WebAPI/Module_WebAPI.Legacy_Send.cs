﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.WebAPI
{
    using Definition;
    using Management;
    using Utilities;
	using View;

	/// <summary>
	/// 템플릿
	/// </summary>
	public partial class Module_WebAPI : AModule
    {
        public void SendRequest(SendRequestCode _requestCode, params object[] arguments)
        {
            switch (_requestCode)
            {
                #region Default Selection step

                case SendRequestCode.SelectObject:      // 객체 선택
                    {
                        // null값은 arguments에 안 넘어올수 있음. 그래서 비활성화
                        //LimitArgument(SendRequestCode.SelectObject.ToString(), 1, arguments);

                        Func_SelectObject(Parser.Parse<GameObject>(arguments[0]));
                    }
                    break;

                case SendRequestCode.SelectIssue:       // 1005 : 손상/보강 선택
                    {
                        LimitArgument(SendRequestCode.SelectIssue.ToString(), 3, arguments);

                        Func_SelectIssue(
                            Parser.Parse<string>(arguments[0]),
                            Parser.Parse<string>(arguments[1]),
                            Parser.Parse<string>(arguments[2]));
                    }
                    break;

                case SendRequestCode.SelectObject6Shape:      // TODO lst UW / 면 방향 카메라 이동
                    {
                        LimitArgument(SendRequestCode.SelectObject6Shape.ToString(), 1, arguments);

                        Func_SelectObject6Shape(Parser.Parse<string>(arguments[0]));
                    }
                    break;

                // TODO Date 0928 : 객체 선택 위치 넘기는거 고민 필요
                // Pin 선택시 Location 정보 넘기는
                case SendRequestCode.SelectSurfaceLocation:   // 완 ; 9면 선택
                    {
                        LimitArgument(SendRequestCode.SelectSurfaceLocation.ToString(), 1, arguments);

                        Func_SelectSurfaceLocation(Parser.Parse<string>(arguments[0]));
                    }
                    break;

                    #endregion
            }

        }


        private void LimitArgument(string _codeName, int _limit, object[] _arguments)
        {
            if (_arguments.Length < _limit)
            {
                throw new System.Exception($"{_codeName} arg not less {_limit}");
            }
        }

        // O :: 객체 선택 (3D, Issue) 포함
        private void Func_SelectObject(GameObject _obj)
        {
            string objectName = "";

            if (_obj != null)
            {
                objectName = _obj.name;
            }

            List<string> codes = ContentManager.Instance.RequestAvailableSurface();
            string code = "";
            int index = codes.Count;
            for (int i = 0; i < index; i++)
            {
                code += $"{codes[i]}";
                if (i != index-1)
                {
                    code += ",";
                }
            }

            //Debug.Log(code);

            string arg = "";

            if (_obj != null)
            {
                //Debug.Log(31);
                Debug.Log($"[Send.SelectObject] // Arg :: {_obj.name}");
                arg = string.Format($"SelectObject/{_obj.name}/{code}");
                Debug.Log(arg);
            }
            else
            {
                //Debug.Log(32);
                Debug.Log($"[Send.SelectObject] // Arg :: null");
                arg = string.Format($"SelectObject");
            }

#if UNITY_EDITOR

#else
        // 프론트에 전달
        UnityRequest(arg);
#endif
        }


        // O :; 손상 정보 선택 (위에 거랑 인자만 다른가?)
        private void Func_SelectIssue(string _orderCode, string _partName, string _ynRecover)
        {
            string issueOrderCode = _orderCode;
            string issuePartName = _partName;
            string issueYnRecover = _ynRecover;

            string arg = string.Format($"SelectIssue/{issueOrderCode}/{issuePartName}/{issueYnRecover}");
            Debug.Log($"[Send.SelectIssue] // Arg :: {arg}");

#if UNITY_EDITOR

#else
            UnityRequest(arg);
#endif
        }

        // - :: 진행중 :: 임시 데이터 모아서 보낸 상태
        private void Func_SelectObject6Shape(string _shapeCode)
        {
            string shapeCode = _shapeCode;

            // TODO 1020 : 6면 선택시 해당 객체의 특정 부재의 특정 6면의 정보 반환
            List<string> targetIssueList = ContentManager.Instance.GetTargetIssues(shapeCode);

            string stringResult = "";
            for (int i = 0; i < targetIssueList.Count; i++)
            {
                if (i != 0)
                {
                    stringResult += "/";
                }
                stringResult += targetIssueList[i];
            }

            // TODO 1019 140
            Debug.Log($"[Send.SelectObject6Shape] : Arg shapeCode {shapeCode}");
            string arg = string.Format("SelectObject6Shape/{0}/{1}", shapeCode, stringResult);
#if UNITY_EDITOR
#else
        UnityRequest(arg);
#endif
        }

        // - :: 진행중 :: 이 코드가 여기서 데이터를 수동으로 수집하는 과정이 필요할까?
        private void Func_SelectSurfaceLocation(string _locationCode)
        {
            // 잘 보니 데이터를 할당하는 과정이 써져있는데.. 그냥 이벤트 걸렸을때 이벤트 관리자에서 데이터 빼와서 데이터 할당하고 보내면 되지않나..?

            string locationCode = _locationCode;
            int locationIndex = int.Parse(locationCode);

            //ContentManager.Instance.CacheIssueEntity.issueData.DcLocation = locationIndex;

            //==================================================

            Issue_Selectable issueEntity = ContentManager.Instance.CacheIssueEntity;
            Transform currentSelectedObj = ContentManager.Instance.SelectedObject;

            //manager.UISubManager.GuideLineController.ChangeCameraDirection(issueEntity.issueData.DcMemberSurface, currentSelectedObj.GetComponent<ObjectData>().Bound);

            ContentManager.Instance.CacheIssueEntity.SwitchRender(true);

            string positionVector = string.Format($"{issueEntity.transform.position.x.ToString()},{issueEntity.transform.position.y.ToString()},{issueEntity.transform.position.z.ToString()}");
            issueEntity.Issue._PositionVector = positionVector;


            Debug.Log($"SendRequest SelectSurfaceLocation : locationCode {locationCode}, positionVector {positionVector}");
            string arg = string.Format("SelectSurfaceLocation/{0}/{1}", locationCode, positionVector);
#if UNITY_EDITOR
            //WebPage.ReceiveRequest(arg);
            //SendRequest(SendRequestCode.SetPinVector);
#else
        UnityRequest(arg);
#endif
        }
    }
}
