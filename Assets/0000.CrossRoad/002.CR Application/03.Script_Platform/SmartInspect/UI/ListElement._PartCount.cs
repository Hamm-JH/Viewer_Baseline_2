using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartInspect
{
    using Definition;
    using Management;
    using Management.Content;
    using Platform.Bridge;
    using Platform.Tunnel;

    /// <summary>
    /// SmartInspect의 UI 내부 리스트 요소 클래스
    /// </summary>
    public partial class ListElement
    {
        private void PartCount_Init()
        {
            PlatformCode pCode = MainManager.Instance.Platform;
            if (Platforms.IsBridgePlatform(pCode))
            {
                // 1 분류 코드 리스트 갖고온다.
                List<BridgeCode> cList = Bridges.GetCodeList();
                // 2 손상정보 리스트도 갖고온다.
                List<Definition._Issue.Issue> issues = GetIssueList(m_catrgory, _countData.m_bCode);
                //SmartInspectManager.Instance.Module<Module_Model>(ModuleID.Model).DmgData;

                // 3 분류 코드 리스트 기반으로 요소 리스트를 생성한다.
                Dictionary<BridgeCode, int> result = GetCountList_Bridge(cList, issues);

                // 4 리스트 생성
                SetCountList_Bridge(result);

                _countData.m_pBar.ChangeValue(100);
                _countData.m_pBar_Text.text = $"{issues.Count}";
            }
            else if (Platforms.IsTunnelPlatform(pCode))
            {
                // 분류 코드 리스트 갖고온다.
                List<TunnelCode> cList = Tunnels.GetCodeList();
                // 손상정보 리스트도 갖고온다.
                List<Definition._Issue.Issue> issues = GetIssueList(m_catrgory, _countData.m_tCode);
                //SmartInspectManager.Instance.Module<Module_Model>(ModuleID.Model).DmgData;

                // 분류 코드 리스트 기반으로 요소 리스트를 생성한다.
                Dictionary<TunnelCode, int> result = GetCountList_Tunnel(cList, issues);

                SetCountList_Tunnel(result);

                _countData.m_pBar.ChangeValue(100);
                _countData.m_pBar_Text.text = $"{issues.Count}";
            }
            else
            {
                throw new Definition.Exceptions.PlatformNotDefinedException(pCode);
            }
        }

        

        #region Bridge - part count

        /// <summary>
        /// 3 이슈 분류
        /// </summary>
        /// <param name="_cList"></param>
        /// <param name="_issues"></param>
        /// <returns></returns>
        private Dictionary<BridgeCode, int> GetCountList_Bridge(
            List<BridgeCode> _cList, List<Definition._Issue.Issue> _issues)
        {
            Dictionary<BridgeCode, int> result = new Dictionary<BridgeCode, int>();

            // result에 dictionary 요소 추가
            _cList.ForEach(x =>
            {
                result.Add(x, 0);
            });

            foreach (Definition._Issue.Issue issue in _issues)
            {
                // 이 정보의 코드를 가져온다.
                BridgeCode code = issue.__PartBridgeCode;

                // 코드가 result 키에 속할 경우
                if (result.ContainsKey(code))
                {
                    // value값 상승
                    result[code]++;
                }
            }

            return result;
        }

        /// <summary>
        /// 4 교량 :: 카운트 리스트 요소 생성
        /// </summary>
        /// <param name="_result"></param>
        private void SetCountList_Bridge(Dictionary<BridgeCode, int> _result)
        {
            // 변수 순회
            foreach (BridgeCode key in _result.Keys)
            {
                // 요소의 값이 0이 아닐 경우에만 index를 생성한다.
                if(_result[key] != 0)
                {
                    // 각 요소마다 새 인스턴스 생성, 생성된 인스턴스 부모할당
                    GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("UI/SmartInspect/Inspect_Records"), m_contentRoot);
                    RecordElement element = obj.GetComponent<RecordElement>();
                    string pName = Bridges.ConvertLv4String(key);
                
                    Packet_Record packet = new Packet_Record(0, _result[key], pName, key, _countData.m_tgElement, _countData.m_toggleResourceElement, m_rootUI);

                    element.Init(packet);

                    _countData.m_elements.Add(element);
                }
            }
        }

        #endregion

        #region Tunnel - part count

        /// <summary>
        /// 터널 :: 카운트 리스트 데이터 가공
        /// </summary>
        /// <param name="_cList"></param>
        /// <param name="_issues"></param>
        /// <returns></returns>
        private Dictionary<TunnelCode, int> GetCountList_Tunnel(
            List<TunnelCode> _cList, List<Definition._Issue.Issue> _issues)
        {
            Dictionary<TunnelCode, int> result = new Dictionary<TunnelCode, int>();

            // result에 dictionary 요소 추가
            _cList.ForEach(x =>
            {
                result.Add(x, 0);
            });

            // issue 순회
            foreach (Definition._Issue.Issue issue in _issues)
            {
                // 이 정보의 코드를 가져온다.
                TunnelCode code = issue.__PartTunnelCode;

                // 코드가 result 키에 속할 경우
                if (result.ContainsKey(code))
                {
                    // value값 상승
                    result[code]++;
                }
            }

            return result;
        }

        /// <summary>
        /// 터널 :: 카운트 리스트 요소 생성
        /// </summary>
        /// <param name="_result"></param>
        private void SetCountList_Tunnel(Dictionary<TunnelCode, int> _result)
        {
            // 변수 순회
            foreach (TunnelCode key in _result.Keys)
            {
                // 요소의 값이 0이 아닐 경우에만 index를 생성한다.
                if(_result[key] != 0)
                {
                    // 각 요소마다 새 인스턴스 생성, 생성된 인스턴스 부모할당
                    GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("UI/SmartInspect/Inspect_Records"), m_contentRoot);
                    RecordElement element = obj.GetComponent<RecordElement>();
                    string pName = Tunnels.GetCodeName(key);

                    // 초기화를 위한 패킷 생성
                    Packet_Record packet = new Packet_Record(0, _result[key], pName, key, _countData.m_tgElement, _countData.m_toggleResourceElement, m_rootUI);

                    element.Init(packet);

                    _countData.m_elements.Add(element);
                }
            }
        }

        #endregion
    }
}
