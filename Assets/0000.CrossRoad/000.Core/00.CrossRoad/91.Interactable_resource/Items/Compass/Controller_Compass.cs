using Definition;
using Management;
using Module.Graphic;
using Module.Interaction;
using Module.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using View;

namespace Items
{
    public class Controller_Compass : AItem
    {
        [System.Serializable]
        public class Arrow
        {
            public Transform m_target;
            public Transform m_seeker;
            public RectTransform m_arm;
        }

        /// <summary>
        /// 회전 기준점 
        /// </summary>
        [SerializeField]
        private Transform m_me;

        /// <summary>
        /// Root UI
        /// </summary>
        [SerializeField]
        private GameObject m_compassUIRoot;

        /// <summary>
        /// Arrow들
        /// </summary>
        [SerializeField]
        private List<Arrow> m_arrows;

        /// <summary>
        /// 나침반 pitch 각도
        /// </summary>
        [SerializeField]
        private float m_compassPitch = 60;

        public Transform Me { get => m_me; set => m_me = value; }
        public GameObject CompassUIRoot { get => m_compassUIRoot; set => m_compassUIRoot = value; }
        public List<Arrow> Arrows { get => m_arrows; set => m_arrows = value; }
        public float CompassPitch { get => m_compassPitch; set => m_compassPitch = value; }

        // Start is called before the first frame update
        void Start()
        {
            //AddCompass(testTrs);
        }

        public void AddCompass(List<Transform> _targets, Module_Graphic _graphic)
        {
            //arrows = new List<Arrow>();
            ClearCompass();

            int index = _targets.Count;
            for (int i = 0; i < index; i++)
            {
                AddCompass(_targets[i], i, in _graphic);
            }
            //_targets.ForEach(x => AddCompass(x, in _graphic));
        }

        private void ClearCompass()
        {
            int index = CompassUIRoot.transform.childCount;
            for (int i = 0; i < index; i++)
            {
                Destroy(CompassUIRoot.transform.GetChild(i).gameObject);
            }

            index = transform.childCount;
            for (int i = 0; i < index; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            Arrows = new List<Arrow>();
        }

        private void AddCompass(Transform _target, int _index, in Module_Graphic _graphic)
        {
            PlatformCode pCode = MainManager.Instance.Platform;

            Arrow arrow = new Arrow();

            GameObject seeker = new GameObject($"Seeker {Arrows.Count}");
            seeker.transform.parent = transform;

            GameObject arm = Instantiate(Resources.Load<GameObject>("Items/Compass Arm"), CompassUIRoot.transform);
            arm.transform.parent = CompassUIRoot.transform;

            UI_Compass compass;
            if(TryGetChildUICompass(arm.transform, out compass))
            {
                Compass_EventType eType = _index == 0 ? Compass_EventType.Compass_FirstPosition : Compass_EventType.Compass_LastPosition;
                Module_Interaction interaction = ContentManager.Instance.Module<Module_Interaction>();

                AUI aui;
                if(Platforms.IsDemoWebViewer(pCode))
                {
                    aui = interaction.UiInstances.First();
                }
                else
                {
                    aui = interaction.UiInstances.Last();
                }

                compass.Init(eType, aui, _graphic);
            }

            arrow.m_target = _target;
            arrow.m_seeker = seeker.transform;
            arrow.m_arm = arm.GetComponent<RectTransform>();

            Arrows.Add(arrow);
        }

        private bool TryGetChildUICompass(Transform _source, out UI_Compass _target)
        {
            _target = null;

            int index = _source.childCount;
            for (int i = 0; i < index; i++)
            {
                if(_source.GetChild(i).TryGetComponent<UI_Compass>(out _target))
                {
                    return true;
                }
            }

            return false;
        }

        // Update is called once per frame
        void Update()
        {
            // me - target간 각도 계산
            // eulerAngle.y 구해짐
            // 이를 arm의 eulerAngle.z에 대응

            if (!CanProcessing()) return;

            Vector3 mePos = default(Vector3);

            List<Vector3> tgPoses = new List<Vector3>();

            SetTargetPoses(out mePos, Arrows, out tgPoses);

            LookTargets(mePos, Arrows, tgPoses);
        }

        private bool CanProcessing()
        {
            bool result = false;

            if (Me == null || CompassUIRoot == null) { }
            else
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 위치값 할당
        /// </summary>
        /// <param name="_mePos"></param>
        /// <param name="_arrows"></param>
        /// <param name="_tgPoses"></param>
        private void SetTargetPoses(out Vector3 _mePos, List<Arrow> _arrows, out List<Vector3> _tgPoses)
        {
            _mePos = Me.position;
            _mePos = new Vector3(_mePos.x, 0, _mePos.z);

            Vector3 source = default(Vector3);

            _tgPoses = new List<Vector3>();

            foreach (Arrow arrow in _arrows)
            {
                source = arrow.m_target.position;
                source = new Vector3(source.x, 0, source.z);
                _tgPoses.Add(source);
            }
        }

        /// <summary>
        /// 위치점 보기
        /// </summary>
        /// <param name="_mePos"></param>
        /// <param name="_arrows"></param>
        /// <param name="_tgPoses"></param>
        private void LookTargets(Vector3 _mePos, List<Arrow> _arrows, List<Vector3> _tgPoses)
        {
            int index = _arrows.Count;
            for (int i = 0; i < index; i++)
            {
                _arrows[i].m_seeker.position = _mePos;
                _arrows[i].m_seeker.LookAt(_tgPoses[i]);

                _arrows[i].m_arm.rotation = Quaternion.Euler(CompassPitch, 0,
                    -_arrows[i].m_seeker.rotation.eulerAngles.y + Me.transform.rotation.eulerAngles.y);
            }
        }









        public override void UpdateState(List<ModuleCode> _mList)
        {
            throw new System.NotImplementedException();
        }


    }
}
