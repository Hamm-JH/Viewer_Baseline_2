using System;
using System.Collections.Generic;
using System.Linq;
using Manager;
using RuntimeData;
using UnityEngine;
using UnityEngine.Serialization;

namespace _02.Scripts._OutlineTest
{
    public struct CamParam
	{
        // 객체명
        // 목표 부모객체
        // 거리두는 벡터값
        // 객체 뷰 각도 벡터값
        // 목표 텍스처
        // 레이어마스크 정수배열 또는 레이어마스크

        public string camName;
        public Transform parent;
        public Vector3 localPos;
        public Vector3 localEuler;
        public RenderTexture targetTex;
        public string maskName;
        public float orthoSize;
        //public Camera targetCam;
	}

    public class DimBox : MonoBehaviour
    {
        public enum Axis
		{
            X, Y, Z
		}

        [FormerlySerializedAs("BoxSize")] public float boxSize = 5f;
        bool found = false;

        public void CreateWall(Transform dimTrns, Transform cached3D)
        {
            // DimBox의 위치를 3D 객체의 위치와 동기화한다.
            //Debug.Log($"*** {transform.name}"); // 임시 생성 객체
            //Debug.Log($"*** {transform.parent.name}"); // DimBoxs
            //transform.parent.position = cached3D.position;

            //Debug.Log($"_*_ rot name : {cached3D.parent.parent.name}"); // Segment n
            //Debug.Log($"_*_ rot : {cached3D.parent.parent.rotation.eulerAngles}"); // Segment n
            // TODO 211214 객체의 회전된 각도에 적용된 dimBox 생성코드 적용
            Vector3 _bAngle = cached3D.parent.parent.rotation.eulerAngles;

            //Debugs.DebugTest.Instance.angleDebug = _bAngle;

            GameObject box = GameObject.CreatePrimitive(PrimitiveType.Plane);
            MeshRenderer[] meshes = box.GetComponents<MeshRenderer>();
            meshes[0].material = Resources.Load<Material>("Materials/Bridge/Wall");


            Bounds bound = new Bounds();
            GetBoundWithChildren(cached3D, ref bound, ref found);
            List<float> dist = new List<float>();

            dist.Add(bound.max.x - bound.min.x);
            dist.Add(bound.max.y - bound.min.y);
            dist.Add(bound.max.z - bound.min.z);

            float distmax = Mathf.Round(dist.Max());

            float distscale = 0f;
            if (distmax / boxSize > 0.5f)
                distscale = distmax / boxSize;
            else
                distscale = 0.4f;

            //Debug.Log($"***** {cached3D.name}");
            //Debug.Log($"***** {bound}");

            float boxP = Mathf.Round(distscale * boxSize);

            GameObject bottomWall = Instantiate(box, gameObject.transform, true);
            bottomWall.GetComponent<Collider>().enabled = false;
            bottomWall.name = "Wall_bottom";
            bottomWall.transform.localPosition = -new Vector3(0, boxP, 0);
            bottomWall.transform.localScale = Vector3.one * distscale;
            bottomWall.transform.rotation = Quaternion.Euler(_bAngle);

			{
				// 객체명
				// 목표 부모객체
				// 거리두는 벡터값
				// 객체 뷰 각도 벡터값
				// 목표 텍스처
				// 레이어마스크 정수배열 또는 레이어마스크 

				CamParam par = new CamParam();
				par.camName = "cambo";
				par.parent = bottomWall.transform;
				par.localPos = new Vector3(0, -1, 0);
				par.localEuler = new Vector3(-90, 0, 90);
				par.targetTex = RootContainer.Instance.captureResource.texbo;
				par.maskName = "BO";

                // 1.bound
                // 2.maskName
                par.orthoSize = SetOrthoSize(bound, par.maskName);

                //par.targetCam = RootContainer.Instance.captureResource.cambo;

				RootContainer.Instance.captureResource.cambo = SetDrawingCapture(par);
			} // BO

			GameObject topWall = Instantiate(box, gameObject.transform, true);
            topWall.GetComponent<Collider>().enabled = false;
            topWall.name = "Wall_Top";
            topWall.transform.localPosition = new Vector3(0, boxP, 0);
            topWall.transform.localScale = Vector3.one * distscale;

            Vector3 _aT = _bAngle + new Vector3(0, 0, 180); // new Vector3(0, 0, 180);
            topWall.transform.localRotation = Quaternion.Euler(_aT); //Quaternion.Euler(0, 0, 180);

            {
				CamParam par = new CamParam();
				par.camName = "camto";
				par.parent = topWall.transform;
				par.localPos = new Vector3(0, -1, 0);
				par.localEuler = new Vector3(-90, 0, 90);
				par.targetTex = RootContainer.Instance.captureResource.texto;
				par.maskName = "TO";
                par.orthoSize = SetOrthoSize(bound, par.maskName);
                //par.targetCam = RootContainer.Instance.captureResource.camto;

                RootContainer.Instance.captureResource.camto = SetDrawingCapture(par);
			}

			GameObject frontWall = Instantiate(box, gameObject.transform, true);
            frontWall.GetComponent<Collider>().enabled = false;
            frontWall.name = "Wall_Front";

            Vector3 _aFr = _bAngle + new Vector3(90, 0, 0);
            frontWall.transform.localRotation = Quaternion.Euler(_aFr); //Quaternion.Euler(-90, 0, 0);

            frontWall.transform.Translate(Vector3.down * boxP);
            //frontWall.transform.localPosition = new Vector3(0, 0, boxP);
            frontWall.transform.localScale = Vector3.one * distscale;

			{
				CamParam par = new CamParam();
				par.camName = "camfr";
				par.parent = frontWall.transform;
				par.localPos = new Vector3(0, -1, 0);
				par.localEuler = new Vector3(-90, 0, 0);
				par.targetTex = RootContainer.Instance.captureResource.texba;   // RootContainer.Instance.captureResource.texfr; // 할당 텍스처 fr, ba 변경
                par.maskName = "BA";                                            // FR BA 변경
                par.orthoSize = SetOrthoSize(bound, par.maskName);
                //par.targetCam = RootContainer.Instance.captureResource.camfr;

                RootContainer.Instance.captureResource.camfr = SetDrawingCapture(par);
			}

			GameObject backWall = Instantiate(box, gameObject.transform, true);
            backWall.GetComponent<Collider>().enabled = false;
            backWall.name = "Wall_back";

            Vector3 _aBa = _bAngle + new Vector3(-90, 0, 0);
            backWall.transform.localRotation = Quaternion.Euler(_aBa);

            backWall.transform.Translate(Vector3.down * boxP);
            //backWall.transform.localPosition = -new Vector3(0, 0, boxP);
            backWall.transform.localScale = Vector3.one * distscale;

			{
				CamParam par = new CamParam();
				par.camName = "camba";
				par.parent = backWall.transform;
				par.localPos = new Vector3(0, -1, 0);
				par.localEuler = new Vector3(-90, 0, 180);
				par.targetTex = RootContainer.Instance.captureResource.texba;   // RootContainer.Instance.captureResource.texba; // 할당 텍스처 fr, ba 변경
                par.maskName = "FR";                                            // FR BA 변경
                par.orthoSize = SetOrthoSize(bound, par.maskName);
                //par.targetCam = RootContainer.Instance.captureResource.camba;

                RootContainer.Instance.captureResource.camba = SetDrawingCapture(par);
			}

			GameObject leftWall = Instantiate(box, gameObject.transform, true);
            leftWall.GetComponent<Collider>().enabled = false;
            leftWall.name = "Wall_left";

            Vector3 _aLe = _bAngle + new Vector3(0, 0, 90);
            leftWall.transform.localRotation = Quaternion.Euler(_aLe);

            leftWall.transform.Translate(Vector3.down * boxP);
            //leftWall.transform.localPosition = -new Vector3(boxP, 0, 0);
            leftWall.transform.localScale = Vector3.one * distscale;

			{
				CamParam par = new CamParam();
				par.camName = "camle";
				par.parent = leftWall.transform;
				par.localPos = new Vector3(0, -1, 0);
				par.localEuler = new Vector3(-90, 0, -90);
				par.targetTex = RootContainer.Instance.captureResource.texle;
				par.maskName = "LE";
                par.orthoSize = SetOrthoSize(bound, par.maskName);
                //par.targetCam = RootContainer.Instance.captureResource.camle;

                RootContainer.Instance.captureResource.camle = SetDrawingCapture(par);
			}

			GameObject rightWall = Instantiate(box, gameObject.transform, true);
            rightWall.GetComponent<Collider>().enabled = false;
            rightWall.name = "Wall_right";

            Vector3 _aRe = _bAngle + new Vector3(0, 0, -90);
            rightWall.transform.localRotation = Quaternion.Euler(_aRe);

            rightWall.transform.Translate(Vector3.down * boxP);
            //rightWall.transform.localPosition = new Vector3(boxP, 0, 0);
            rightWall.transform.localScale = Vector3.one * distscale;

            {
                CamParam par = new CamParam();
                par.camName = "camri";
                par.parent = rightWall.transform;
                par.localPos = new Vector3(0, -1, 0);
                par.localEuler = new Vector3(-90, 0, 90);
                par.targetTex = RootContainer.Instance.captureResource.texri;
                par.maskName = "RE";
                par.orthoSize = SetOrthoSize(bound, par.maskName);

                Debug.Log($"right orthosize : {par.orthoSize}");
                //par.targetCam = RootContainer.Instance.captureResource.camri;

                RootContainer.Instance.captureResource.camri = SetDrawingCapture(par);
            }

            Destroy(box);
            gameObject.transform.localPosition = bound.center;

            return;

            foreach (Transform surface in dimTrns)
            {
                switch (surface.name.Split('_')[0])
                {
                    case "TO":
                        foreach (Transform local in surface)
                        {
                            var localPosition = local.position;
                            localPosition.y = topWall.transform.position.y;
                            local.position = localPosition;
                        }

                        break;

                    case "BO":
                        foreach (Transform local in surface)
                        {
                            var localPosition = local.position;
                            localPosition.y = bottomWall.transform.position.y;
                            local.position = localPosition;
                        }

                        break;

                    case "LE":
                        foreach (Transform local in surface)
                        {
                            var localPosition = local.position;
                            localPosition.x = leftWall.transform.position.x;
                            local.position = localPosition;
                        }

                        break;

                    case "RE":
                        foreach (Transform local in surface)
                        {
                            var localPosition = local.position;
                            localPosition.x = rightWall.transform.position.x;
                            local.position = localPosition;
                        }

                        break;

                    case "FR":
                        foreach (Transform local in surface)
                        {
                            var localPosition = local.position;
                            localPosition.z = frontWall.transform.position.z;
                            local.position = localPosition;
                        }

                        break;

                    case "BA":
                        foreach (Transform local in surface)
                        {
                            var localPosition = local.position;
                            localPosition.z = backWall.transform.position.z;
                            local.position = localPosition;
                        }

                        break;
                }
            }
        }

        private Camera SetDrawingCapture(CamParam par)
		{
            // 객체명
            // 목표 부모객체
            // 거리두는 벡터값
            // 객체 뷰 각도 벡터값
            // 목표 텍스처
            // 레이어마스크 정수배열 또는 레이어마스크

            GameObject cam = new GameObject(par.camName);
            cam.transform.SetParent(par.parent);
            cam.transform.localPosition = par.localPos;
            cam.transform.localRotation = Quaternion.Euler(par.localEuler);

			//cam.AddComponent<Camera>();
			cam.AddComponent<Camera>().targetTexture = par.targetTex;
			Camera _cam = cam.GetComponent<Camera>();
            _cam.orthographic = true;
            _cam.orthographicSize = par.orthoSize;
            _cam.cullingMask = 1 << 0 | 1 << LayerMask.NameToLayer(par.maskName);

            _cam.clearFlags = CameraClearFlags.SolidColor;
            _cam.backgroundColor = Color.black;

            return _cam;

        }

        private bool GetBoundWithChildren(Transform parent, ref Bounds pBound, ref bool initBound)
        {
            Bounds bound = new Bounds();
            bool didOne = false;

            if (parent.gameObject.GetComponent<Renderer>() != null)
            {
                bound = parent.gameObject.GetComponent<Renderer>().bounds;
                if (initBound)
                {
                    pBound.Encapsulate(bound.min);
                    pBound.Encapsulate(bound.max);
                }
                else
                {
                    pBound.min = new Vector3(bound.min.x, bound.min.y, bound.min.z);
                    pBound.max = new Vector3(bound.max.x, bound.max.y, bound.max.z);
                    initBound = true;
                }

                didOne = true;
            }

            // union with bound(s) of any/all children
            foreach (Transform child in parent)
            {
                if (GetBoundWithChildren(child, ref pBound, ref initBound))
                {
                    didOne = true;
                }
            }

            return didOne;
        }

        private void ShowPartsDim(Transform m_transform)
        {
            return;

            RootContainer.Instance.Root2DObject.SetActive(true);
            Transform lv01 = RootContainer.Instance.Root2DObject.transform.GetChild(0);
            lv01.gameObject.SetActive(true);
            foreach (Transform parts in lv01)
            {
                if (parts.name != m_transform.name.Substring(0, 4))
                    parts.gameObject.SetActive(false);
                else
                {
                    foreach (Transform lv02 in parts)
                    {
                        if (lv02.name != m_transform.name)
                        {
                            lv02.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        private float SetOrthoSize(Bounds _bound, string _mask)
		{
            float result = 0f;
            Axis maxAxis = Axis.X;

            maxAxis = _bound.extents.x > _bound.extents.y ? Axis.X : Axis.Y;
            float temp = maxAxis == Axis.X ? _bound.extents.x : _bound.extents.y;
            maxAxis = temp > _bound.extents.z ? maxAxis : Axis.Z;

            // 최대값 세팅해두기
            //float max = _bound.extents.x;
            //max = max > _bound.extents.y ? max : _bound.extents.y;
            //max = max > _bound.extents.z ? max : _bound.extents.z;

            switch(_mask)
			{
                case "BO":
					{
                        // x z > x y
                        float size = _bound.extents.x >= _bound.extents.z ? _bound.extents.x + 1.2f : _bound.extents.z + 1.2f;
                        result = size;
					}
                    break;

                case "TO":
					{
                        // x z > x y
                        float size = _bound.extents.x >= _bound.extents.z ? _bound.extents.x + 1.2f : _bound.extents.z + 1.2f;
                        result = size;
					}
                    break;

                case "FR":
					{
                        // x y > x z
                        float size = _bound.extents.x >= _bound.extents.y ? _bound.extents.x : _bound.extents.y + 1.2f;
                        result = size;
					}
                    break;

                case "BA":
					{
                        // x y > x z
                        float size = _bound.extents.x >= _bound.extents.y ? _bound.extents.x : _bound.extents.y + 1.2f;
                        result = size;
					}
                    break;

                case "LE":
					{
                        // y z
                        float size = _bound.extents.y >= _bound.extents.z ? _bound.extents.y + 1.2f : _bound.extents.z+1.2f;
                        result = size;
					}
                    break;

                case "RE":
					{
                        // y z
                        float size = _bound.extents.y >= _bound.extents.z ? _bound.extents.y + 1.2f : _bound.extents.z+1.2f;
                        result = size;
					}
                    break;
			}

            //result = result >= max ? result + 1 : result;

            return result;
		}
    }
}