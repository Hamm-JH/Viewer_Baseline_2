using Definition._Issue;
using Management;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IssueType
{
    Null,
    damage,
    recover
}

namespace Issues
{
    public class _Entity : MonoBehaviour
    {
        [SerializeField]
        public Issue issueData;

        public Material defaultMaterial;
        public Material material;

        public void Reset()
        {
            issueData.IssueOrderCode = "";
            issueData.CdBridge = "";
            issueData.CdBridgeParts = "";
            issueData.DcMemberSurface = "";
            issueData.DcLocation = 0;

            issueData._IssueCode = "";
            issueData.IssueCode = Code.Null;
            issueData.IssueStatus = "";

            issueData.YnRecover = "";

            transform.position = new Vector3(0, 0, 0);
            SwitchRender(false);
            SetMaterial();
        }

        public void SwitchRender(bool isOn)
        {
            this.GetComponent<MeshRenderer>().enabled = isOn;
            this.GetComponent<Collider>().enabled = isOn;
        }

        public void SetIssueCode(string issueCode, string issueIndex)
        {
            issueData.IssueCode = (Code)Enum.Parse(typeof(Code), issueCode);
        }

        /// <summary>
        /// 손상/보강을 등록할때 등록된 IssueStatus를 확인하고 등록
        /// </summary>
        public void SetMaterial(IssueType issueType = IssueType.Null)
        {
            if (issueData.IssueCode == Code.Null)
            {
                transform.GetComponent<MeshRenderer>().material = defaultMaterial;
                return;
            }

            if (issueType == IssueType.damage)
            {
                ContentManager.Instance.SetIssueMaterial(transform.GetComponent<MeshRenderer>(), issueType, issueData.IssueCode);
                //transform.GetComponent<MeshRenderer>().material = MainManager.Instance.DamagedIssueMaterialList[(int)issueData.IssueCode];
            }
            else if (issueType == IssueType.recover)
            {
                ContentManager.Instance.SetIssueMaterial(transform.GetComponent<MeshRenderer>(), issueType, issueData.IssueCode);
                //transform.GetComponent<MeshRenderer>().material = MainManager.Instance.RecoverIssueMaterialList[(int)issueData.IssueCode];
            }
            else
            {
                Debug.LogError("Issues.Entity def mat deprecated");
                //transform.GetComponent<MeshRenderer>().material = defaultMaterial;
            }
        }

        public void SetMaterial(Material mat)
        {
            transform.GetComponent<MeshRenderer>().material = mat;
        }
    }
}

