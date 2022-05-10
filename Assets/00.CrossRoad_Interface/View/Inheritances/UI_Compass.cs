using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace View
{
    public class UI_Compass : Interactable,
        IPointerEnterHandler, IPointerExitHandler
    {
        public override GameObject Target
        {
            get { return gameObject; }
        }

        public override List<GameObject> Targets => throw new System.NotImplementedException();

        public override void OnChangeValue(float _value)
        {
            throw new System.NotImplementedException();
        }

        public override void OnDeselect()
        {
            throw new System.NotImplementedException();
        }

        public override void OnDeselect<T1, T2>(T1 t1, T2 t2)
        {
            throw new System.NotImplementedException();
        }


        public override void OnSelect()
        {
            Debug.Log($"I am {gameObject.name} click");
        }

        Button m_btn;

        // Start is called before the first frame update
        void Start()
        {
            if(gameObject.TryGetComponent<Button>(out m_btn))
            {
                m_btn.onClick.AddListener(new UnityEngine.Events.UnityAction(OnSelect));
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log($"I am {gameObject.name} enter");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log($"I am {gameObject.name} exit");
        }
    }
}
