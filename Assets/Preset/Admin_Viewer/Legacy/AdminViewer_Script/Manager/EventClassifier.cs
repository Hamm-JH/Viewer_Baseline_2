using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.EventSystems;
//using MODBS_Library;
using System;
using System.Linq;
using UnityEngine.UI;

namespace Control
{
    public enum Status
    {
        Wait = -1,
        Enter = 0,
        Exit = 1,
        Down = 2,
        Click = 3,
        Up = 4
    }

    public class EventOrder<T>
    {
        public Order<T> inOrder;

        public EventOrder(Order<T> _order)
        {
            inOrder = new Order<T>(_order.status, _order.data);
        }

        public System.Type OrderDataType
        {
            get => typeof(T);
        }
    }

    public class Order<T>
    {
        public Status status;
        public T data;
        public PointerEventData eventData;

        public System.Type DataType
        {
            get
            {
                return typeof(T);
            }
        }

        public Order() { }

        public Order(Status _status, T _data)
        {
            status = _status;
            data = _data;
        }

        public Order(Status _status, T _data, PointerEventData _eventData)
        {
            status = _status;
            data = _data;
            eventData = _eventData;
        }

        public void Set(Status _status, T _data)
        {
            status = _status;
            data = _data;
        }

        public void Set(Status _status, T _data, PointerEventData _eventData)
        {
            status = _status;
            data = _data;
            eventData = _eventData;
        }
    }
}

namespace Manager
{
    public class EventCache
    {
        public System.Type type;
        public object targetObj;
        public Transform targetTransform;
        public Manager.ViewSceneStatus status;

        // scene change menu 선택시 변경되는 메뉴
        public Manager.SceneChangeMenu.SceneMenu menu;

        public EventCache(System.Type _type, object _targetObj, Transform _targetTransform, Manager.ViewSceneStatus _status)
        {
            type = _type;
            targetObj = _targetObj;
            targetTransform = _targetTransform;
            status = _status;
        }

        public EventCache(System.Type _type, object _targetObj, Transform _targetTransform, Manager.ViewSceneStatus _status, Manager.SceneChangeMenu.SceneMenu _menu)
        {
            type = _type;
            targetObj = _targetObj;
            targetTransform = _targetTransform;
            status = _status;
            menu = _menu;
        }
    }

    public class EventClassifier : MonoBehaviour
    {
        #region Instance
        private static EventClassifier instance;

        public static EventClassifier Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<EventClassifier>() as EventClassifier;
                }
                return instance;
            }
        }
        #endregion

        [SerializeField] EventSystem es;
        [SerializeField] MouseRayOver rayOver;

        public SceneChangeMenu _sceneMenu;

        public Queue<object> clickQueue;

        public Stack<EventCache> backwardStack; // 선택된 상태   (뒤로가기용)
        public EventCache currentCache;         // 현재 상태
        public Stack<EventCache> forwardStack;  // 뒤돌아간 상태  (앞으로가기용)

        [SerializeField]
        public Dictionary<System.Type, System.Type> eventTypeDics;

        private void Awake()
        {
            backwardStack = new Stack<EventCache>();
            
            forwardStack = new Stack<EventCache>();

            clickQueue = new Queue<object>();

            eventTypeDics = new Dictionary<System.Type, System.Type>();
            eventTypeDics.Add(typeof(Skew), typeof(Skew));
            eventTypeDics.Add(typeof(Curve), typeof(Curve));
            //eventTypeDics.Add(typeof(Interactable), typeof(Interactable));
            eventTypeDics.Add(typeof(CurveSkew), typeof(CurveSkew));

            eventTypeDics.Add(typeof(Bridge.ObjectData), typeof(Bridge.ObjectData));
            eventTypeDics.Add(typeof(Indicator.Element.KeymapElement), typeof(Indicator.Element.KeymapElement));
            
            eventTypeDics.Add(typeof(Issue.DamagedIssue), typeof(Issue.DamagedIssue));
            eventTypeDics.Add(typeof(Issue.RecoveredIssue), typeof(Issue.RecoveredIssue));

            //eventTypeDics.Add(typeof(Indicator.Element.SingleIssueRecordInfo), typeof(Indicator.Element.SingleIssueRecordInfo));
            eventTypeDics.Add(typeof(Manager.SceneChangeMenu), typeof(Manager.SceneChangeMenu));
        }

        //private void Start()
        //{
        //    //currentCache = new EventCache(typeof(Manager.SceneChangeMenu), Manager.SceneChangeMenu.Instance, Manager.SceneChangeMenu.Instance.transform, ViewSceneStatus.Ready);
        //    //backwardStack.Push(cache);
        //}

        private void Update()
        {
            if(clickQueue != null)
            {
                int index = clickQueue.Count;
                if (index != 0)
                {
                    List<object> _events = new List<object>();
                    Debug.Log($"Queue Count : {clickQueue.Count}");
                    
                    for (int i = 0; i < index; i++)
                    {
                        _events.Add(clickQueue.Dequeue());
                    }

                    ParseEvents(_events.ToArray());
                }

                clickQueue.Clear();
            }
            // 클릭 상태가 아닐 때,
            else
            {
            }

            List<RaycastResult> rays = rayOver.HoveringCheck();
            if(rays != null)
            {
                int index = rays.Count;

                bool isCorrectFlag = false;
                for (int i = 0; i < index; i++)
                {
                    //Debug.Log(rays[i].gameObject.name);
                    HoverFlag flag;
                    if(rays[i].gameObject.TryGetComponent<HoverFlag>(out flag))
                    {
                        //Debug.Log(flag.name);
                        //Debug.Log(flag.FlagName);

                        UIManager.Instance.ToggleHoverPos(true, Input.mousePosition);
                        UIManager.Instance.SetHoverText(flag.FlagName);
                        isCorrectFlag = true;
                        break;
                    }
                }

                if(!isCorrectFlag)
                {
                    UIManager.Instance.ToggleHoverPos(false);
                }
            }
            else
            {
                UIManager.Instance.ToggleHoverPos(false);
            }

            //if(es.currentSelectedGameObject != null)
            //{
            //    Debug.Log(EventSystem.current.currentSelectedGameObject);
            //    //Debug.Log(es.currentSelectedGameObject.name);
            //}

        }

        #region selection data transfer

        /// <summary>
        /// 다른 코드로 넘어가기 전에 현재 선택 정보를 캐시정보에 넘긴다.
        /// </summary>
        public void TransferSelectionData(object _data, Transform _selectedTransform)
        {
            RuntimeData.RootContainer.Instance.cachedInstance = new RuntimeData.SelectedObject();
            RuntimeData.RootContainer.Instance.cachedInstance.Set(
                _data, _selectedTransform
                );
        }

        public void ClearSelectionData()
        {
            RuntimeData.RootContainer.Instance.selectedInstance = new RuntimeData.SelectedObject();
            RuntimeData.RootContainer.Instance.cachedInstance = new RuntimeData.SelectedObject();
            RuntimeData.RootContainer.Instance.cached3DInstance = new RuntimeData.SelectedObject();
        }

        #endregion

        #region Home button

        public void ReturnHome()
        {
            ClearSelectionData();
            MainManager.Instance.SceneStatus = ViewSceneStatus.ViewAllDamage;
        }

        #endregion

        #region Dimension view Button

        public void SwitchDimensionView()
        {
            bool _dimView = RuntimeData.RootContainer.Instance.dimensionView;

            if(_dimView)
            {
                // Sprite Off Icon 변경
                RuntimeData.RootContainer.Instance.dimensionView = false;
                ObjectManager.Instance.SetDimensionView(false);

                {
                    ForwardRendererData forwardData = MainManager.Instance.RenderSetting;
                    int index = forwardData.rendererFeatures.Count;
                    for (int i = 0; i < index; i++)
                    {
                        forwardData.rendererFeatures[i].SetActive(true);
                    }
                }

                //UIManager.Instance._btnDimView.SetButtonImage(true);
            }
            else
            {
                // Sprite On Icon 변경
                RuntimeData.RootContainer.Instance.dimensionView = true;
                ObjectManager.Instance.SetDimensionView(true);

                {
                    ForwardRendererData forwardData = MainManager.Instance.RenderSetting;
                    int index = forwardData.rendererFeatures.Count;
                    for (int i = 0; i < index; i++)
                    {
                        if (forwardData.rendererFeatures[i].name.Contains("Outline"))
                        {
                            forwardData.rendererFeatures[i].SetActive(false);
                        }
                        else
                        {
                            forwardData.rendererFeatures[i].SetActive(true);
                        }
                    }
                }

                //UIManager.Instance._btnDimView.SetButtonImage(false);
            }
        }

        public void SetDimensionView(bool isOn)
        {
            // TODO : 치수선 키고 끄기
            ObjectManager.Instance.SetActiveDimCube(isOn);
        }

        #endregion

        #region ViewPart2R Button

        public void ViewPart2R()
        {
            Debug.Log("ViewPart2R");

            ViewSceneStatus sceneStatus = MainManager.Instance.SceneStatus;

            switch(sceneStatus)
            {
                case ViewSceneStatus.ViewPartDamage:
                    MainManager.Instance.SceneStatus = ViewSceneStatus.ViewPart2R;
                    break;

                case ViewSceneStatus.ViewPart2R:
                case ViewSceneStatus.ViewMaintainance:
                    MainManager.Instance.SceneStatus = ViewSceneStatus.ViewPartDamage;
                    break;
            }

            
        }

        #endregion

        #region Selection event

        public void OnEvent<T>(Control.Status _status, object _data, PointerEventData _eventData = null) where T : class
        {
            Transform selectedObj = null;

            //Debug.Log(EventSystem.current);
            EventSystem es = gameObject.GetComponent<EventSystem>();
            GameObject go = es.currentSelectedGameObject;

            clickQueue.Enqueue(_data);
        }

        private void ParseEvents(object[] obj)
        {
            object[] _obj = obj;

            int index = obj.Length;

            // 하나의 이벤트만 발생한 경우 바로 이벤트 실행
            if (index == 1)
            {
                if (eventTypeDics[_obj[0].GetType()].Equals(typeof(Bridge.ObjectData)))
                {
                    Debug.Log(_obj[0].GetType());

                    //System.Type type = typeof(Bridge.ObjectData);
                    //Debug.Log(_obj.GetType().Assembly.FullName);

                    Bridge.ObjectData _data = obj[0] as Bridge.ObjectData;
                    InvokeEvent<Bridge.ObjectData>(_data);
                }

                else if (eventTypeDics[_obj[0].GetType()].Equals(typeof(Manager.SceneChangeMenu)))
                {
                    Manager.SceneChangeMenu _data = obj[0] as Manager.SceneChangeMenu;
                    InvokeEvent<Manager.SceneChangeMenu>(_data);
                }

                else if (eventTypeDics[_obj[0].GetType()].Equals(typeof(Indicator.Element.KeymapElement)))
                {
                    Indicator.Element.KeymapElement _data = obj[0] as Indicator.Element.KeymapElement;
                    InvokeEvent<Indicator.Element.KeymapElement>(_data);
                }

                else if (eventTypeDics[_obj[0].GetType()].Equals(typeof(Issue.DamagedIssue)))
                {
                    Issue.DamagedIssue _data = obj[0] as Issue.DamagedIssue;
                    InvokeEvent<Issue.DamagedIssue>(_data);
                }
                else if (eventTypeDics[_obj[0].GetType()].Equals(typeof(Issue.RecoveredIssue)))
                {
                    Issue.RecoveredIssue _data = obj[0] as Issue.RecoveredIssue;
                    InvokeEvent<Issue.RecoveredIssue>(_data);
                }

                //else if (eventTypeDics[_obj[0].GetType()].Equals(typeof(Indicator.Element.SingleIssueRecordInfo)))
                //{
                //    Indicator.Element.SingleIssueRecordInfo _data = obj[0] as Indicator.Element.SingleIssueRecordInfo;
                //    InvokeEvent<Indicator.Element.SingleIssueRecordInfo>(_data);
                //}

                else if(eventTypeDics[_obj[0].GetType()].Equals(typeof(Skew)))
                {
                    Skew _data = obj[0] as Skew;
                    InvokeEvent<Skew>(_data);
                }

                else if (eventTypeDics[_obj[0].GetType()].Equals(typeof(Curve)))
                {
                    Curve _data = obj[0] as Curve;
                    InvokeEvent<Curve>(_data);
                }

                //else if (eventTypeDics[_obj[0].GetType()].Equals(typeof(Interactable)))
                //{
                //    Interactable _data = obj[0] as Interactable;
                //    InvokeEvent<Interactable>(_data);
                //}

                else if (eventTypeDics[_obj[0].GetType()].Equals(typeof(CurveSkew)))
                {
                    CurveSkew _data = obj[0] as CurveSkew;
                    InvokeEvent<CurveSkew>(_data);
                }
            }
            // 두개 이상의 이벤트 발생한 경우 이벤트 실행
            else
            {
                for (int i = 0; i < index; i++)
                {
                    if (eventTypeDics[_obj[i].GetType()].Equals(typeof(Bridge.ObjectData)))
                    {
                        continue;
                    }
                    
                    else if (eventTypeDics[_obj[i].GetType()].Equals(typeof(Manager.SceneChangeMenu)))
                    {
                        Manager.SceneChangeMenu _data = obj[i] as Manager.SceneChangeMenu;
                        InvokeEvent<Manager.SceneChangeMenu>(_data);
                    }

                    else if (eventTypeDics[_obj[i].GetType()].Equals(typeof(Indicator.Element.KeymapElement)))
                    {
                        Indicator.Element.KeymapElement _data = obj[i] as Indicator.Element.KeymapElement;
                        InvokeEvent<Indicator.Element.KeymapElement>(_data);
                        return;
                    }

                    else if (eventTypeDics[_obj[i].GetType()].Equals(typeof(Issue.DamagedIssue)))
                    {
                        Issue.DamagedIssue _data = obj[i] as Issue.DamagedIssue;
                        InvokeEvent<Issue.DamagedIssue>(_data);
                        return;
                    }
                    else if (eventTypeDics[_obj[i].GetType()].Equals(typeof(Issue.RecoveredIssue)))
                    {
                        Issue.RecoveredIssue _data = obj[i] as Issue.RecoveredIssue;
                        InvokeEvent<Issue.RecoveredIssue>(_data);
                        return;
                    }

                    //else if (eventTypeDics[_obj[i].GetType()].Equals(typeof(Indicator.Element.SingleIssueRecordInfo)))
                    //{
                    //    Indicator.Element.SingleIssueRecordInfo _data = obj[i] as Indicator.Element.SingleIssueRecordInfo;
                    //    InvokeEvent<Indicator.Element.SingleIssueRecordInfo>(_data);
                    //    return;
                    //}

                    else if (eventTypeDics[_obj[0].GetType()].Equals(typeof(Skew)))
                    {
                        Skew _data = obj[0] as Skew;
                        InvokeEvent<Skew>(_data);
                        return;
                    }

                    else if (eventTypeDics[_obj[0].GetType()].Equals(typeof(Curve)))
                    {
                        Curve _data = obj[0] as Curve;
                        InvokeEvent<Curve>(_data);
                        return;
                    }

                    //else if (eventTypeDics[_obj[0].GetType()].Equals(typeof(Interactable)))
                    //{
                    //    Interactable _data = obj[0] as Interactable;
                    //    InvokeEvent<Interactable>(_data);
                    //    return;
                    //}

                    else if (eventTypeDics[_obj[0].GetType()].Equals(typeof(CurveSkew)))
                    {
                        CurveSkew _data = obj[0] as CurveSkew;
                        InvokeEvent<CurveSkew>(_data);
                        return;
                    }
                }
            }
        }

        private void InvokeEvent<T>(object _data) where T : class
        {
            Transform selectedObj = null;

            //Debug.Log(typeof(T));

            if (eventTypeDics.ContainsKey(_data.GetType()))
            {
                selectedObj = GetObjectTransform<T>((_data as T));
            }
            if (selectedObj == null)
            {
                Debug.Log("정의되지 않은 변수 접근");
                return;
            }

            if (_data.GetType().Equals(typeof(Manager.SceneChangeMenu)))
            {
                EventCache cache = new EventCache(_data.GetType(), _data, selectedObj, Manager.MainManager.Instance.SceneStatus, (_data as Manager.SceneChangeMenu).selectedButton);

                GetNewEventCache(cache);    // 새 이벤트캐시 할당
                ChangeSceneStatus<T>(_data, selectedObj);
            }

            RuntimeData.SelectedObject SlcInstance = RuntimeData.RootContainer.Instance.selectedInstance;

            // 처음 선택일 경우
            if (SlcInstance == null)
            {
                // TODO 현재 선택 객체 할당
                RuntimeData.RootContainer.Instance.selectedInstance = new RuntimeData.SelectedObject(_data, selectedObj);

                ChangeElementStatus(null, null, _data, selectedObj);

                Debug.Log("SlcInstance null");
                return;
            }
            else if (SlcInstance.item == null)
            {
                // TODO 현재 선택 객체 할당
                RuntimeData.RootContainer.Instance.selectedInstance = new RuntimeData.SelectedObject(_data, selectedObj);

                ChangeElementStatus(null, null, _data, selectedObj);

                Debug.Log("SlcInstance.item null");
                return;
            }

            // 이전 선택 객체유형이 같은가?
            if (SlcInstance.item.GetType() == _data.GetType())
            {
                // 이전 선택 객체와 같은가?
                if (SlcInstance.itemTransform.name == selectedObj.name)
                {
                    Debug.Log("[Type] : same | [Obj] : same");

                    EventCache cache = new EventCache(_data.GetType(), _data, selectedObj, Manager.MainManager.Instance.SceneStatus);

                    // 현재 SceneStatus에 따라 다음 단계 실행
                    TransferSelectionData(_data, selectedObj);
                    GetNewEventCache(cache);    // 새 이벤트캐시 할당
                    ChangeSceneStatus<T>(_data, selectedObj);
                }
                // 이전 선택 객체와 다른가?
                else
                {
                    Debug.Log("[Type] : same | [Obj] : diff");
                    // TODO 이전 객체가 3D 객체이므로 이전 객체의 Material 상태를 기본값으로 설정
                    // TODO 현재 선택 객체 Material 상태를 선택 상태로 설정
                    ChangeElementStatus(SlcInstance.item, SlcInstance.itemTransform, _data, selectedObj);
                    RuntimeData.RootContainer.Instance.selectedInstance.Set(_data, selectedObj);
                }
            }
            // 이전 선택 객체유형이 다른가?
            else
            {
                Debug.Log("[Type] : diff | [Obj] : diff");

                // TODO 이전 객체에 기본 상태 변경
                // TODO 현재 선택 객체 Material 상태를 선택 상태로 설정
                ChangeElementStatus(SlcInstance.item, SlcInstance.itemTransform, _data, selectedObj);
                RuntimeData.RootContainer.Instance.selectedInstance.Set(_data, selectedObj);
            }
        }


        /// <summary>
        /// 선택한 객체의 Transform을 받는다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_value"></param>
        /// <returns></returns>
        private Transform GetObjectTransform<T>(object _value) where T : class
        {
            var _data = _value as T;

            Transform targetTransform = null;

            if(eventTypeDics.ContainsKey(_data.GetType()))
            {
                if (_data.GetType().Equals(typeof(Bridge.ObjectData)))
                {
                    targetTransform = (_data as Bridge.ObjectData).transform;

                    if(MainManager.Instance.AppUseCase == Definition.UseCase.Bridge)
					{
                        string objParentName = targetTransform.parent.name;
                        string[] args = objParentName.Split('_');

                        // 단일부재
                        if (args.Length < 2)
                        {
                            return null;
                            //if (objParentName.Contains(CodeLv3.AP.ToString()) || objParentName.Contains(CodeLv3.SP.ToString()))
                            //{
                            //    Debug.Log(objParentName);

                            //    return targetTransform;
                            //}
                            //else
                            //{
                            //    return null;
                            //}
                        }
                        else
                        {
                            return targetTransform.parent;
                        }
					}
                    else if(MainManager.Instance.AppUseCase == Definition.UseCase.Tunnel)
					{
                        return targetTransform;
					}
                    else
					{
                        return null;
					}
                }
                else if (_data.GetType().Equals(typeof(Manager.SceneChangeMenu)))
                {
                    targetTransform = (_data as Manager.SceneChangeMenu).transform;
                }
                else if (_data.GetType().Equals(typeof(Indicator.Element.KeymapElement)))
                {
                    targetTransform = (_data as Indicator.Element.KeymapElement).clickedTransform;

                    if(MainManager.Instance.AppUseCase == Definition.UseCase.Bridge)
					{
                        string objParentName = targetTransform.parent.name;
                        string[] args = objParentName.Split('_');

                        // 단일부재
                        if (args.Length < 2)
                        {
                            //if(objParentName.Contains(CodeLv3.AP.ToString()) || objParentName.Contains(CodeLv3.SP.ToString()))
                            //{
                            //    Debug.Log(objParentName);

                            //    return targetTransform;
                            //}
                            //else
                            //{
                            //    return null;
                            //}
                            return null;
                        }
                        else
                        {
                            return targetTransform.parent;
                        }
					}
                    else if(MainManager.Instance.AppUseCase == Definition.UseCase.Tunnel)
					{
                        return targetTransform;
					}
                    else
					{
                        return targetTransform;
					}
                }
                //else if (_data.GetType().Equals(typeof(Indicator.Element.SingleIssueRecordInfo)))
                //{
                //    targetTransform = (_data as Indicator.Element.SingleIssueRecordInfo).transform;
                //}
                else if(_data.GetType().Equals(typeof(Skew)))
                {
                    targetTransform = (_data as Skew).transform;
                }
                else if (_data.GetType().Equals(typeof(Curve)))
                {
                    targetTransform = (_data as Curve).transform;
                }
                //else if (_data.GetType().Equals(typeof(Interactable)))
                //{
                //    targetTransform = (_data as Interactable).transform;
                //}
                else if (_data.GetType().Equals(typeof(CurveSkew)))
                {
                    targetTransform = (_data as CurveSkew).transform;
                }
                return targetTransform;
            }

            return null;
            
        }

        /// <summary>
        /// 상태코드 변경
        /// </summary>
        private void ChangeSceneStatus<T>(object _targetObj, Transform _targetTransform) where T : class
        {
            var _data = _targetObj as T;

            // 목표객체 타입 확인
            if(eventTypeDics.ContainsKey(_data.GetType()))
            {
                ViewSceneStatus sceneStatus = Manager.MainManager.Instance.SceneStatus;

                if (eventTypeDics[_data.GetType()].Equals(typeof(Manager.SceneChangeMenu)))
                {
                    Manager.SceneChangeMenu obj = _data as Manager.SceneChangeMenu;
                    switch (obj.selectedButton)
                    {
                        case SceneChangeMenu.SceneMenu.BridgeModel:
                            obj.ActiveButtonSelectedChild((int)SceneChangeMenu.SceneMenu.BridgeModel);
                            if (Manager.SceneChangeMenu.Instance.permitSceneChange)
                            {
                                MainManager.Instance.SceneStatus = ViewSceneStatus.Ready;
                                Manager.IssueManager.Instance.DisplayAll(false);
                            }
                            break;

                        case SceneChangeMenu.SceneMenu.BridgeDamageInfo:
                            obj.ActiveButtonSelectedChild((int)SceneChangeMenu.SceneMenu.BridgeDamageInfo);
                            if (Manager.SceneChangeMenu.Instance.permitSceneChange)
                            {
                                MainManager.Instance.SceneStatus = ViewSceneStatus.ViewAllDamage;
                                Manager.IssueManager.Instance.DisplayIssue();
                            }
                            break;

                        // TODO : 3단계, 4단계에서 키맵 활성화 및 기능 확인
                        case SceneChangeMenu.SceneMenu.PartsDamageInfo:
                            obj.ActiveButtonSelectedChild((int)SceneChangeMenu.SceneMenu.PartsDamageInfo);
                            if (Manager.SceneChangeMenu.Instance.permitSceneChange)
                            {
                                if (!Manager.SceneChangeMenu.Instance.oneCycleCheck)
                                {
                                    if(MainManager.Instance.AppUseCase == Definition.UseCase.Bridge)
									{
                                        //TODO : 2021_02_01 안상호 수정 : 이전에 선택된 객체가 없는 경우 초기값으로 AB01을 지정
                                        if (RuntimeData.RootContainer.Instance.cached3DInstance == null)
                                        {
                                            //foreach (Transform child in Manager.DimViewManager.Instance.objLevel3)
                                            //{
                                            //    if (child.name.Contains("AB"))
                                            //    {
                                            //        RuntimeData.SelectedObject defaultObj = new RuntimeData.SelectedObject(child.gameObject, child);
                                            //        RuntimeData.RootContainer.Instance.cached3DInstance = defaultObj;
                                            //        break;
                                            //    }
                                            //}
                                        }
									    else if (RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform == null)
									    {
                                            //foreach (Transform child in Manager.DimViewManager.Instance.objLevel3)
                                            //{
                                            //    if (child.name.Contains("AB"))
                                            //    {
                                            //        RuntimeData.SelectedObject defaultObj = new RuntimeData.SelectedObject(child.gameObject, child);
                                            //        RuntimeData.RootContainer.Instance.cached3DInstance = defaultObj;
                                            //        break;
                                            //    }
                                            //}
                                        }
									}
                                    else if(MainManager.Instance.AppUseCase == Definition.UseCase.Tunnel)
									{
                                        if(RuntimeData.RootContainer.Instance.cached3DInstance == null)
										{
                                            List<GameObject> objs = Data.Viewer.Cache.Instance.models.Objects;

                                            GameObject randObj = objs[UnityEngine.Random.Range(0, objs.Count)];

                                            RuntimeData.SelectedObject defaultObj = new RuntimeData.SelectedObject(randObj, randObj.transform);
                                            RuntimeData.RootContainer.Instance.cached3DInstance = defaultObj;
										}
                                        else
										{
                                            if (RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform == null)
										    {
                                                List<GameObject> objs = Data.Viewer.Cache.Instance.models.Objects;

                                                GameObject randObj = objs[UnityEngine.Random.Range(0, objs.Count)];

                                                RuntimeData.SelectedObject defaultObj = new RuntimeData.SelectedObject(randObj, randObj.transform);
                                                RuntimeData.RootContainer.Instance.cached3DInstance = defaultObj;
                                                //Dim.DimScript.Instance.OnInput(randObj.transform);

                                                //RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform = objs[UnityEngine.Random.Range(0, objs.Count)].transform;
                                                //Data.Viewer.Cache.Instance.models.Objects[UnityEngine.Random.RandomRange()]
                                            }
										}
									}
								}
                                MainManager.Instance.SceneStatus = ViewSceneStatus.ViewPartDamage;
                            }
                            break;
                        case SceneChangeMenu.SceneMenu.PartsRecoverInfo:
                            obj.ActiveButtonSelectedChild((int)SceneChangeMenu.SceneMenu.PartsRecoverInfo);
                            if (Manager.SceneChangeMenu.Instance.permitSceneChange)
                            {
                                if(MainManager.Instance.AppUseCase == Definition.UseCase.Bridge)
								{
                                    //TODO : 2021_02_01 안상호 수정 : 이전에 선택된 객체가 없는 경우 초기값으로 AB01을 지정
                                    if (RuntimeData.RootContainer.Instance.cached3DInstance == null)
                                    {
                                        //foreach (Transform child in Manager.DimViewManager.Instance.objLevel3)
                                        //{
                                        //    if (child.name.Contains("AB"))
                                        //    {
                                        //        RuntimeData.SelectedObject defaultObj = new RuntimeData.SelectedObject(child.gameObject, child);
                                        //        RuntimeData.RootContainer.Instance.cached3DInstance = defaultObj;
                                        //        break;
                                        //    }
                                        //}
                                    }
                                    else if (RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform == null)
                                    {
                                        //foreach (Transform child in Manager.DimViewManager.Instance.objLevel3)
                                        //{
                                        //    if (child.name.Contains("AB"))
                                        //    {
                                        //        RuntimeData.SelectedObject defaultObj = new RuntimeData.SelectedObject(child.gameObject, child);
                                        //        RuntimeData.RootContainer.Instance.cached3DInstance = defaultObj;
                                        //        break;
                                        //    }
                                        //}
                                    }
								}
                                else if(MainManager.Instance.AppUseCase == Definition.UseCase.Tunnel)
								{
                                    if(RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform == null)
									{
                                        List<GameObject> objs = Data.Viewer.Cache.Instance.models.Objects;

                                        GameObject randObj = objs[UnityEngine.Random.Range(0, objs.Count)];

                                        RuntimeData.SelectedObject defaultObj = new RuntimeData.SelectedObject(randObj, randObj.transform);
                                        RuntimeData.RootContainer.Instance.cached3DInstance = defaultObj;
                                        //Dim.DimScript.Instance.OnInput(randObj.transform);
                                        //RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform = objs[UnityEngine.Random.Range(0, objs.Count)].transform;
                                    }
								}

                                MainManager.Instance.SceneStatus = ViewSceneStatus.ViewPart2R;
                            }
                            break;
                        case SceneChangeMenu.SceneMenu.BridgeManagement:
                            obj.ActiveButtonSelectedChild((int)SceneChangeMenu.SceneMenu.BridgeManagement);
                            if (Manager.SceneChangeMenu.Instance.permitSceneChange)
                            {
                                //if(MainManager.Instance.AppUseCase == Definition.UseCase.Bridge)
								//{
                                //    if (RuntimeData.RootContainer.Instance.cached3DInstance == null)
                                //    {
                                //        foreach (Transform child in Manager.DimViewManager.Instance.objLevel3)
                                //        {
                                //            if (child.name.Contains("AB"))
                                //            {
                                //                RuntimeData.SelectedObject defaultObj = new RuntimeData.SelectedObject(child.gameObject, child);
                                //                RuntimeData.RootContainer.Instance.cached3DInstance = defaultObj;
                                //                break;
                                //            }
                                //        }
                                //    }
								//}
                                //else if(MainManager.Instance.AppUseCase == Definition.UseCase.Tunnel)
								//{
                                //    if(RuntimeData.RootContainer.Instance.cached3DInstance == null)
								//	{
                                //        List<GameObject> objs = Data.Viewer.Cache.Instance.models.Objects;
                                //
                                //        Transform selected = objs[UnityEngine.Random.Range(0, objs.Count)].transform;
                                //        RuntimeData.RootContainer.Instance.cached3DInstance.itemTransform = selected;
                                //        Dim.DimScript.Instance.OnInput(selected);
								//	}
								//}
                                MainManager.Instance.SceneStatus = ViewSceneStatus.ViewMaintainance;
                            }
                            break;
                    }
                }
                // O
                if (eventTypeDics[_data.GetType()].Equals(typeof(Indicator.Element.KeymapElement)))
                {
                    switch(sceneStatus)
                    {
                        //case ViewSceneStatus.ViewAllDamage:
                        //    Debug.Log($"초기단계 {_data.GetType()}");

                        //    return;

                        case ViewSceneStatus.ViewPartDamage:
                            Debug.Log($"특정객체 보기단계 {_data.GetType()}");
                            break;

                        case ViewSceneStatus.ViewPart2R:
                            Debug.Log($"특정객체 이력단계 {_data.GetType()}");
                            break;

                        //case ViewSceneStatus.ViewMaintainance:
                        //    Debug.Log($"특정손상 이력단계 {_data.GetType()}");
                        //    break;
                    }

					//Debug.Log($"****************** 초기단계 {_data.GetType()}");
					//Debug.Log($"****************** {_targetTransform.name}");


					if (_targetTransform != null)
					{
                        // 재귀실행임을 알리는 변수 갱신
                        MainManager.Instance.isRecursived = true;

						RuntimeData.RootContainer.Instance.cached3DInstance = new RuntimeData.SelectedObject();
						RuntimeData.RootContainer.Instance.cached3DInstance.Set(
							_targetTransform.GetComponent<Bridge.ObjectData>(), _targetTransform
							);
						if (MainManager.Instance.SceneStatus == ViewSceneStatus.ViewPartDamage)
							MainManager.Instance.SceneStatus = ViewSceneStatus.ViewPartDamage;
						if (MainManager.Instance.SceneStatus == ViewSceneStatus.ViewPart2R)
							MainManager.Instance.SceneStatus = ViewSceneStatus.ViewPart2R;
					}
				}
                // TODO
                if (eventTypeDics[_data.GetType()].Equals(typeof(Issue.DamagedIssue)))
                {
                    switch (sceneStatus)
                    {
                        //case ViewSceneStatus.ViewAllDamage:
                        //    Debug.Log($"초기단계 {_data.GetType()}");
                        //    return;

                        case ViewSceneStatus.ViewPartDamage:
                            Debug.Log($"특정객체 보기단계 {_data.GetType()}");
                            break;

                        case ViewSceneStatus.ViewPart2R:
                            Debug.Log($"특정객체 이력단계 {_data.GetType()}");
                            break;

                            //case ViewSceneStatus.ViewMaintainance:
                            //    Debug.Log($"특정손상 이력단계 {_data.GetType()}");
                            //    return;
                    }

                    Transform _viewIssueTransform = RuntimeData.RootContainer.Instance.rootViewIssue;

                    // issue정보 초기화
                    Issue.DamagedIssue _issue = _data as Issue.DamagedIssue;

                    GameObject obj = new GameObject(_issue.IssueOrderCode);
                    obj.AddComponent<Issue.DamagedIssue>();
                    obj.GetComponent<Issue.DamagedIssue>().Set(_issue);

                    RuntimeData.RootContainer.Instance.cachedIssueInstance = new RuntimeData.SelectedObject();
                    RuntimeData.RootContainer.Instance.cachedIssueInstance.item = obj.GetComponent<Issue.DamagedIssue>();

                    obj.transform.SetParent(_viewIssueTransform);

                    //_sceneMenu.selectedButton = (SceneChangeMenu.SceneMenu)4;
                    _sceneMenu.ActiveButtonSelectedChild((int)SceneChangeMenu.SceneMenu.BridgeManagement);

                    MainManager.Instance.SceneStatus = ViewSceneStatus.ViewMaintainance;
                }

                if (eventTypeDics[_data.GetType()].Equals(typeof(Issue.RecoveredIssue)))
                {
                    switch (sceneStatus)
                    {
                        //case ViewSceneStatus.ViewAllDamage:
                        //    Debug.Log($"초기단계 {_data.GetType()}");
                        //    return;

                        case ViewSceneStatus.ViewPartDamage:
                            Debug.Log($"특정객체 보기단계 {_data.GetType()}");
                            break;

                        case ViewSceneStatus.ViewPart2R:
                            Debug.Log($"특정객체 이력단계 {_data.GetType()}");
                            break;

                            //case ViewSceneStatus.ViewMaintainance:
                            //    Debug.Log($"특정손상 이력단계 {_data.GetType()}");
                            //    return;
                    }

                    Transform _viewIssueTransform = RuntimeData.RootContainer.Instance.rootViewIssue;

                    // issue정보 초기화
                    Issue.RecoveredIssue _issue = _data as Issue.RecoveredIssue;

                    GameObject obj = new GameObject(_issue.IssueOrderCode);
                    obj.AddComponent<Issue.RecoveredIssue>();
                    obj.GetComponent<Issue.RecoveredIssue>().Set(_issue);

                    RuntimeData.RootContainer.Instance.cachedIssueInstance = new RuntimeData.SelectedObject();
                    RuntimeData.RootContainer.Instance.cachedIssueInstance.item = obj.GetComponent<Issue.RecoveredIssue>();

                    obj.transform.SetParent(_viewIssueTransform);

                    //_sceneMenu.selectedButton = (SceneChangeMenu.SceneMenu)4;
                    _sceneMenu.ActiveButtonSelectedChild((int)SceneChangeMenu.SceneMenu.BridgeManagement);

                    MainManager.Instance.SceneStatus = ViewSceneStatus.ViewMaintainance;
                }

                return;

            }
        }

        /// <summary>
        /// 객체요소 상태변경
        /// </summary>
        /// <param name="selectedObj"></param>
        /// <param name="currentObj"></param>
        private void ChangeElementStatus(object _selectedObj, Transform _slcTransform, object _currentObj, Transform _currentTransform)
        {
            //return;
            if(_selectedObj != null)
            {
                if(eventTypeDics.ContainsKey(_selectedObj.GetType()))
                {
                    if (eventTypeDics[_selectedObj.GetType()].Equals(typeof(Bridge.ObjectData)))
                    {
                        // 객체 선택상태 해제
                        if(_slcTransform != null)
                        {
                            Bridge.ObjectData objData;
                            if(_slcTransform.TryGetComponent<Bridge.ObjectData>(out objData))
                            {
                                objData.SetMaterial(_isSelected: false);
                                objData.IsSelected = false;
                            }
                        }
                    }

                    else if (eventTypeDics[_selectedObj.GetType()].Equals(typeof(Indicator.Element.KeymapElement)))
                    {
                        // 객체 선택상태 해제
                        if(_slcTransform != null)
                        {
                            Bridge.ObjectData objData;
                            if(_slcTransform.TryGetComponent<Bridge.ObjectData>(out objData))
                            {
                                objData.SetMaterial(_isSelected: false);
                                objData.IsSelected = false;
                            }
                        }
                    }

                    else if (eventTypeDics[_selectedObj.GetType()].Equals(typeof(Issue.DamagedIssue)))
                    {

                    }

                    else if (eventTypeDics[_selectedObj.GetType()].Equals(typeof(Issue.RecoveredIssue)))
                    {

                    }

                    //else if (eventTypeDics[_selectedObj.GetType()].Equals(typeof(Indicator.Element.SingleIssueRecordInfo)))
                    //{
                    //    (_selectedObj as Indicator.Element.SingleIssueRecordInfo).isSelected = false;
                    //}
                }
            }

            if(_currentObj != null)
            {
                if(eventTypeDics.ContainsKey(_currentObj.GetType()))
                {
                    if (eventTypeDics[_currentObj.GetType()].Equals(typeof(Bridge.ObjectData)))
                    {
                        // 객체 선택상태 해제
                        if(_currentTransform != null)
                        {
                            Bridge.ObjectData objData;
                            if(_currentTransform.TryGetComponent<Bridge.ObjectData>(out objData))
                            {
                                objData.SetMaterial(_isSelected: true);
                                objData.IsSelected = true;
                            }
                        }
                    }

                    else if (eventTypeDics[_currentObj.GetType()].Equals(typeof(Indicator.Element.KeymapElement)))
                    {
                        // 객체 선택상태 해제
                        if(_currentTransform != null)
                        {
                            Bridge.ObjectData objData;
                            if(_currentTransform.TryGetComponent<Bridge.ObjectData>(out objData))
                            {
                                objData.SetMaterial(_isSelected: true);
                                objData.IsSelected = true;
                            }
                        }
                    }

                    else if (eventTypeDics[_currentObj.GetType()].Equals(typeof(Issue.DamagedIssue)))
                    {

                    }

                    else if (eventTypeDics[_currentObj.GetType()].Equals(typeof(Issue.RecoveredIssue)))
                    {

                    }

                    //else if (eventTypeDics[_currentObj.GetType()].Equals(typeof(Indicator.Element.SingleIssueRecordInfo)))
                    //{
                    //    (_currentObj as Indicator.Element.SingleIssueRecordInfo).isSelected = true;
                    //}
                }
            }
        }

        #endregion

        #region 상태변경 스택 관리

        /// <summary>
        /// 새 이벤트 스택을 선택 상태에 할당
        /// </summary>
        /// <param name="_cache"></param>
        public void GetNewEventCache(EventCache _cache)
        {
            // 현재 캐시 뒤로가기 스택 푸시
            backwardStack.Push(currentCache);

            // 받은 캐시 현재 캐시로 할당
            currentCache = _cache;

            // 앞으로가기 스택 비우기
            forwardStack.Clear();
        }

        /// <summary>
        /// 뒤로가기 (이전 상태)
        /// </summary>
        public void SetBackwardEvent()
        {
            // 뒤로가기 스택이 1개 이상인 경우
            if(backwardStack.Count > 0)
            {
                // 뒤로가기 스택 pool, 변수 받음
                EventCache _cache = backwardStack.Pop();

                // 앞으로가기 스택 현재 상태 변수 push
                forwardStack.Push(currentCache);

                // 현재 상태 변수 받은 캐시 할당
                currentCache = _cache;
                //forwardStack.Push(_cache);
                
                // 상태변경 이벤트 실행

                System.Type _type = _cache.type;

                if(_type == typeof(Manager.SceneChangeMenu))
                {
                    (_cache.targetObj as Manager.SceneChangeMenu).selectedButton = _cache.menu;
                    //Manager.SceneChangeMenu.Instance.selectedButton = _cache.menu;
                    ChangeSceneStatus<Manager.SceneChangeMenu>(_cache.targetObj, _cache.targetTransform);
                }
                else if(_type == typeof(Indicator.Element.KeymapElement))
                {
                    ChangeSceneStatus<Indicator.Element.KeymapElement>(_cache.targetObj, _cache.targetTransform);
                }
                else if (_type == typeof(Issue.DamagedIssue))
                {
                    ChangeSceneStatus<Issue.DamagedIssue>(_cache.targetObj, _cache.targetTransform);
                }
                else if (_type == typeof(Issue.RecoveredIssue))
                {
                    ChangeSceneStatus<Issue.RecoveredIssue>(_cache.targetObj, _cache.targetTransform);
                }

            }
        }

        /// <summary>
        /// 앞으로가기 (돌아갔었던 최근상태)
        /// </summary>
        public void SetForwardEvent()
        {
            // 앞으로가기 스택이 1개 이상인 경우
            if(forwardStack.Count > 0)
            {
                // 앞으로가기 스택 pool, 변수 받음
                EventCache _cache = forwardStack.Pop();

                // 뒤로가기 스택 현재 상태 변수할당
                backwardStack.Push(currentCache);

                // 현재 상태 변수 받은 변수 할당
                currentCache = _cache;

                // 상태변경 이벤트 실행
                System.Type _type = _cache.type;

                if (_type == typeof(Manager.SceneChangeMenu))
                {
                    (_cache.targetObj as Manager.SceneChangeMenu).selectedButton = _cache.menu;
                    //Manager.SceneChangeMenu.Instance.selectedButton = _cache.menu;
                    ChangeSceneStatus<Manager.SceneChangeMenu>(_cache.targetObj, _cache.targetTransform);
                }
                else if (_type == typeof(Indicator.Element.KeymapElement))
                {
                    ChangeSceneStatus<Indicator.Element.KeymapElement>(_cache.targetObj, _cache.targetTransform);
                }
                else if (_type == typeof(Issue.DamagedIssue))
                {
                    ChangeSceneStatus<Issue.DamagedIssue>(_cache.targetObj, _cache.targetTransform);
                }
                else if (_type == typeof(Issue.RecoveredIssue))
                {
                    ChangeSceneStatus<Issue.RecoveredIssue>(_cache.targetObj, _cache.targetTransform);
                }
            }
        }

        #endregion
    }
}
