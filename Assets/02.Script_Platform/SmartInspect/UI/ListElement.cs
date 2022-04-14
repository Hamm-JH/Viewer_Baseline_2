using Management;
using Module.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartInspect
{
    /// <summary>
    /// SmartInspect의 UI 내부 리스트 요소 클래스
    /// </summary>
    [System.Serializable]
    public partial class ListElement : MonoBehaviour
    {
        /*
         * 리스트 요소는 초기화 지시를 받을 시에
         * 해당 요소가 가지고 있는 데이터를 기반으로 데이터 가공 수행
         */
        public enum Template
        {
            PartCount,
            IssueList,
        }

        /// <summary>
        /// 내부 분류용 카테고리
        /// </summary>
        public enum Category
        {
            ALL,
            DMG,
            RCV,
            REIN,
        }

        public Template m_template;
        public Category m_catrgory;
        public UITemplate_SmartInspect m_rootUI;
        public Transform m_contentRoot;

        public void Init()
        {
            //ContentManager.Instance._Model.DmgData

            Debug.Log($"hello list {gameObject.name}");

            ClearElement();

            // 템플릿
            // 1 파트 카운트

            switch(m_catrgory)
            {
                case Category.DMG:
                    DMG_Init();
                    break;

                case Category.RCV:
                    RCV_Init();
                    break;

                case Category.REIN:
                    REIN_Init();
                    break;
            }
        }

        

        /// <summary>
        /// 기존 요소에 있는 객체들 지우기
        /// </summary>
        private void ClearElement()
        {
            int index = m_contentRoot.childCount;

            for (int i = 0; i < index; i++)
            {
                Destroy(m_contentRoot.GetChild(i).gameObject);
            }
        }

    }
}
