using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.UI
{
    using Definition;
    using TMPro;
    using UnityEngine.UI;
    using View;

    public partial class Element_BottomBar : AElement
    {
		[System.Serializable]
		public class State_Image
        {
			public Color m_defaultColor;
			public Color m_selectedColor;
			public Sprite m_defaultSprite;
			public Sprite m_selectedSprite;

			public void Set_Default(Image _tg)
            {
				_tg.sprite = m_defaultSprite;
				_tg.color = m_defaultColor;
            }

			public void Set_Selected(Image _tg)
            {
				_tg.sprite = m_selectedSprite;
				_tg.color = m_selectedColor;
			}
        }

		[System.Serializable]
		public class Group_Element
        {
			public List<Image> m_img;
			public List<bool> m_pinned;

			public void On(State_Image _state)
			{
				m_img.ForEach(x =>
				{
					_state.Set_Selected(x);
				});
			}

			public void On(GameObject _tg, State_Image _state)
            {
				m_img.ForEach(x =>
				{
					if (x.gameObject == _tg)
					{
						_state.Set_Selected(x);
					}
					else
					{
						_state.Set_Default(x);
					}
				});
			}

			public void Off(State_Image _state)
			{
				m_img.ForEach(x =>
				{
					_state.Set_Default(x);
				});
			}

			public void Toggle(GameObject _tg, State_Image _state)
			{
				m_img.ForEach(x =>
				{
					if (x.gameObject == _tg)
					{
						_state.Set_Selected(x);
					}
					else
					{
						_state.Set_Default(x);
					}
				});
			}
		}

		[System.Serializable]
		public class Children
        {
			[SerializeField] List<GameObject> list;

            public List<GameObject> _List { get => list; set => list = value; }

			public void On()
            {
				list.ForEach(x => x.SetActive(true));
            }

			public void Off()
            {
				list.ForEach(x => x.SetActive(false));
            }

			public void Toggle(GameObject _target)
            {
				list.ForEach(x => x.SetActive(x == _target && !x.activeSelf));
            }
        }

		[System.Serializable]
		public class Settings
		{
			public Slider m_iconSizeSlider;
			public Slider m_transparencySlider;
			public Slider m_sensitivitySlider;
			public Slider m_fontSizeSlider;

			[SerializeField] TextMeshProUGUI m_iconSizeText;
			[SerializeField] TextMeshProUGUI m_transparencyText;
			[SerializeField] TextMeshProUGUI m_sensitivityText;
			[SerializeField] TextMeshProUGUI m_fontSizeText;

			public void ResetSlider_iconSize()
			{
				if (m_iconSizeSlider == null) return;
				m_iconSizeSlider.value = 1;
				m_iconSizeText.text = "100";
			}

			public void ResetSlider_transparency()
			{
				if (m_transparencySlider == null) return;
				m_transparencySlider.value = 1;
				m_transparencyText.text = "100";
			}

			public void ResetSlider_Sensitivity()
            {
				if (m_sensitivitySlider == null) return;
				m_sensitivitySlider.value = 1;
				m_sensitivityText.text = "100";
            }

			public void ResetSlider_fontSize()
            {
				if (m_fontSizeSlider == null) return;
				m_fontSizeSlider.value = 1;
				m_fontSizeText.text = "100";
            }

			public void Update_iconSize(float _value)
            {
				if (m_iconSizeText == null) return;
				m_iconSizeText.text = ((int)(_value * 100)).ToString();
            }

			public void Update_transparency(float _value)
            {
				if (m_transparencyText == null) return;
				m_transparencyText.text = ((int)(_value * 100)).ToString();
			}

			public void Update_sensitivity(float _value)
            {
				if (m_sensitivityText == null) return;
				m_sensitivityText.text = ((int)(_value * 100)).ToString();
			}

			public void Update_fontSize(float _value)
            {
				if (m_fontSizeText == null) return;
				m_fontSizeText.text = ((int)(_value * 100)).ToString();
			}
		}

		[System.Serializable]
		public class Resource
        {
			[Header("State")]
			public State_Image m_bottomBarState;
			public State_Image m_subElementState;

			/*
			 * 0 bottomBar
			 * 1 camera
			 * 2 view
			 * 3 orthogonal
			 * 4 show
			 */
			[Header("elements")]
			public List<Group_Element> m_groups;

			public List<GameObject> m_defaultGroup;

			public void On_Group(int index)
            {
				m_groups[index].On(index == 0 ? m_bottomBarState : m_subElementState);
            }

			public void On_Group(int index, GameObject _tg)
            {
				m_groups[index].On(_tg, index == 0 ? m_bottomBarState : m_subElementState);
            }

			public void Off_Group(int index)
            {
				m_groups[index].Off(index == 0 ? m_bottomBarState : m_subElementState);
			}

			public void Toggle_Group(int index, GameObject _tg)
            {
				m_groups[index].Toggle(_tg, index == 0 ? m_bottomBarState: m_subElementState);
			}

			public void Reset()
            {
                for (int i = 0; i < m_groups.Count; i++)
                {
					m_groups[i].Toggle(m_defaultGroup[i], i == 0 ? m_bottomBarState : m_subElementState);
                }
            }
        }

		[SerializeField] Children children;
		[SerializeField] Settings settings;
		[SerializeField] Resource resource;

        public Children _Children { get => children; set => children = value; }
        public Settings _Settings { get => settings; set => settings = value; }
        public Resource _Resource { get => resource; set => resource = value; }
    }
}
