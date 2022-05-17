using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartInspect
{
    /// <summary>
    /// SmartInspect의 UI 내부 리스트 요소 클래스
    /// </summary>
    public partial class ListElement
    {
        private void IMG_Init(Definition._Issue.Issue _issue)
        {
            string fgroup = _issue.Fgroup;
            List<Definition._Issue.Issue.ImgIndex> imgs = _issue.Imgs;

            foreach(var _imgIndex in imgs)
            {
                string fid = _imgIndex.fid;
                string ftype = _imgIndex.ftype;

                GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("UI/SmartInspect/Inspect_Records"), m_contentRoot);
                RecordElement element = obj.GetComponent<RecordElement>();

                Packet_Record packet = new Packet_Record(3, fgroup, fid, ftype, _issue, m_rootUI);

                element.Init(packet);

                _countData.m_elements.Add(element);
            }
        }
    }
}
