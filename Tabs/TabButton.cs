using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CustomUIComponents.Tabs
{
	[RequireComponent(typeof(Image))]
	public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
	{
		public TabGroup tabGroup;
		[HideInInspector] public Image background;
		[HideInInspector] public TextMeshProUGUI text;

		public UnityEvent onTabSelected;
		public UnityEvent onTabDeselected;

		private void OnValidate()
		{
			if (text == null)
				text = GetComponentInChildren<TextMeshProUGUI>();

			if (tabGroup == null)
				tabGroup = GetComponentInParent<TabGroup>();
		}

		private void Awake()
		{
			background = GetComponent<Image>();
			tabGroup.Subscribe(this);
		}

		public void Select()
		{
			onTabSelected?.Invoke();
			tabGroup.onTabSelected?.Invoke(this);
		}

		public void Deselect()
		{
			onTabDeselected?.Invoke();
			tabGroup.onTabDeselected?.Invoke(this);
		}
		
		public void OnPointerEnter(PointerEventData eventData) => tabGroup.OnTabEnter(this);
		public void OnPointerExit(PointerEventData eventData) => tabGroup.OnTabExit(this);
		public void OnPointerClick(PointerEventData eventData) => tabGroup.SelectTab(this);
	}
}