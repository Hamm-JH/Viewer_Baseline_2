using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.WebAPI
{
    using System;
    using System.IO;
    using AdminViewer.API;
    using Definition;
    using Management;
    using UnityEngine.UI;

	public partial class Module_WebAPI : AModule
	{
        public Ad_Capture capture;

        #region 보고서 출력 버튼

        public void DownloadReport(Ad_Capture _capture)
        {
            capture = _capture;

            StartCoroutine(OnReadyToCapture());
        }

        private IEnumerator OnReadyToCapture()
        {
            capture.isCapEnd1 = false;
            capture.isCapEnd2 = false;
            capture.isCapEnd3 = false;

            capture.L1_Result = "";
            capture.L2_Result = "";
            capture.L3_Result = "";

            yield return new WaitForEndOfFrame();

            StartCoroutine(capture.L1_Capture.GetImage(this, capture.L1_CaptureTarget.TogglesBeforeCapture));
            StartCoroutine(capture.L2_Capture.GetImage(this, capture.L2_CaptureTarget.TogglesBeforeCapture));
            //StartCoroutine(capture.L3_Capture.GetImage(this));

            while (true)
            {
                if (capture.isCapEnd1 && 
                    capture.isCapEnd2 &&
                    capture.isCapEnd3)
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }

            // TODO 0616
#if UNITY_EDITOR
#else
            ExternalAPI.OnReadyToPrint(
                    _fName1: capture.L1_Result,
                    _fName2: capture.L2_Result,
                    _fName3: capture.L3_Result);
#endif
        }

        private IEnumerator ReadyToCapture()
        {
            capture.isCapReady1 = false;
            capture.isCapReady2 = false;

            Indicator.State5_BP1_Indicator _indic1 = capture.IndicatorState5_BP1;
            Indicator.State5_BP2_Indicator _indic2 = capture.IndicatorState5_BP2;

            capture.isCapReady1 = true;
            StartCoroutine(_indic2.ReadyCapture());

            while (true)
            {
                yield return new WaitForEndOfFrame();
                if (capture.isCapReady1 && capture.isCapReady2) break;
            }
            //yield return new WaitUntil(() => capture.isCapReady1 && capture.isCapReady2);

            CaptureImages();

            yield break;
        }

        private void CaptureImages()
        {
            List<RectTransform> panels = new List<RectTransform>();
            panels.Add(capture.IndicatorTypeState5_BP1);
            panels.Add(capture.IndicatorTypeState5_BP2);

            capture.isCapEnd1 = false;
            capture.isCapEnd2 = false;

            capture.imgName1 = "";
            capture.imgName2 = "";

            capture.img1Base64 = "";
            capture.img2Base64 = "";

            int index = panels.Count;
            for (int i = 0; i < index; i++)
            {
                StartCoroutine(CreateScreenshot(panels[i], i));
            }

            //capture.IndicatorTypeState5_BP1
            //capture.IndicatorTypeState5_BP2
        }

        private IEnumerator CreateScreenshot(RectTransform _target, int _index)
        {
            yield return new WaitForEndOfFrame();

            #region set value

            string fileLocation = SetFileLocation();
            string fileName = SetFilename(_index);
            if (_index == 0) capture.imgName1 = fileName;
            else if (_index == 1) capture.imgName2 = fileName;

            //string fileRoute =  $@"{fileLocation}\{fileName}";
            string fileRoute = SetFileRoute(_target, _index, fileLocation);

            //Debug.Log($"file Route : {fileRoute}");

            Camera targetCamera = MainManager.Instance.MainCamera;
            #endregion

            if (!Directory.Exists(fileLocation)) Directory.CreateDirectory(fileLocation);

            //Debug.Log(Application.persistentDataPath);
            //Debug.Log(Application.dataPath);
            //Debug.Log(fileLocation);
            //Debug.Log(SetRelativePath(fileLocation));

            // 캡쳐 전 불투명화
            _target.GetComponent<Image>().color = new Color(1, 1, 1, 1);

            yield return new WaitForEndOfFrame();

            RenderTexture currentTexture = RenderTexture.active;
            RenderTexture.active = targetCamera.targetTexture;

            byte[] imgByte;     // 스크린샷을 Byte로 저장. Texture2D use
            Texture2D tex = new Texture2D(int.Parse(_target.rect.width.ToString()), int.Parse(_target.rect.height.ToString()), TextureFormat.RGB24, false);

            Debug.Log($"target width : {_target.rect.width.ToString()}");
            Debug.Log($"target height : {_target.rect.height.ToString()}");
            Debug.Log($"target rect.x : {_target.rect.x}");
            Debug.Log($"target rect.y : {_target.rect.y}");
            Debug.Log($"target pos.x : {_target.position.x}");
            Debug.Log($"target pos.y ; {_target.position.y}");

            float xPos = _target.rect.x + _target.position.x;
            float yPos = _target.rect.y + _target.position.y;

            tex.ReadPixels(new Rect(xPos, yPos, _target.rect.width, _target.rect.height), 0, 0);

            tex.Apply();

            RenderTexture.active = currentTexture;  // ?

            // Encode texture into PNG
            imgByte = tex.EncodeToPNG();
            DestroyImmediate(tex);

            string base64Image = "";
            base64Image = Convert.ToBase64String(imgByte);  // 이미지 base64 변환

            //Debug.Log(base64Image);

            File.WriteAllBytes(fileRoute, imgByte);

            // 캡쳐 전 투명화 복구
            _target.GetComponent<Image>().color = new Color(1, 1, 1, 0.85f);

            if (_index == 0)
            {
                capture.isCapEnd1 = true;
                capture.img1Base64 = base64Image;
            }
            else if (_index == 1)
            {
                capture.isCapEnd2 = true;
                capture.img2Base64 = base64Image;
            }

            if (capture.isCapEnd1 && capture.isCapEnd2)
            {
                
                //#if UNITY_EDITOR
                //#else
                //                WebManager.OnReadyToPrint(
                //                    _fName1: capture.img1Base64,
                //                    _fName2: capture.img2Base64);
                //#endif
            }
        }

        private string SetAbsolutePath(string absolutePath)
        {
            return absolutePath;
        }

        private string SetRelativePath(string absolutePath)
        {
            string result = absolutePath;

            result = result.Replace(Application.dataPath, ".");

            Debug.Log(result);

            return result;
        }

        private string SetFileLocation()
        {
            string appPath = Application.dataPath;
            string inFolderName = @"/CaptureImages";
            //return @"D:\GLTFBuild\GraphImages";
            return appPath + inFolderName;
        }

        private string SetFilename(int _index)
        {
            string result = "";

            //string bridgeCode = Manager.MainManager.Instance.BridgeCode;

            //DateTime nowTime = DateTime.Now;
            //string dateCode = string.Format("{0}-{1:00}-{2:00}-{3:00}-{4:00}",
            //    nowTime.Year, nowTime.Month, nowTime.Day,
            //    nowTime.Hour, nowTime.Minute);

            //string fileCode = "";
            //if (_index == 0)
            //{
            //    fileCode = "F1";
            //}
            //else if (_index == 1)
            //{
            //    fileCode = "F2";
            //}

            //result = $"{bridgeCode},{dateCode},{fileCode}.png";

            return result;
        }



        private string SetFileRoute(RectTransform _target, int _index, string _fileLocation)
        {
            string fileRoute = "";

            //string bridgeCode = ContentManager.Instance. Manager.MainManager.Instance.BridgeCode;

            // 파일 위치 지정
            //string fileLocation = @"D:\GLTFBuild\GraphImages";

            string fileName = string.Format(@"\{0}", SetFilename(_index)); // @"\" + "1" + _index + ".png"; //SetFilename(_index);
            //string fileName = string.Format(@"\{0}.png", $"{bridgeCode}_GraphImage_{string.Format("{0:00}", _index)}", _target.gameObject.name + _index);

            fileRoute = _fileLocation + fileName;

            return fileRoute;
        }

        #endregion
    }
}
