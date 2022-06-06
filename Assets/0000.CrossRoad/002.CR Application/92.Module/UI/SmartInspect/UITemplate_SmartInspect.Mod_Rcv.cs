using Definition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

namespace Module.UI
{
    public partial class UITemplate_SmartInspect : RootUI
    {
        /// <summary>
        /// Mode Rcv 활성화
        /// </summary>
        public void ModRcv_Active()
        {
            ModRcv_ResetBasePosition();

            ModRcv_TogglePanel(0, true);
            ModRcv_TogglePanel(1, false);   // TODO :: CHECK :: 1번 보강 정보 삭제 예정
            ModRcv_TogglePanel(2, false);
            ModRcv_TogglePanel(3, false);
            ModRcv_TogglePanel(4, false);
            ModRcv_TogglePanel(5, false);   // TODO :: CHECK :: 5번 <- 1번 이관 예정
        }

        public void ModRcv_ResetBasePosition()
        {
            if (!m_moduleElements.m_rcvElement.root.activeSelf) return;

            m_moduleElements.m_rcvElement.m_issueCountPanels.ForEach(x => x.ReturnBasePosition());
            m_moduleElements.m_rcvElement.m_issueListPanels.ForEach(x => x.ReturnBasePosition());
        }

        public void ModRcv_ToggleIssueList()
        {
            if (!m_moduleElements.m_rcvElement.root.activeSelf) return;

            m_moduleElements.m_rcvElement.m_issueListPanels.ForEach(x => x.root.SetActive(!x.root.activeSelf));
        }

        public void ModRcv_ToggleIssueList(bool isOn)
        {
            if (!m_moduleElements.m_rcvElement.root.activeSelf) return;

            m_moduleElements.m_rcvElement.m_issueListPanels.ForEach(x => x.root.SetActive(isOn));
        }

        /// <summary>
        /// index //
        /// 0 : RepairList //
        /// 1 : ReinforceList //
        /// 2 : objStatus //
        /// 3 : dimension //
        /// 4 : drawing print //
        /// </summary>
        /// <param name="index"></param>
        /// <param name="isOn"></param>
        private void ModRcv_TogglePanel(int index, bool isOn)
        {
            List<GameObject> toggleRoots = _ModRcv_GetToggleRoot(index);

            if(toggleRoots != null)
            {
                toggleRoots.ForEach(x => x.SetActive(isOn));
            }

            MRcv_ToggleLBar(index, isOn);
        }

        private List<GameObject> _ModRcv_GetToggleRoot(int index)
        {
            List<GameObject> result = null;

            switch(index)
            {
                case 0:
                    result = new List<GameObject>();
                    result.Add(m_moduleElements.m_rcvElement.m_rcvCount);
                    //result.Add(m_moduleElements.m_rcvElement.m_rcvList);    // 둘 다 같이 끄고 키는 구조 변경
                    break;

                // TODO :: CHECK :: 보강 정보 삭제 예정
                case 1:
                    result = new List<GameObject>();
                    result.Add(m_moduleElements.m_rcvElement.m_reinCount);
                    //result.Add(m_moduleElements.m_rcvElement.m_reinList);
                    break;

                case 2:
                    result = new List<GameObject>();
                    result.Add(m_general.m_objStatus.root);
                    break;

                    // 3
                    // 4

                    // TODO :: CHECK :: 1번으로 이전
                case 5:
                    result = new List<GameObject>();
                    m_moduleElements.m_rcvElement.m_issueListPanels.ForEach(x =>
                    {
                        result.Add(x.root);
                    });
                    break;
            }

            return result;
        }

    }
}
