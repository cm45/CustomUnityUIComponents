using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CustomUIComponents.Tabs
{
	[RequireComponent(typeof(Image))]
	public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
	{
		public TabGroup tabGroup;
		[HideInInspector] public Image background;

		private void OnValidate()
		{
			if (tabGroup == null)
				tabGroup = GetComponentInParent<TabGroup>();
		}

		private void Start()
		{
			background = GetComponent<Image>();
			tabGroup.Subscribe(this);
		}

		public void OnPointerEnter(PointerEventData eventData) => tabGroup.OnTabEnter(this);
		public void OnPointerClick(PointerEventData eventData) => tabGroup.OnTabSelected(this);
		public void OnPointerExit(PointerEventData eventData) => tabGroup.OnTabExit(this);
	}
}