using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Capture : MonoBehaviour
{
    [System.Serializable]
    public class Env
    {
        public Camera camera;
        public bool isFirst;
        public bool isSecond;
        public bool isThird;
        public RectTransform target;
    }

    public Env env;

    // Start is called before the first frame update
    //void Start()
    //{
    //    //StartCoroutine(GetImage(Manager.UIManager.Instance));
    //    //StartCoroutine(GetImage((Texture2D)l1Image.texture) );
    //    //StartCoroutine(GetImage());
    //}

    private void Update()
    {
        
    }

    //public IEnumerator GetImage(Manager.UIManager callback, List<GameObject> toggles = null)
    //{
    //    #region Set directory
    //    yield return new WaitForEndOfFrame();

    //    string fileLocation = @"D:/Images";
    //    string plus = "";
    //    if(env.isFirst) { plus = "1"; }
    //    else if(env.isSecond) { plus = "2"; }
    //    else if(env.isThird) { plus = "3"; }

    //    string fileRoute = $"{fileLocation}/img{plus}.png";

    //    if (!Directory.Exists(fileLocation)) Directory.CreateDirectory(fileLocation);
    //    #endregion

    //    #region Set Before Capture

    //    if(toggles != null) ToggleObjs(toggles, false);

    //    #endregion

    //    if (toggles != null) yield return new WaitForEndOfFrame();
    //    yield return new WaitForEndOfFrame();

    //    RenderTexture currentTexture = RenderTexture.active;
    //    RenderTexture.active = env.camera.targetTexture;

    //    Debug.Log($"pixelWidth : {env.camera.pixelWidth}");
    //    Debug.Log($"pixelHeight : {env.camera.pixelHeight}");

    //    Rect rect = new Rect(0, 0, env.camera.targetTexture.width, env.camera.targetTexture.height);

    //    byte[] imgByte;
    //    Texture2D _tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);

    //    float xPos = env.camera.rect.x + env.camera.pixelWidth / 2;
    //    float yPos = env.camera.rect.y + env.camera.pixelHeight / 2;

    //    Debug.Log($"cam rect x : {env.camera.rect.x}");
    //    Debug.Log($"cam rect y : {env.camera.rect.y}");
    //    Debug.Log($"pixelWidth / 2 : {env.camera.pixelWidth / 2}");
    //    Debug.Log($"pixelHeight / 2 : {env.camera.pixelHeight / 2}");
    //    Debug.Log(xPos);
    //    Debug.Log(yPos);

    //    //env.camera.activeTexture


    //    //l1Camera.targetTexture.width
    //    //l1Camera.targetTexture.height
    //    //_tex.ReadPixels(new Rect(xPos, yPos, env.camera.pixelWidth/2, env.camera.pixelHeight/2), 0, 0);

    //    //// 1번
    //    //if(env.isFirst)
    //    //{
    //    //    //_tex.ReadPixels(new Rect(0, 320, 500, 500), 100, 100, true);

    //    //    // Rect ( 시작위치 x, 시작위치 y, 폭 x, 폭 y ), 그릴 시작위치 x, y
    //    //    //_tex.ReadPixels(new Rect(0, 200, 500, 900), 0, 0, true);
    //    //    //_tex.ReadPixels(new Rect(0, 320, 1920, 1156), 0, 0, true);
    //    //    _tex.ReadPixels(rect, 0, 0, false);
    //    //}
    //    //// 2번
    //    //else
    //    //{
    //    //    //_tex.ReadPixels(new Rect(0, 400, 100, 100), 100, 100, true);
    //    //    //_tex.ReadPixels(new Rect(0, 400, 1920, 680), 0, 0, true);
    //    //    _tex.ReadPixels(rect, 0, 0, false);
    //    //}
    //    _tex.ReadPixels(rect, 0, 0, false);

    //    _tex.Apply();

    //    RenderTexture.active = currentTexture;

    //    imgByte = _tex.EncodeToPNG();
    //    DestroyImmediate(_tex);

    //    string base64Image = Convert.ToBase64String(imgByte);

    //    File.WriteAllBytes(fileRoute, imgByte);

    //    if(env.isFirst)
    //    {
    //        callback.env.L1_Result = base64Image;
    //        callback.isCapEnd1 = true;
    //    }
    //    else if(env.isSecond)
    //    {
    //        callback.env.L2_Result = base64Image;
    //        callback.isCapEnd2 = true;
    //    }
    //    else if(env.isThird)
    //    {
    //        callback.env.L3_Result = base64Image;
    //        callback.isCapEnd3 = true;
    //    }

    //    // 모든 캡쳐 종료 후 다시 타겟 오브젝트 켜기
    //    if (toggles != null) ToggleObjs(toggles, true);
    //}

    private void ToggleObjs(List<GameObject> objs, bool isOn)
    {
        int index = objs.Count;
        for (int i = 0; i < index; i++)
        {
            objs[i].SetActive(isOn);
        }
    }
}
