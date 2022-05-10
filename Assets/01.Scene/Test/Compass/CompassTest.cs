using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Test
{
    public class CompassTest : MonoBehaviour
    {
        [System.Serializable]
        public class Arrow
        {
            public Transform m_target;
            public Transform m_look;
            public RectTransform arm;

        }

        public List<Transform> testTrs;

        /// <summary>
        /// ȸ�� ������
        /// </summary>
        public Transform me;

        /// <summary>
        /// Root UI
        /// </summary>
        public GameObject m_uiRoot;

        /// <summary>
        /// Arrow��
        /// </summary>
        public List<Arrow> arrows;

        /// <summary>
        /// ��ħ�� pitch ����
        /// </summary>
        [SerializeField]
        private float m_compassPitch;

        /// <summary>
        /// �����?
        /// </summary>
        //public List<Transform> targets;

        // Start is called before the first frame update
        void Start()
        {
            AddCompass(testTrs);
        }

        public void AddCompass(List<Transform> _targets)
        {
            //arrows = new List<Arrow>();
            ClearCompass();

            _targets.ForEach(x => AddCompass(x));
        }

        private void ClearCompass()
        {
            int index = m_uiRoot.transform.childCount;
            for (int i = 0; i < index; i++)
            {
                Destroy(m_uiRoot.transform.GetChild(i).gameObject);
            }

            index = transform.childCount;
            for (int i = 0; i < index; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            arrows = new List<Arrow>();
        }

        private void AddCompass(Transform _target)
        {
            Arrow arrow = new Arrow();

            GameObject seeker = new GameObject($"Seeker {arrows.Count}");
            seeker.transform.parent = transform;

            GameObject arm = Instantiate(Resources.Load<GameObject>("Items/Compass Arm"), m_uiRoot.transform);
            arm.transform.parent = m_uiRoot.transform;

            arrow.m_target = _target;
            arrow.m_look = seeker.transform;
            arrow.arm = arm.GetComponent<RectTransform>();

            arrows.Add(arrow);
        }

        // Update is called once per frame
        void Update()
        {
            // me - target�� ���� ���
            // eulerAngle.y ������
            // �̸� arm�� eulerAngle.z�� ����

            Vector3 mePos = default(Vector3);

            List<Vector3> tgPoses = new List<Vector3>();

            SetTargetPoses(out mePos, arrows, out tgPoses);

            LookTargets(mePos, arrows, tgPoses);
        }



        /// <summary>
        /// ��ġ�� �Ҵ�
        /// </summary>
        /// <param name="_mePos"></param>
        /// <param name="_arrows"></param>
        /// <param name="_tgPoses"></param>
        private void SetTargetPoses(out Vector3 _mePos, List<Arrow> _arrows, out List<Vector3> _tgPoses)
        {
            _mePos = me.position;
            _mePos = new Vector3(_mePos.x, 0, _mePos.z);

            Vector3 source = default(Vector3);

            _tgPoses = new List<Vector3>();

            foreach(Arrow arrow in _arrows)
            {
                source = arrow.m_target.position;
                source = new Vector3(source.x, 0, source.z);
                _tgPoses.Add(source);
            }
        }

        /// <summary>
        /// ��ġ�� ����
        /// </summary>
        /// <param name="_mePos"></param>
        /// <param name="_arrows"></param>
        /// <param name="_tgPoses"></param>
        private void LookTargets(Vector3 _mePos, List<Arrow> _arrows, List<Vector3> _tgPoses)
        {
            int index = _arrows.Count;
            for (int i = 0; i < index; i++)
            {
                _arrows[i].m_look.position = _mePos;
                _arrows[i].m_look.LookAt(_tgPoses[i]);

                _arrows[i].arm.rotation = Quaternion.Euler(m_compassPitch, 0,
                    -_arrows[i].m_look.rotation.eulerAngles.y + me.transform.rotation.eulerAngles.y);
            }
        }
    }
}
