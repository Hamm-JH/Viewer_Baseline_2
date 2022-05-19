using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class RenderTest : MonoBehaviour
    {
        [SerializeField] Renderer renderer;
        [SerializeField] Camera cam;

        // Start is called before the first frame update
        void Start()
        {
            renderer = GetComponent<Renderer>();
            cam = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            //if (renderer.isVisible)
            //{
            //    Debug.Log($"is visible");
            //}
            //else
            //{
            //    Debug.Log($"is not visible");
            //}
            //renderer.allowOcclusionWhenDynamic = true;

            Debug.Log(CheckObjectsInCamera(gameObject));
            //renderer.

            // �� ��ü�� ī�޶� ǥ�ÿ��� �ȿ� �ֳ�?
            if (!CheckObjectsInCamera(gameObject)) return;

            if(IsCameraLookUp())
            {
                Debug.Log("true");
            }
            else
            {
                Debug.Log("false");
            }
        }

        public bool CheckObjectsInCamera(GameObject _target)
        {
            //Camera selectedCamera = Camera.main;
            Vector3 screenPoint = cam.WorldToViewportPoint(_target.transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

            return onScreen;
        }

        /// <summary>
        /// �� ��ü�� ī�޶� ���� �����ִ°�?
        /// </summary>
        /// <returns></returns>
        public bool IsCameraLookUp()
        {
            bool result = false;

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(cam.WorldToScreenPoint(transform.position));
            if(Physics.Raycast(ray, out hit))
            {
                
                if(hit.transform.gameObject == gameObject)
                {
                    result = true;
                }
            }

            return result;
        }

        // �ʱ�ȭ
            // 1. �� ī�޶� �Ҵ�
        // ������Ʈ
            // �ڱ� �ڽ� ��ġ���� ī�޶�� ray ���
            // ī�޶� �ȸ°� �ٸ��� ������ 

        //private void OnRenderObject()
        //{
        //    Debug.Log($"OnRenderObject");
        //}
        
        
        
    }
}
