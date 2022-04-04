//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;
////using MODBS_Library;
//using UnityEngine.UI;

//public class DataManager : MonoBehaviour
//{
//    #region Instance
//    private static DataManager instance;

//    public static DataManager Instance
//    {
//        get
//        {
//            if(instance == null)
//            {
//                instance = FindObjectOfType<DataManager>() as DataManager;
//            }
//            return instance;
//        }
//    }
//    #endregion

//    #region 변수 선언부

    

//    #region 3D 객체 변수

//    [Header("3D 객체 변수")]
//    [SerializeField] private GameObject rootObject;
//    [SerializeField] private GameObject root2DObject;
//    [SerializeField] private GameObject root3DObject;
//    [SerializeField] private GameObject[] bridgeTopParts;
//    [SerializeField] private GameObject[] bridgeBottomParts;
//    [SerializeField] private List<MeshRenderer> meshRenderers;   // renderer 관리용 리스트

//    #endregion

//    #region 2D 치수선 배열

//    [Header("2D 치수선, 외곽선 배열")]
//    private Transform[] mainLineTransforms;
//    private Transform[] subLineTransforms;
//    private Transform[] outlineTransforms;
//    private float dimensionOffset;

//    #endregion

//    #region Coroutine check value

//    private bool is2DDimTextEnd;
//    private bool is2DCodeEnd;       // 2D 초기화 코드 종료 확인
//    private bool is3DCodeEnd;       // 3D 초기화 코드 종료 확인

//    #endregion

//    #region collider

//    private List<Transform> colliders;          // collider를 가진 Transform 관리용 리스트

//    #endregion

//    #region issue test value

//    private GameObject tempIssueObject;        // (임시) 손상/보강 난수생성 리스트

//    #endregion

//    #region drag extension border

//    private float dragExtensionBorder;

//    #endregion

//    #endregion

//    #region 속성 선언부

//    #region 3D 객체 변수

//    public GameObject RootObject
//    {
//        get => rootObject;
//        set => rootObject = value;
//    }

//    public GameObject Root2DObject
//    {
//        get => root2DObject;
//        set => root2DObject = value;
//    }

//    public GameObject Root3DObject
//    {
//        get => root3DObject;
//        set => root3DObject = value;
//    }

//    public GameObject[] BridgeTopParts
//    {
//        get => bridgeTopParts;
//        set => bridgeTopParts = value;
//    }

//    public GameObject[] BridgeBottomParts
//    {
//        get => bridgeBottomParts;
//        set => bridgeBottomParts = value;
//    }

//    public List<MeshRenderer> MeshRenderers
//    {
//        get => meshRenderers;
//        set => meshRenderers = value;
//    }

//    #endregion

//    #region 2D 치수선 배열

//    public Transform[] MainLineTransforms
//    {
//        get => mainLineTransforms;
//        set => mainLineTransforms = value;
//    }

//    public Transform[] SubLineTransforms
//    {
//        get => subLineTransforms;
//        set => subLineTransforms = value;
//    }

//    public Transform[] OutlineTransforms
//    {
//        get => outlineTransforms;
//        set => outlineTransforms = value;
//    }

//    public float offset
//    {
//        get => dimensionOffset;
//        set => dimensionOffset = value;
//    }

//    #endregion

//    #region Coroutine check property (객체 초기화 단계 종료 확인코드 같이 실행)

//    public bool Is2DCodeEnd
//    {
//        get => is2DCodeEnd;
//        set
//        {
//            is2DCodeEnd = value;
//            InitializeCheck();
//        }
//    }

//    public bool Is3DCodeEnd
//    {
//        get => is3DCodeEnd;
//        set
//        {
//            is3DCodeEnd = value;
//            InitializeCheck();
//        }
//    }

//    #endregion

//    #region drag extension border

//    public float DragExtensionBorder
//    {
//        get => dragExtensionBorder;
//        set => dragExtensionBorder = value;
//    }

//    #endregion

//    #endregion

//    private void Start()
//    {
//        is2DDimTextEnd = false;
//        is2DCodeEnd = false;
//        is3DCodeEnd = false;
//        colliders = new List<Transform>();
//    }

//    //public void SetIssueObject()
//    //{
//    //    ResetIssue();
//    //    //StartCoroutine(DataLoader.LoadJSON(MainManager.Instance.BridgeCode));
//    //}

//    //private void ResetIssue()
//    //{
//    //    int index = InputManager.Instance.SelectionController.StoredIssueEntityList.Count;
//    //    List<Issues.Entity> _list = InputManager.Instance.SelectionController.StoredIssueEntityList;
//    //    for (int i = 0; i < index; i++)
//    //    {
//    //        Destroy(_list[i].gameObject);
//    //    }

//    //    _list.Clear();
//    //}

//    #region Initialize Object setting

//    /// <summary>
//    /// 할당받은 root 객체 아래의 객체를 분류 시작
//    /// </summary>
//    /// <param name="_rootObject"></param>
//    public void InitializeObject(GameObject _rootObject)
//    {
//        // TODO SceneStatus : 3. ObjectInitialize 변경

//        MeshRenderers = new List<MeshRenderer>();

//        GameObject rootObj = _rootObject.transform.GetChild(0).gameObject;
//        GameObject root3DObj = null;
//        GameObject root2DObj = null;

//        // 2D root 객체와 3D root 객체를 원하는 변수에 각각 할당한다.
//        int index = RootObject.transform.childCount;
//        for (int i = 0; i < index; i++)
//        {
//            //if(LevelCodeConverter.ConvertLv1Code(RootObject.transform.GetChild(i).name) != CodeLv1.Null)

//            // TODO 0222
//            if(RootObject.transform.GetChild(i).name != "")
//            {
//                root3DObj = RootObject.transform.GetChild(i).gameObject;
//            }
//            else
//            {
//                root2DObj = RootObject.transform.GetChild(i).gameObject;
//            }
//        }

//        // 2D, 3D 객체 초기화
//        StartCoroutine(Initialize3DObject(root3DObject));
//    }

//    #endregion

//    #region Initialize routine end check

//    public void InitializeCheck()
//    {
//        bool check = false;

//        check = Is2DCodeEnd & Is3DCodeEnd;

//        if(check == true)
//        {
//            // TODO SceneStatus : 4. Ready 변경
//            Debug.Log($"초기화 단계 완료. 상호작용 가능");
//        }
//        else
//        {
//            Debug.Log($"waiting another initialize routine");
//        }
//    }

//    #endregion

//    #region 3D Object Initialize

//    #region Initialize 3D Object setting

//    private IEnumerator Initialize3DObject(GameObject _root3DObject)
//    {
//        //manager.RootObject.transform.parent.position = new Vector3(0, 0, 0);

//        // 지역 리스트 초기화
//        List<GameObject> bridgeTopParts = new List<GameObject>();
//        List<GameObject> bridgeBottomParts = new List<GameObject>();
        
//        // 지역 리스트에 요소 할당
//        int index = _root3DObject.transform.childCount;
//        for (int i = 0; i < index; i++)
//        {
//            // todo 0222
//            //if(LevelCodeConverter.ConvertLv3Code(_root3DObject.transform.GetChild(i).name.Substring(0, 2)) == CodeLv3.SP)
//            //{
//            //    bridgeTopParts.Add(_root3DObject.transform.GetChild(i).gameObject);
//            //}
//            //else if(LevelCodeConverter.ConvertLv3Code(_root3DObject.transform.GetChild(i).name.Substring(0, 2)) == CodeLv3.AP)
//            //{
//            //    bridgeBottomParts.Add(_root3DObject.transform.GetChild(i).gameObject);
//            //}
//        }

//        // 리스트 전역 배열 변환
//        BridgeTopParts = (from obj in bridgeTopParts orderby obj.name select obj).ToArray<GameObject>();
//        BridgeBottomParts = (from obj in bridgeBottomParts orderby obj.name select obj).ToArray<GameObject>();

//        // 다음 메서드 실행
//        SetObjectsMaterialNCollider(root3DObject);

//        yield break;
//    }

//    #endregion

//    #region Set 3D Objects Material, Collider

//    private void SetObjectsMaterialNCollider(GameObject _root3DObject)
//    {
//        #region 변수 선언부
//        // 3D 객체 아래의 모든 자식객체 수집
//        Transform[] objTransforms = _root3DObject.gameObject.GetComponentsInChildren<Transform>();

//        // 자식객체 중에 조건에 맞는 (material을 가진 객체의 조건) 객체 수집
//        Transform[] selected_objTransforms = (from obj in objTransforms where obj.name.Split(',').Length > 1 select obj).ToArray<Transform>();

//        Dictionary<string, int> multiSplitIndex = new Dictionary<string, int>();
//        #endregion

//        #region Set Object data to all child objects

//        // 모든 자식객체에 ObjectData 할당 (자식객체의 데이터와 연결된 변수값을 정리하는 코드)
//        int arrayIndex = objTransforms.Length;
//        for (int i = 0; i < arrayIndex; i++)
//        {
//            //objTransforms[i].gameObject.AddComponent<ObjectData>();
//        }

//        #endregion

//        #region 모든 객체의 material, collider를 할당한다.

//        // Material을 할당해야 하는 객체에 Material, Collider 할당
//        // [반복] material을 가진 모든 자식객체
//        int index1 = selected_objTransforms.Length;
//        for (int i = 0; i < index1; i++)
//        {
//            // 현재 index 객체의 문자열 인자값 할당
//            string[] splitObjectString = selected_objTransforms[i].name.Split(',');

//            string name = splitObjectString[0];
//            bool isTarget = false;
//            //if(name.Contains("PI"))
//            //{
//            //    if(name.Contains("CC"))
//            //    {
//            //        isTarget = true;
//            //    }
//            //}

//            // [조건] 분할 문자열이 2개 : (객체명 문자열 1, material 정보 1) 객체가 한 개의 Material을 필요로 하는 경우
//            if(splitObjectString.Length == 2)
//            {
//                selected_objTransforms[i].GetComponent<MeshRenderer>().material = 
//                    Resources.Load<UnityEngine.Material>($"Materials/{splitObjectString[1].Substring(0, 1)}/{splitObjectString[1]}");

//                SetMaterialOffset(name, selected_objTransforms[i].GetComponent<MeshRenderer>());

//                //if(isTarget)
//                //{
//                //    selected_objTransforms[i].GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(
//                //        17,
//                //        selected_objTransforms[i].GetComponent<MeshRenderer>().material.mainTextureScale.y
//                //        );
//                //}

//                selected_objTransforms[i].gameObject.AddComponent<MeshCollider>().convex = true;
//                colliders.Add(selected_objTransforms[i]);
//            }
//            // [조건] 분할 문자열이 2개 이상: (객체명 문자열 1, material 정보 n) 객체가 여러 개의 Material을 필요로 하는 경우
//            else
//            {
//                // Dictionary에 확인
//                int stringIndex = 0;                

//                // [조건] Dictionary에 객체명 키값 검색시 키가 있는 경우 : 첫 번째 객체가 아닌 경우
//                if (multiSplitIndex.TryGetValue(splitObjectString[0], out stringIndex).Equals(true))
//                {
//                    multiSplitIndex[splitObjectString[0]]++;

//                    selected_objTransforms[i].GetComponent<MeshRenderer>().material = Resources.Load<UnityEngine.Material>($"Materials/{splitObjectString[multiSplitIndex[splitObjectString[0]]].Substring(0, 1)}/{splitObjectString[multiSplitIndex[splitObjectString[0]]]}");

//                    SetMaterialOffset(name, selected_objTransforms[i].GetComponent<MeshRenderer>());

//                    //if (isTarget)
//                    //{
//                    //    selected_objTransforms[i].GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(
//                    //        17,
//                    //        selected_objTransforms[i].GetComponent<MeshRenderer>().material.mainTextureScale.y
//                    //        );
//                    //}
//                }
//                // 해당 키값이 없으면 첫 번째 검색된 객체임
//                else
//                {
//                    // Dictionary 삭제 (의도치 않은 오류가 발생해서 Dictionary 키를 한번씩 날려버리는 코드 작성)
//                    // 교각의 받침 02 03 라인이 2번째 교각에 있을때
//                    // 교각의 받침 03 04 라인이 3번째 교각에 있다. 이때 03 라인의 이름이 중복되는 문제 발생
//                    multiSplitIndex.Clear();

//                    // Dictionary에 새 키 할당
//                    multiSplitIndex.Add(splitObjectString[0], stringIndex);
//                    multiSplitIndex[splitObjectString[0]]++;

//                    selected_objTransforms[i].GetComponent<MeshRenderer>().material = Resources.Load<UnityEngine.Material>($"Materials/{splitObjectString[multiSplitIndex[splitObjectString[0]]].Substring(0, 1)}/" +
//                        $"{splitObjectString[multiSplitIndex[splitObjectString[0]]]}");

//                    SetMaterialOffset(name, selected_objTransforms[i].GetComponent<MeshRenderer>());

//                    //if (isTarget)
//                    //{
//                    //    selected_objTransforms[i].GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(
//                    //        17,
//                    //        selected_objTransforms[i].GetComponent<MeshRenderer>().material.mainTextureScale.y
//                    //        );
//                    //}

//                    selected_objTransforms[i].gameObject.AddComponent<MeshCollider>().convex = true;
//                }
//            }

//            // 객체에 Meshrenderer가 존재할 경우 Meshrenderer 관리 리스트에 Meshrenderer 할당
//            MeshRenderers.Add(selected_objTransforms[i].GetComponent<MeshRenderer>());
//        }

//        multiSplitIndex.Clear();

//        #endregion

//        #region 다음 단계를 실행한다.

//        //Return3DRootObject(_root3DObject);
//        SetBoundsNCenterVector(_root3DObject);
//        //InitCamPosition(_root3DObject);
//        //SetMouseDragBounds(_root3DObject);


//        Is3DCodeEnd = true;     // 3D 객체 생성단계 종료됨
//        //test(_root3DObject);

//        #endregion
//    }

//    private void SetMaterialOffset(string name, MeshRenderer target)
//    {
//        if (name.Contains("PI"))
//        {
//            if (name.Contains("RBP"))
//            {
//                if (name.Contains("CC"))
//                {
//                    target.material.mainTextureScale = new Vector2(
//                    2.1f,
//                    3.9f
//                    );
//                }
//            }
//            else if (name.Contains("CC"))
//            {
//                target.material.mainTextureScale = new Vector2(
//                    17,
//                    target.material.mainTextureScale.y
//                    );
//            }
//        }

//        if (name.Contains("FT"))
//        {
//            if (name.Contains("Up"))
//            {
//                target.material.mainTextureScale = new Vector2(
//                    11,
//                    target.material.mainTextureScale.y
//                    );
//            }
//        }

//        if (name.Contains("SL"))
//        {
//            if (name.Contains("RCS"))
//            {
//                if (name.Contains("CL"))
//                {
//                    target.material.mainTextureScale = new Vector2(
//                        7.3f,
//                        8
//                        );
//                }
//                else if (name.Contains("CR"))
//                {
//                    target.material.mainTextureScale = new Vector2(
//                        -1.88f,
//                        3.66f
//                        );
//                }
//                else if (name.Contains("CC"))
//                {
//                    target.material.mainTextureScale = new Vector2(
//                        4.82f,
//                        3.82f
//                        );
//                }
//            }
//            else if (name.Contains("LC"))
//            {
//                target.material.mainTextureScale = new Vector2(
//                    8,
//                    target.material.mainTextureScale.y
//                    );
//            }
//            else if (name.Contains("RC"))
//            {
//                target.material.mainTextureScale = new Vector2(
//                    8,
//                    target.material.mainTextureScale.y
//                    );
//            }
//            else if (name.Contains("CL"))
//            {
//                target.material.mainTextureScale = new Vector2(
//                    7.3f,
//                    target.material.mainTextureScale.y
//                    );

//            }
//            else if (name.Contains("MC"))
//            {
//                target.material.mainTextureScale = new Vector2(
//                    4.57f,
//                    target.material.mainTextureScale.y
//                    );
//            }
//            else if (name.Contains("ML"))
//            {
//                target.material.mainTextureScale = new Vector2(
//                    5.13f,
//                    target.material.mainTextureScale.y
//                    );
//            }
//            else if (name.Contains("MR"))
//            {
//                target.material.mainTextureScale = new Vector2(
//                    1f,
//                    target.material.mainTextureScale.y
//                    );
//            }
//        }
//    }

//    #endregion

//    #region Set Bounds, Vector

//    /// <summary>
//    /// 모든 Collider를 가지고 있는 객체의 Bounds, Vector 할당
//    /// </summary>
//    /// <param name="_objects"></param>
//    private void SetBoundsNCenterVector(GameObject _objects)
//    {
//        #region 변수 선언부

//        List<Vector3> minVector = new List<Vector3>();
//        List<Vector3> maxVector = new List<Vector3>();

//        Vector3 minVectorResult = new Vector3(0, 0, 0);
//        Vector3 maxVectorResult = new Vector3(0, 0, 0);
//        #endregion

//    }

//    #endregion

//    #endregion

//}
