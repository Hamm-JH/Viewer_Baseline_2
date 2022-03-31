using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Issue;

namespace Manager
{
    public class IssueManager : MonoBehaviour
    {
        #region Instance
        private static IssueManager instance;

        public static IssueManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = FindObjectOfType<IssueManager>() as IssueManager;
                }
                return instance;
            }
        }
        #endregion

        List<Issue.AIssue> issueList;

        /// <summary>
        /// 모든 손상/보수 객체의 On/Off
        /// </summary>
        /// <param name="isOn"></param>
        public void DisplayAll(bool isOn)
        {
            if(issueList == null)
            {
                //issueList = RuntimeData.RootContainer.Instance.IssueObjectList;
            }

            for (int i = 0; i < issueList.Count; i++)
            {
                issueList[i].GetComponent<MeshRenderer>().enabled = isOn;
                issueList[i].GetComponent<Collider>().enabled = !isOn;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isOn"></param>
        public void DisplayAllDamagedOnlyView(bool isOn)
        {
            //if (issueList == null)
            //{
            //    issueList = RuntimeData.RootContainer.Instance.IssueObjectList;
            //}

            //for (int i = 0; i < issueList.Count; i++)
            //{
            //    if (issueList[i].GetComponent<DamagedIssue>())
            //    {
            //        issueList[i].GetComponent<MeshRenderer>().enabled = isOn;
            //        issueList[i].GetComponent<Collider>().enabled = !isOn; // 현재 큐브 클릭이벤트 제외한 상태이므로 bool 반대값 할당
            //    }
            //}
        }

        /// <summary>
        /// 특정 객체에 관련된 손상/보수 객체 On
        /// 나머지 객체 Off
        /// </summary>
        /// <param name="partName"></param>
        private void DisplayPart(string partName, string issueType)
        {
            if(issueList == null)
            {
                //issueList = RuntimeData.RootContainer.Instance.IssueObjectList;
            }

            if (issueType == "Damaged")
            {
                for (int i = 0; i < issueList.Count; i++)
                {
                    if (issueList[i].BridgePartName == partName)
                    {
                        //if (issueList[i].GetComponent<DamagedIssue>())
                        //{
                        //    issueList[i].GetComponent<MeshRenderer>().enabled = true;
                        //    issueList[i].GetComponent<Collider>().enabled = true;
                        //}
                    }
                    else
                    {
                        issueList[i].GetComponent<MeshRenderer>().enabled = false;
                        issueList[i].GetComponent<Collider>().enabled = false;
                    }
                }
            }
            else if (issueType == "Recovered")
            {
                for (int i = 0; i < issueList.Count; i++)
                {
                    //if (issueList[i].BridgePartName == partName)
                    //{
                    //    if (issueList[i].GetComponent<RecoveredIssue>())
                    //    {
                    //        issueList[i].GetComponent<MeshRenderer>().enabled = true;
                    //        issueList[i].GetComponent<Collider>().enabled = true;
                    //    }
                    //}
                    //else
                    //{
                    //    issueList[i].GetComponent<MeshRenderer>().enabled = false;
                    //    issueList[i].GetComponent<Collider>().enabled = false;
                    //}
                }
            }
        }

        private void DisplaySpecifiedIssue(string issueName)
        {
            //if(issueList == null)
            //{
            //    issueList = RuntimeData.RootContainer.Instance.IssueObjectList;
            //}

            //Debug.Log($"***********************");
            //Debug.Log($"display issue name : {issueName}");
            //Debug.Log($"***********************");

            int index = issueList.Count;
            for (int i = 0; i < index; i++)
            {
                if(issueList[i].IssueOrderCode == issueName)
                {
                    issueList[i].GetComponent<MeshRenderer>().enabled = true;
                    issueList[i].GetComponent<Collider>().enabled = true;
                }
                else
                {
                    issueList[i].GetComponent<MeshRenderer>().enabled = false;
                    issueList[i].GetComponent<Collider>().enabled = false;
                }
            }
        }

        public void DisplayIssue()
        {
            //ViewSceneStatus sceneStatus = MainManager.Instance.SceneStatus;
            //List<AIssue> issueList = RuntimeData.RootContainer.Instance.IssueObjectList;

            //switch(sceneStatus)
            //{
            //    case ViewSceneStatus.Ready:
            //    case ViewSceneStatus.ViewAllDamage:
            //        DisplayAll(false);
            //        DisplayAllDamagedOnlyView(true);
            //        break;

            //    case ViewSceneStatus.ViewPartDamage:
            //        {
            //            string partName = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name;
            //            DisplayAll(false);
            //            DisplayPart(partName, "Damaged");
            //        }
            //        break;

            //    case ViewSceneStatus.ViewPart2R:
            //        {
            //            string partName = RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform.name;
            //            DisplayAll(false);
            //            DisplayPart(partName, "Recovered");
            //        }
            //        break;

            //    case ViewSceneStatus.ViewMaintainance:
            //        {
            //            DisplayAll(true);
            //        }
            //        break;
            //}
        }

        private void DisplayData<T>(T _data) where T : Issue.AIssue
        {
            //Debug.Log($"------------------------");
            //Debug.Log($"DisplayData");
            //Debug.Log($"Type : {typeof(T)}");
            //Debug.Log($"Bridge name : {_data.BridgeName}");
            //Debug.Log($"Bridge part name : {_data.BridgePartName}");
            //Debug.Log($"Issue description : {_data.Description}");
            //Debug.Log($"Surface code : {_data.Issue6Surfaces.ToString()}");
            //Debug.Log($"location code : {_data.Issue9Location.ToString()}");
            //Debug.Log($"issue codename : {_data.IssueCodes.ToString()}");

            //Debug.Log($"손상 등급 : {_data.IssueGrade}");
            //Debug.Log($"손상 폭 : {_data.IssueWidth}");
            //Debug.Log($"손상 높이 : {_data.IssueHeight}");
            //Debug.Log($"손상 깊이 : {_data.IssueDepth}");

            //Debug.Log($"Issue order code : {_data.IssueOrderCode}");
            //Debug.Log($"------------------------");

            string _issueOrderCode = _data.IssueOrderCode;

            DisplaySpecifiedIssue(_issueOrderCode);
        }
    }
}
