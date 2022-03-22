using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
//using iTextSharp;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
using System.Text;

namespace Manager
{
    public class ScreenCaptureManager : MonoBehaviour
    {
        #region Singleton

        private static ScreenCaptureManager instance;

        public static ScreenCaptureManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<ScreenCaptureManager>();
                    if (instance == null)
                    {
                        return null;
                    }

                    return instance;
                }

                return instance;
            }
        }

        #endregion

        #region 변수

        public Image[] SceneStatusCaptures = new Image[5];
        public Camera _3DgameCamera;

        public GameObject printButton;

        private bool isCorutinePlaying;
        private string fileLocation;
        private string fileName;
        private string finalLOC;

        #endregion

        // 렌더링이 끝나고 스크린샷 촬영을 위해 함수 호출
        // UIManager.cs에서 ScreenCapture()를 호출 하는 부분을 주석처리한 상태
        // 
        public void ScreenCapture()
        {
            Manager.ViewSceneStatus sceneStatus = MainManager.Instance.SceneStatus;

            switch (sceneStatus)
            {
                case ViewSceneStatus.Ready:
                case ViewSceneStatus.ViewAllDamage:
                case ViewSceneStatus.ViewPartDamage:
                case ViewSceneStatus.ViewPart2R:
                case ViewSceneStatus.ViewMaintainance:
                    if (!isCorutinePlaying)
                        StartCoroutine("TakeScreenCapture");
                    break;
                default:
                    break;
            }

            if (sceneStatus == ViewSceneStatus.ViewMaintainance)
                printButton.SetActive(true);
            else
                printButton.SetActive(false);
        }

        // 스크린샷 저장
        public IEnumerator TakeScreenCapture()
        {
            isCorutinePlaying = true;

            // 파일 경로 지정
            yield return new WaitForEndOfFrame();
            //string fileLocation = Application.dataPath;
            fileLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            fileName = string.Format("Image_{0}_{1}.png", System.DateTime.Now.ToString("yyyyMMdd"), TransSceneStatusName());
            finalLOC = fileLocation + "\\" + fileName;

            // 폴더 유무 확인
            yield return new WaitForSecondsRealtime(0.3f);
            if (!Directory.Exists(fileLocation))
            {
                Directory.CreateDirectory(fileLocation);
            }

            // 스크린샷
            yield return new WaitForEndOfFrame();
            byte[] imageBytes;
            Texture2D screenImage = new Texture2D(_3DgameCamera.pixelWidth, _3DgameCamera.pixelHeight, TextureFormat.RGB24, true);
            screenImage.ReadPixels(new Rect(0, 0, _3DgameCamera.pixelWidth, _3DgameCamera.pixelHeight), 0, 0, true);
            screenImage.Apply();

            // png로 변환
            yield return new WaitForEndOfFrame();
            imageBytes = screenImage.EncodeToPNG();

            // 이미지 저장
            yield return new WaitForEndOfFrame();
            System.IO.File.WriteAllBytes(finalLOC, imageBytes);
            Debug.Log("스크린샷 저장 성공");
            //System.Diagnostics.Process.Start("explorer.exe", "D:\\wesmart_svn\\trunk");
            isCorutinePlaying = false;
        }

        // 스크린샷 이미지의 이름
        private string TransSceneStatusName()
        {
            string sceneName = "";
            switch (MainManager.Instance.SceneStatus)
            {
                case ViewSceneStatus.Ready:
                    sceneName = "[1]Ready";
                    break;
                case ViewSceneStatus.ViewAllDamage:
                    sceneName = "[2]ViewAllDamage";
                    break;
                case ViewSceneStatus.ViewPartDamage:
                    sceneName = "[3]ViewPartDamage";
                    break;
                case ViewSceneStatus.ViewPart2R:
                    sceneName = "[4]ViewPart2R";
                    break;
                case ViewSceneStatus.ViewMaintainance:
                    sceneName = "[5]ViewMaintainance";
                    break;
            }
            return sceneName;
        }

        private string TransFileName(int i)
        {
            string sceneName = "";
            switch (i)
            {
                case 0:
                    sceneName = "[1]Ready";
                    break;
                case 1:
                    sceneName = "[2]ViewAllDamage";
                    break;
                case 2:
                    sceneName = "[3]ViewPartDamage";
                    break;
                case 3:
                    sceneName = "[4]ViewPart2R";
                    break;
                case 4:
                    sceneName = "[5]ViewMaintainance";
                    break;
            }
            return sceneName;
        }

        //public void PrintButton()
        //{
        //    string path = string.Format("{0}\\", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        //    //Document doc = new Document(iTextSharp.text.PageSize.LETTER);   // 객체 생성
        //    Document doc = new Document(iTextSharp.text.PageSize.A4.Rotate());
        //    PdfWriter pdfWriter = PdfWriter.GetInstance(doc, new FileStream(path + "pdftest.pdf", FileMode.Create, FileAccess.Write));  // PdfWriter가 doc 내용을 test.pdf 파일에 쓰도록 설정
        //    StringBuilder sb = new StringBuilder("");

        //    doc.Open();
        //    string pngPath = string.Format("{0}\\{1}", path, string.Format("Image_{0}_{1}.png", System.DateTime.Now.ToString("yyyyMMdd"), TransFileName(0)));
        //    FileInfo fi = new FileInfo(pngPath);
        //    if (fi.Exists)
        //    {
        //        iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(pngPath);
        //        pdfImage.ScaleToFit(800, 450);
        //        pdfImage.Alignment = iTextSharp.text.Image.UNDERLYING;
        //        doc.Add(pdfImage);
        //    }
        //    sb.AppendLine();

        //    pngPath = string.Format("{0}\\{1}", path, string.Format("Image_{0}_{1}.png", System.DateTime.Now.ToString("yyyyMMdd"), TransFileName(1)));
        //    fi = new FileInfo(pngPath);
        //    if (fi.Exists)
        //    {
        //        iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(pngPath);
        //        pdfImage.ScaleToFit(800, 450);
        //        pdfImage.Alignment = iTextSharp.text.Image.UNDERLYING;
        //        doc.Add(pdfImage);
        //    }

        //    pngPath = string.Format("{0}\\{1}", path, string.Format("Image_{0}_{1}.png", System.DateTime.Now.ToString("yyyyMMdd"), TransFileName(2)));
        //    fi = new FileInfo(pngPath);
        //    if (fi.Exists)
        //    {
        //        iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(pngPath);
        //        pdfImage.ScaleToFit(800, 450);
        //        pdfImage.Alignment = iTextSharp.text.Image.UNDERLYING;
        //        doc.Add(pdfImage);
        //    }

        //    pngPath = string.Format("{0}\\{1}", path, string.Format("Image_{0}_{1}.png", System.DateTime.Now.ToString("yyyyMMdd"), TransFileName(3)));
        //    fi = new FileInfo(pngPath);
        //    if (fi.Exists)
        //    {
        //        iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(pngPath);
        //        pdfImage.ScaleToFit(800, 450);
        //        pdfImage.Alignment = iTextSharp.text.Image.UNDERLYING;
        //        doc.Add(pdfImage);
        //    }

        //    pngPath = string.Format("{0}\\{1}", path, string.Format("Image_{0}_{1}.png", System.DateTime.Now.ToString("yyyyMMdd"), TransFileName(4)));
        //    fi = new FileInfo(pngPath);
        //    if (fi.Exists)
        //    {
        //        iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(pngPath);
        //        pdfImage.ScaleToFit(800, 450);
        //        pdfImage.Alignment = iTextSharp.text.Image.UNDERLYING;
        //        doc.Add(pdfImage);
        //    }
        //    doc.Close();

        //}
    }
}