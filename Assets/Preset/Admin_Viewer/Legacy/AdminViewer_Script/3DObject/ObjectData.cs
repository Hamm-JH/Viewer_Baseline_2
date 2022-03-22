using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Bridge
{
    public class ObjectData : MonoBehaviour
    {
        #region 변수
        [SerializeField] private Bounds bound;
        [SerializeField] private Vector3 minVector;
        [SerializeField] private Vector3 maxVector;
        [SerializeField] private Vector3 centerVector;

        [SerializeField] private Vector3 centerBound;
        [SerializeField] private bool isSelected = false;       // 객체가 선택되었는지 상태를 저장하는 변수
        [SerializeField] private string parentName = "";        // 부모객체의 이름 확인
        [SerializeField] private Material defaultMaterial;      // 객체가 기본적으로 가지는 Material
        #endregion

        #region 속성
        public Bounds Bound
        {
            get => bound;
            set
            {
                bound = value;
                minVector = bound.min;
                maxVector = bound.max;
                centerVector = (minVector + maxVector) / 2f;
            }
        }

        public Vector3 CenterBound
        {
            get => centerBound;
            set => centerBound = value;
        }

        public Vector3 MinVector3
        {
            get => minVector;
        }

        public Vector3 MaxVector3
        {
            get => maxVector;
        }

        public bool IsSelected
        {
            get => isSelected;
            set => isSelected = value;
        }

        public string ParentName
        {
            get => parentName;
            set => parentName = value;
        }

        public Material DefaultMaterial
        {
            get => defaultMaterial;
            set => defaultMaterial = value;
        }

        #endregion

        #region Set Material

        public void SetMaterial(bool _isSelected)
        {
            MeshRenderer[] meshes = gameObject.GetComponentsInChildren<MeshRenderer>();
            int index = meshes.Length;

            // 객체가 선택상태일 경우
            if (_isSelected)
            {
                for (int i = 0; i < index; i++)
                {
                    meshes[i].material = Resources.Load<Material>("Materials/Damage/selection");
                }
            }
            // 객체가 선택상태가 아닐 경우
            else
            {
                for (int i = 0; i < index; i++)
                {
                    meshes[i].material = meshes[i].GetComponent<ObjectData>().DefaultMaterial;
                }
            }
        }

        #endregion

        #region Events

        //private void OnMouseEnter()
        //{
        //    //Debug.Log("3D Object enter");
        //}

        //private void OnMouseExit()
        //{
        //    //Debug.Log("3D Object exit");
        //}

        //private void OnCollisionEnter(Collision collision)
        //{
        //    //collision.rigidbody.
        //}

        //private void OnMouseDown()
        //{

        //    //Manager.EventClassifier.Instance.OnEvent<ObjectData>(Control.Status.Click, gameObject.GetComponent<ObjectData>());

        //    //Control.Order<ObjectData> order = new Control.Order<ObjectData>(Control.Status.Down, gameObject.GetComponent<ObjectData>());
        //    //Manager.EventClassifier.Instance.OnEvent<ObjectData>(order);

        //    //Debug.Log("3D Object down");
        //}

        private void OnMouseUp()
        {
            //Debug.Log("3D Object up");
            Manager.EventClassifier.Instance.OnEvent<ObjectData>(Control.Status.Click, gameObject.GetComponent<ObjectData>());
        }



        //public void OnPointerClick(PointerEve ntData eventData)
        //{
        //    Debug.Log("3D Object");
        //}
        #endregion
    }
}
