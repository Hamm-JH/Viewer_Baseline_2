using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Manager
{
    public class SceneChangeMenu : MonoBehaviour
    {
        public enum SceneMenu
        {
            BridgeModel = 0,
            BridgeDamageInfo = 1,
            PartsDamageInfo = 2,
            PartsRecoverInfo = 3,
            BridgeManagement = 4
        }

        public class NavigatorElement
        {
            public Image nav_BG;
            public Image nav_image;
            public TextMeshProUGUI nav_text;

            public NavigatorElement() { }

            public NavigatorElement(Image _BG, Image _image, TextMeshProUGUI _text)
            {
                nav_BG = _BG;
                nav_image = _image;
                nav_text = _text;
            }

            public void Init(Image _BG, Image _image, TextMeshProUGUI _text)
            {
                nav_BG = _BG;
                nav_image = _image;
                nav_text = _text;
            }
        }

        #region Navigation resources

        [Header("procedure arrows")]
        [SerializeField] private List<Image> procedureArrows;

        [Header("Sprite resource")]
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Sprite SelectedSprite;

        [Header("Nav 1")]
        [SerializeField] private Image nav1_BG;
        [SerializeField] private Image nav1_Icon;
        [SerializeField] private TextMeshProUGUI nav1_Text;

        [Header("Nav 2")]
        [SerializeField] private Image nav2_BG;
        [SerializeField] private Image nav2_Icon;
        [SerializeField] private TextMeshProUGUI nav2_Text;

        [Header("Nav 3")]
        [SerializeField] private Image nav3_BG;
        [SerializeField] private Image nav3_Icon;
        [SerializeField] private TextMeshProUGUI nav3_Text;

        [Header("Nav 4")]
        [SerializeField] private Image nav4_BG;
        [SerializeField] private Image nav4_Icon;
        [SerializeField] private TextMeshProUGUI nav4_Text;

        [Header("Nav 5")]
        [SerializeField] private Image nav5_BG;
        [SerializeField] private Image nav5_Icon;
        [SerializeField] private TextMeshProUGUI nav5_Text;

        #endregion

        #region Connector

        private Dictionary<SceneMenu, NavigatorElement> navElement;

        #endregion

        public List<GameObject> SceneButton;
        public List<bool> isActiveBtn;
        public List<Image> sceneStatusLinker;
        public SceneMenu selectedButton;
        public GameObject alarmPopup;

        // 프로그램 시작 직후 각 단계를 한번씩 이동. 이후 이동은 제한 없음
        public bool permitSceneChange;
        public bool oneCycleCheck;
        public int oneCycleCheckCount;

        #region Instance

        private static SceneChangeMenu _instance;

        public static SceneChangeMenu Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SceneChangeMenu>();
                }
                return _instance;
            }
        }

        #endregion

        private void Awake()
        {
            navElement = InitNavElement();
        }

        private void Start()
        {
            navElement = InitNavElement();

            selectedButton = SceneMenu.BridgeModel;

            permitSceneChange = false;
            oneCycleCheck = false;
            oneCycleCheckCount = 0;
            alarmPopup.SetActive(false);

            for (int i = 0; i < isActiveBtn.Count; i++)
                isActiveBtn[i] = true;

            ButtonStatus();
        }

        private Dictionary<SceneMenu, NavigatorElement> InitNavElement()
        {
            var init = new Dictionary<SceneMenu, NavigatorElement>();

            NavigatorElement nav1 = new NavigatorElement(nav1_BG, nav1_Icon, nav1_Text);
            NavigatorElement nav2 = new NavigatorElement(nav2_BG, nav2_Icon, nav2_Text);
            NavigatorElement nav3 = new NavigatorElement(nav3_BG, nav3_Icon, nav3_Text);
            NavigatorElement nav4 = new NavigatorElement(nav4_BG, nav4_Icon, nav4_Text);
            NavigatorElement nav5 = new NavigatorElement(nav5_BG, nav5_Icon, nav5_Text);

            init.Add(SceneMenu.BridgeModel, nav1);
            init.Add(SceneMenu.BridgeDamageInfo, nav2);
            init.Add(SceneMenu.PartsDamageInfo, nav3);
            init.Add(SceneMenu.PartsRecoverInfo, nav4);
            init.Add(SceneMenu.BridgeManagement, nav5);

            return init;
        }

        public void ButtonStatus()
        {
            for (int i = 0; i < isActiveBtn.Count; i++)
            {
                if (isActiveBtn[i])
                {
                    SceneButton[i].GetComponent<Button>().interactable = true;
                    SceneButton[i].transform.GetChild(1).GetComponent<Image>().color = new Color(1f, 1f, 1f);
                }
                else
                {
                    SceneButton[i].GetComponent<Button>().interactable = false;
                    SceneButton[i].transform.GetChild(1).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                }
            }
        }

        public void SelectButton(int btnNum)
        {
            selectedButton = (SceneMenu)btnNum;
            Manager.EventClassifier.Instance.OnEvent<SceneChangeMenu>(Control.Status.Click, gameObject.GetComponent<SceneChangeMenu>());
        }

        public void ActiveButtonSelectedChild(int btnNum)
        {
            int count = Enum.GetValues(typeof(SceneMenu)).Length;

            if (btnNum < 0 || btnNum >= count)
            {
                return;
            }

            // TODO : 2021_02_01 안상호 수정 : 기존 1사이클 조건 부분 주석처리
            //if (!oneCycleCheck)
            //{
            //    if ((oneCycleCheckCount + 1) == btnNum)
            //    {
            //        oneCycleCheckCount += 1;
            //        for (int i = 0; i < SceneButton.Count; i++)
            //        {
            //            if (i == btnNum)
            //            {
            //                SceneButton[i].transform.GetChild(0).gameObject.SetActive(true);
            //                SceneButton[i].transform.GetChild(1).gameObject.SetActive(false);
            //                sceneStatusLinker[i - 1].sprite = Resources.Load<Sprite>("Icon/SceneChangeMenu/ICON_LinkerActivation");
            //            }
            //            else
            //            {
            //                SceneButton[i].transform.GetChild(0).gameObject.SetActive(false);
            //                SceneButton[i].transform.GetChild(1).gameObject.SetActive(true);
            //            }
            //        }
            //        permitSceneChange = true;
            //    }
            //    else if (oneCycleCheckCount == 0 && btnNum == 0)
            //    {
            //        SceneButton[0].transform.GetChild(0).gameObject.SetActive(true);
            //        SceneButton[0].transform.GetChild(1).gameObject.SetActive(false);
            //        permitSceneChange = true;
            //    }
            //    else if ((oneCycleCheckCount + 1) != btnNum)
            //    {
            //        permitSceneChange = false;
            //        AlarmControl(true);
            //    }

            //    if (oneCycleCheckCount == 4)
            //    {
            //        oneCycleCheck = true;
            //        permitSceneChange = true;
            //    }
            //}
            //else
            //{
            permitSceneChange = true;
            //for (int i = 0; i < SceneButton.Count; i++)
            //{
            //    if (i == btnNum)
            //    {
            //        SceneButton[i].transform.GetChild(0).gameObject.SetActive(true);
            //        SceneButton[i].transform.GetChild(1).gameObject.SetActive(false);
            //    }
            //    else
            //    {
            //        SceneButton[i].transform.GetChild(0).gameObject.SetActive(false);
            //        SceneButton[i].transform.GetChild(1).gameObject.SetActive(true);
            //    }
            //}

            for (SceneMenu i = SceneMenu.BridgeModel; i <= SceneMenu.BridgeManagement; i++)
            {
                SetNavigationState(
                    (SceneMenu)btnNum == i,
                    navElement[(SceneMenu)i]
                    );

                //Debug.Log(((SceneMenu)btnNum).ToString());
                //Debug.Log((SceneMenu)btnNum == i);
            }

            for (int i = 0; i < procedureArrows.Count; i++)
            {
                SetNavigationArrow(
                    _arrows: procedureArrows,
                    _index: i,
                    isOn: (i < btnNum)
                    );
            }
            //}
        }

        public void AlarmControl(bool check)
        {
            alarmPopup.SetActive(check);
        }

        #region Set Navigation state

        private void SetNavigationState(bool isSelected, NavigatorElement _element)
        {
            _element.nav_BG.sprite = SetSprite(isSelected);
            _element.nav_image.color = SetColor(isSelected);
            _element.nav_text.color = SetColor(isSelected);
        }

        private Sprite SetSprite(bool isSelected)
        {
            if (isSelected)
            {
                return SelectedSprite;
            }
            else
            {
                return defaultSprite;
            }
        }

        private Color SetColor(bool isSelected)
        {
            return UI.Colors.Instance.Set(
                    _layer: UI.Colors.TargetLayer.Top_Navigation,
                    _item: UI.Colors.TargetItem.Navigation_Button,
                    _isOn: isSelected
                    );
        }

        #endregion

        #region Set Navagation arrow

        private void SetNavigationArrow(List<Image> _arrows, int _index, bool isOn)
        {
            _arrows[_index].color = UI.Colors.Instance.Set(
                _layer: UI.Colors.TargetLayer.Top_Navigation,
                _item:  UI.Colors.TargetItem.Navigation_Arrow,
                _isOn:  isOn
                );
        }

        #endregion
    }
}
